using Core.Constants;
using Core.Entities.Identity;
using Core.Entities.Ocorrencias;
using Core.Enums;
using Core.Extensions;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Gruposlojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetById;
using Core.Features.Mercados.Queries.GetAllCached;
using Core.Features.NotificacoesOcorrencias.Commands.Create;
using Core.Features.NotificacoesOcorrencias.Commands.Delete;
using Core.Features.NotificacoesOcorrencias.Queries.GetAllCached;
using Core.Features.OcorrenciaDocuments.Commands.Create;
using Core.Features.OcorrenciaDocuments.Commands.Delete;
using Core.Features.OcorrenciaDocuments.Commands.Update;
using Core.Features.OcorrenciaDocuments.Queries.GetAllCached;
using Core.Features.OcorrenciaDocuments.Queries.GetById;
using Core.Features.Ocorrencias.Commands.Create;
using Core.Features.Ocorrencias.Commands.Delete;
using Core.Features.Ocorrencias.Commands.Update;
using Core.Features.Ocorrencias.Queries.GetAllCached;
using Core.Features.Ocorrencias.Queries.GetById;
using Core.Features.Ocorrencias.Response;
using Core.Features.TiposOcorrencias.Queries.GetAllCached;
using Core.Features.TiposOcorrenciasLocalized.Commands.Update;
using Core.Features.TiposOcorrenciasLocalized.Queries.GetAllCached;
using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Extensions;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Business.Controllers.Mercado;
using LVLgroupApp.Areas.Notification.Controllers.Notification;
using LVLgroupApp.Areas.Ocorrencia.Controllers.TipoOcorrencia;
using LVLgroupApp.Areas.Ocorrencia.Models.Ocorrencia;
using LVLgroupApp.Areas.Ocorrencia.Models.OcorrenciaDocument;
using LVLgroupApp.Areas.Ocorrencia.Models.TipoOcorrencia;
using LVLgroupApp.Areas.Vendas.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Ocorrencia.Controllers.Ocorrencia
{
    [Area("Ocorrencia")]
    [Authorize]
    public class OcorrenciaController : BaseController<OcorrenciaController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<OcorrenciaController> _localizer;

        private IWebHostEnvironment _environment;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;


        //---------------------------------------------------------------------------------------------------

        public OcorrenciaController(IWebHostEnvironment environment, IStringLocalizer<OcorrenciaController> localizer, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _localizer = localizer;
            _signInManager = signInManager;
            _environment = environment;
            _userManager = userManager;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Ocorrencias.View)]
        public async Task<IActionResult> IndexAsync()
        {
            var model = new OcorrenciaViewModel();
            model.CurrentRole = await GetCurrentRoleAsync(null);
            model.Mercados = await MercadoController.GetSelectListAllMercadosAsync(model.CurrentRole.MercadoId, _mapper, _mediator);
            model.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(model.CurrentRole.EmpresaId, _mapper, _mediator);
            model.GruposLojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(model.CurrentRole.EmpresaId, model.CurrentRole.GrupolojaId, _mapper, _mediator);
            model.Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(model.CurrentRole.GrupolojaId, model.CurrentRole.LojaId, _mapper, _mediator);
            model.Years = new List<string>() { "2021", "2022", "2023", "2024", "2025", "2026" };

            var cal = new CultureInfo(_localizer["pt-PT"]).Calendar;
            var semanaDataInicial = DateTime.Now;


            // testar se estamos na primeira hora de 2ªfeira
            // durante a primeira hora de 2ªfeira continuamos a mostrar a venda semanal anterior
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && DateTime.Now.Hour < 1)
            {
                // mostrar a venda semanal da semana anterior
                semanaDataInicial = DateTime.Now.AddDays(-1).MondayOfWeek();
                model.Mes = DateTime.Now.AddDays(-1).Month;
            }
            else
            {
                // mostrar a venda semanal da semana corrente
                semanaDataInicial = DateTime.Now.MondayOfWeek();
                model.Mes = DateTime.Now.Month;
            }
            model.MesLiteral = new CultureInfo(_localizer["pt-PT"]).DateTimeFormat.GetMonthName(model.Mes);
            model.MesLiteral = model.MesLiteral.FirstCharToUpper();
            model.NumeroDaSemana = cal.GetWeekOfYear(semanaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            model.Ano = semanaDataInicial.Year;


            // objetivo semanal

            //model.VendaSemanalId = 0;
            var vendaSemanal = await VendaSemanalController.GetVendaSemanalWithDiárias(model.CurrentRole.LojaId, model.Ano, model.NumeroDaSemana, _mapper, _mediator);
            if (vendaSemanal != null)
            {
                model.ObjetivoDaVendaSemanal = vendaSemanal.ObjetivoDaVendaSemanal;
                model.ValorTotalDaVenda = vendaSemanal.ValorTotalDaVenda;
                model.VendaSemanalId = vendaSemanal.Id;
            }
            else
            {
                model.ObjetivoDaVendaSemanal = await  VendaSemanalController.GetObjetivoDaVendaSemanal(model.CurrentRole.MercadoId, model.CurrentRole.EmpresaId, model.CurrentRole.GrupolojaId, model.CurrentRole.LojaId, model.NumeroDaSemana, model.Ano, _mediator, _mapper);
                model.ValorTotalDaVenda = await VendaSemanalController.GetValorTotalDaVenda(model.CurrentRole.MercadoId, model.CurrentRole.EmpresaId, model.CurrentRole.GrupolojaId, model.CurrentRole.LojaId, model.NumeroDaSemana, model.Ano, _mediator, _mapper);
                model.VendaSemanalId = 0;
            }


            // objetivo mensal
            model.ValorTotalMensalDaVenda = await VendaSemanalController.GetValorTotalMensalDaVenda(model.CurrentRole.MercadoId, model.CurrentRole.EmpresaId, model.CurrentRole.GrupolojaId, model.CurrentRole.LojaId, model.Mes, model.Ano, _mediator, _mapper);
            model.ObjetivoMensalDaVenda = await VendaSemanalController.GetObjetivoMensalDaVenda(model.CurrentRole.MercadoId, model.CurrentRole.EmpresaId, model.CurrentRole.GrupolojaId, model.CurrentRole.LojaId, model.Mes, model.Ano, _mediator, _mapper);
            var vendasDiariasMes = await VendaSemanalController.GetVendasMensaisAsync(model.CurrentRole.LojaId, model.Ano, model.Mes, _mapper, _mediator);
            vendasDiariasMes.RemoveAll(v => v.VendaSemanalId == model.VendaSemanalId);
            model.ValorAcumuladoMensal = vendasDiariasMes.Sum(v => v.ValorDaVenda);

            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - Index - return viewModel");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Ocorrencias.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - LoadAll - return lista vazia de OcorrenciaViewModel");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de Ocorrencias para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Ocorrencias.View)]
        public async Task<IActionResult> GetOcorrencias()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();

                var calendarFilterOption = Request.Form["filterOption"].FirstOrDefault();

                var desdedateFilter = Request.Form["desdedateFilter"].FirstOrDefault();
                var atedateFilter = Request.Form["atedateFilter"].FirstOrDefault();
                var desdeSemanaFilter = Request.Form["desdeSemanaFilter"].FirstOrDefault();
                var ateSemanaFilter = Request.Form["ateSemanaFilter"].FirstOrDefault();

                var janeiroFilter = Request.Form["janeiroFilter"].FirstOrDefault();
                var fevereiroFilter = Request.Form["fevereiroFilter"].FirstOrDefault();
                var marçoFilter = Request.Form["marçoFilter"].FirstOrDefault();
                var abrilFilter = Request.Form["abrilFilter"].FirstOrDefault();
                var maioFilter = Request.Form["maioFilter"].FirstOrDefault();
                var junhoFilter = Request.Form["junhoFilter"].FirstOrDefault();
                var julhoFilter = Request.Form["julhoFilter"].FirstOrDefault();
                var agostoFilter = Request.Form["agostoFilter"].FirstOrDefault();
                var setembroFilter = Request.Form["setembroFilter"].FirstOrDefault();
                var outubroFilter = Request.Form["outubroFilter"].FirstOrDefault();
                var novembroFilter = Request.Form["novembroFilter"].FirstOrDefault();
                var dezembroFilter = Request.Form["dezembroFilter"].FirstOrDefault();

                var quarter1Filter = Request.Form["quarter1Filter"].FirstOrDefault();
                var quarter2Filter = Request.Form["quarter2Filter"].FirstOrDefault();
                var quarter3Filter = Request.Form["quarter3Filter"].FirstOrDefault();
                var quarter4Filter = Request.Form["quarter4Filter"].FirstOrDefault();

                var ano1Filter = Request.Form["ano1Filter"].FirstOrDefault();
                var ano2Filter = Request.Form["ano2Filter"].FirstOrDefault();
                var ano3Filter = Request.Form["ano3Filter"].FirstOrDefault();
                var ano4Filter = Request.Form["ano4Filter"].FirstOrDefault();
                var ano5Filter = Request.Form["ano5Filter"].FirstOrDefault();
                var ano6Filter = Request.Form["ano6Filter"].FirstOrDefault();



                int intFilterMercado = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int intFilterEmpresa = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int intFilterGrupoloja = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int intFilterLoja = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;

                int intCalendarFilterOption = calendarFilterOption != null ? Convert.ToInt32(calendarFilterOption) : 0;

                //DateTime dateDesde = string.IsNullOrEmpty(desdedateFilter) ? DateTime.MinValue : DateTime.Parse(desdedateFilter);
                //DateTime dateAte = string.IsNullOrEmpty(atedateFilter) ? DateTime.MaxValue : DateTime.Parse(atedateFilter);

                int intFilterDesdeSemana = string.IsNullOrEmpty(desdeSemanaFilter) ? 1 : Convert.ToInt32(desdeSemanaFilter);
                int intFilterAteSemana = string.IsNullOrEmpty(ateSemanaFilter) ? 53 : Convert.ToInt32(ateSemanaFilter);

                bool boolFilterJaneiro = janeiroFilter != null ? Convert.ToBoolean(janeiroFilter) : false;
                bool boolFilterFevereiro = fevereiroFilter != null ? Convert.ToBoolean(fevereiroFilter) : false;
                bool boolFilterMarço = marçoFilter != null ? Convert.ToBoolean(marçoFilter) : false;
                bool boolFilterAbril = abrilFilter != null ? Convert.ToBoolean(abrilFilter) : false;
                bool boolFilterMaio = maioFilter != null ? Convert.ToBoolean(maioFilter) : false;
                bool boolFilterJunho = junhoFilter != null ? Convert.ToBoolean(junhoFilter) : false;
                bool boolFilterJulho = julhoFilter != null ? Convert.ToBoolean(julhoFilter) : false;
                bool boolFilterAgosto = agostoFilter != null ? Convert.ToBoolean(agostoFilter) : false;
                bool boolFilterSetembro = setembroFilter != null ? Convert.ToBoolean(setembroFilter) : false;
                bool boolFilterOutubro = outubroFilter != null ? Convert.ToBoolean(outubroFilter) : false;
                bool boolFilterNovembro = novembroFilter != null ? Convert.ToBoolean(novembroFilter) : false;
                bool boolFilterDezembro = dezembroFilter != null ? Convert.ToBoolean(dezembroFilter) : false;

                bool boolFilterQuarter1 = quarter1Filter != null ? Convert.ToBoolean(quarter1Filter) : false;
                bool boolFilterQuarter2 = quarter2Filter != null ? Convert.ToBoolean(quarter2Filter) : false;
                bool boolFilterQuarter3 = quarter3Filter != null ? Convert.ToBoolean(quarter3Filter) : false;
                bool boolFilterQuarter4 = quarter4Filter != null ? Convert.ToBoolean(quarter4Filter) : false;

                int intFilterAno1 = string.IsNullOrEmpty(ano1Filter) ? 0 : Convert.ToInt32(ano1Filter);
                int intFilterAno2 = string.IsNullOrEmpty(ano2Filter) ? 0 : Convert.ToInt32(ano2Filter);
                int intFilterAno3 = string.IsNullOrEmpty(ano3Filter) ? 0 : Convert.ToInt32(ano3Filter);
                int intFilterAno4 = string.IsNullOrEmpty(ano4Filter) ? 0 : Convert.ToInt32(ano4Filter);
                int intFilterAno5 = string.IsNullOrEmpty(ano5Filter) ? 0 : Convert.ToInt32(ano5Filter);
                int intFilterAno6 = string.IsNullOrEmpty(ano6Filter) ? 0 : Convert.ToInt32(ano6Filter);

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // lista de ocorrencias permitidas ao current user
                var allOcorrencias = await GetOcorrenciaListAsync();

                // filtrar por mercado se necessário
                if (intFilterMercado > 0)
                {
                    allOcorrencias = allOcorrencias.Where(o => o.MercadoId == intFilterMercado);
                };
                // filtrar por empresa se necessário
                if (intFilterEmpresa > 0)
                {
                    allOcorrencias = allOcorrencias.Where(v => v.EmpresaId == intFilterEmpresa);
                };
                // filtrar por grupoloja se necessário
                if (intFilterGrupoloja > 0)
                {
                    allOcorrencias = allOcorrencias.Where(v => v.GrupolojaId == intFilterGrupoloja);
                };
                // filtrar por loja se necessário
                if (intFilterLoja > 0)
                {
                    allOcorrencias = allOcorrencias.Where(v => v.LojaId == intFilterLoja);
                };



                var cal = new CultureInfo("pt-PT").Calendar;
                var estaSemanaDataInicial = DateTime.Now.MondayOfWeek();
                var semanaPassadaDataInicial = DateTime.Now.AddDays(-7).MondayOfWeek();
                var estaSemana = cal.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var semanaAnterior = estaSemana - 1;
                var esteMes = DateTime.Now.Month;
                var mesAnterior = esteMes - 1;
                var esteTrimestre = (esteMes + 2) / 3;
                var trimestreAntrior = esteTrimestre - 1;
                var esteAno = DateTime.Now.Year;
                var anoAnterior = esteAno - 1;



                switch (intCalendarFilterOption)
                {
                    case 0:     // "Utilizar filtro"
                        // filtrar por year se necessário
                        if ((intFilterAno1 > 0) || (intFilterAno2 > 0) || (intFilterAno3 > 0) || (intFilterAno4 > 0) || (intFilterAno5 > 0) || (intFilterAno6 > 0))
                        {
                            allOcorrencias = allOcorrencias.Where(o => o.DataOcorrencia.Year == intFilterAno1 || o.DataOcorrencia.Year == intFilterAno2 || o.DataOcorrencia.Year == intFilterAno3 || o.DataOcorrencia.Year == intFilterAno4 || o.DataOcorrencia.Year == intFilterAno5 || o.DataOcorrencia.Year == intFilterAno6);
                        };

                        // filtrar por quarter se necessário
                        if (boolFilterQuarter1 || boolFilterQuarter2 || boolFilterQuarter3 || boolFilterQuarter4)
                        {
                            allOcorrencias = allOcorrencias.Where(o => (boolFilterQuarter1 && ((o.DataOcorrencia.Month + 2)/3) == 1) || (boolFilterQuarter2 && ((o.DataOcorrencia.Month + 2) / 3) == 2) || (boolFilterQuarter3 && ((o.DataOcorrencia.Month + 2) / 3) == 3) || (boolFilterQuarter4 && ((o.DataOcorrencia.Month + 2) / 3) == 4));
                        };

                        // filtrar por month se necessário
                        if (boolFilterJaneiro || boolFilterFevereiro || boolFilterMarço ||
                            boolFilterAbril || boolFilterMaio || boolFilterJunho ||
                            boolFilterJulho || boolFilterAgosto || boolFilterSetembro ||
                            boolFilterOutubro || boolFilterNovembro || boolFilterDezembro)
                        {
                            allOcorrencias = allOcorrencias.Where(o => (boolFilterJaneiro && o.DataOcorrencia.Month == 1) || (boolFilterFevereiro && o.DataOcorrencia.Month == 2) || (boolFilterMarço && o.DataOcorrencia.Month == 3) ||
                                                                       (boolFilterAbril && o.DataOcorrencia.Month == 4) || (boolFilterMaio && o.DataOcorrencia.Month == 5) || (boolFilterJunho && o.DataOcorrencia.Month == 6) ||
                                                                       (boolFilterJulho && o.DataOcorrencia.Month == 7) || (boolFilterAgosto && o.DataOcorrencia.Month == 8) || (boolFilterSetembro && o.DataOcorrencia.Month == 9) ||
                                                                       (boolFilterOutubro && o.DataOcorrencia.Month == 10) || (boolFilterNovembro && o.DataOcorrencia.Month == 11) || (boolFilterDezembro && o.DataOcorrencia.Month == 12));
                        };

                        // filtrar por Semana se necessário
                        if (intFilterDesdeSemana > 1)
                        {
                            allOcorrencias = allOcorrencias.Where(o => cal.GetWeekOfYear(o.DataOcorrencia, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) >= intFilterDesdeSemana);
                        };

                        if (intFilterAteSemana < 53)
                        {
                            allOcorrencias = allOcorrencias.Where(o => cal.GetWeekOfYear(o.DataOcorrencia, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) <= intFilterAteSemana);
                        };

                        // filtrar por Data se necessário
                        //allOcorrencias = allOcorrencias.Where(o => o.DataOcorrencia >= dateDesde);
                        //allOcorrencias = allOcorrencias.Where(o => o.DataOcorrencia <= dateAte);
                        break;
                    case 1:     // "Esta semana"
                        allOcorrencias = allOcorrencias.Where(o => (o.DataOcorrencia.Year == esteAno) && (cal.GetWeekOfYear(o.DataOcorrencia, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == estaSemana));
                        break;
                    case 2:     // "Semana anterior"
                        if (semanaAnterior == 0)
                        {
                            // semana anterior é a do ano passado
                            semanaAnterior = cal.GetWeekOfYear(semanaPassadaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                        };
                        allOcorrencias = allOcorrencias.Where(o => (o.DataOcorrencia.Year == esteAno) && (cal.GetWeekOfYear(o.DataOcorrencia, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) == semanaAnterior));
                        break;
                    case 3:     // "Este mês"
                        allOcorrencias = allOcorrencias.Where(o => (o.DataOcorrencia.Year == esteAno) && ( o.DataOcorrencia.Month == esteMes));
                        break;
                    case 4:     // "Mês anterior"
                        if (mesAnterior == 0)
                        {
                            allOcorrencias = allOcorrencias.Where(o => (o.DataOcorrencia.Year == anoAnterior) && (o.DataOcorrencia.Month == 12));
                        }
                        else
                        {
                            allOcorrencias = allOcorrencias.Where(o => (o.DataOcorrencia.Year == esteAno) && (o.DataOcorrencia.Month == mesAnterior));
                        };
                        break;
                    case 5:     // "Este trimestre"
                        allOcorrencias = allOcorrencias.Where(o => (o.DataOcorrencia.Year == esteAno) && ((o.DataOcorrencia.Month + 2) / 3 == esteTrimestre));
                        break;
                    case 6:     // "Trimestre anterior"
                        if (trimestreAntrior == 0)
                        {
                            allOcorrencias = allOcorrencias.Where(o => (o.DataOcorrencia.Year == anoAnterior) && ((o.DataOcorrencia.Month + 2) / 3 == 4));
                        }
                        else
                        {
                            allOcorrencias = allOcorrencias.Where(o => (o.DataOcorrencia.Year == esteAno) && ((o.DataOcorrencia.Month + 2) / 3 == trimestreAntrior));
                        };
                        break;
                    case 7:     // "Este ano"
                        allOcorrencias = allOcorrencias.Where(o => o.DataOcorrencia.Year == esteAno);
                        break;
                    case 8:     // "Ano anterior"
                        allOcorrencias = allOcorrencias.Where(o => o.DataOcorrencia.Year == anoAnterior);
                        break;
                }



                // construir lista para View Model

                var responseAllMercados = await _mediator.Send(new GetAllMercadosCachedQuery());
                if (!responseAllMercados.Succeeded) return new ObjectResult(new { status = "error" });
                var allMercados = _mapper.Map<List<Core.Entities.Business.Mercado>>(responseAllMercados.Data).AsQueryable();

                var responseAllEmpresas = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (!responseAllEmpresas.Succeeded) return new ObjectResult(new { status = "error" });
                var allEmpresas = _mapper.Map<List<Core.Entities.Business.Empresa>>(responseAllEmpresas.Data).AsQueryable();

                var responseAllGrupolojas = await _mediator.Send(new GetAllGruposlojasCachedQuery());
                if (!responseAllGrupolojas.Succeeded) return new ObjectResult(new { status = "error" });
                var allGruposlojas = _mapper.Map<List<Core.Entities.Business.Grupoloja>>(responseAllGrupolojas.Data).AsQueryable();

                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded) return new ObjectResult(new { status = "error" });
                var allLojas = _mapper.Map<List<Core.Entities.Business.Loja>>(responseAllLojas.Data).AsQueryable();

                var responseAllTipos = await _mediator.Send(new GetAllTiposOcorenciasCachedQuery());
                if (!responseAllTipos.Succeeded) return new ObjectResult(new { status = "error" });
                var allTipos = _mapper.Map<List<Core.Entities.Ocorrencias.TipoOcorrencia>>(responseAllTipos.Data).AsQueryable();

                // Culture contains the information of the requested culture
                var culture = _culture.RequestCulture.Culture;

                var ocorrenciaData = from ocorr in allOcorrencias
                                     join m in allMercados on ocorr.MercadoId equals m.Id into mlist
                                     from merc in mlist.DefaultIfEmpty()
                                     join e in allEmpresas on ocorr.EmpresaId equals e.Id into elist
                                     from emp in elist.DefaultIfEmpty()
                                     join g in allGruposlojas on ocorr.GrupolojaId equals g.Id into glist
                                     from grp in glist.DefaultIfEmpty()
                                     join l in allLojas on ocorr.LojaId equals l.Id into llist
                                     from loj in llist.DefaultIfEmpty()
                                     join t in allTipos on ocorr.TipoOcorrenciaId equals t.Id into tlist
                                     from to in tlist.DefaultIfEmpty()
                                     select new OcorrenciaViewModel()
                                     {
                                         Id = ocorr.Id,
                                         CodeId = ocorr.CodeId,
                                         TipoOcorrenciaId = ocorr.TipoOcorrenciaId,
                                         CategoriaId = ocorr.CategoriaId,
                                         CategoriaNome = OcorrenciaCategoriaList.GetCategoriaName(culture.Name, ocorr.CategoriaId),
                                         StatusId = ocorr.StatusId,
                                         StatusNome = OcorrenciaStatusList.GetStatusName(culture.Name, ocorr.StatusId),
                                         OcorrenciaNome = ocorr.OcorrenciaNome,
                                         DataOcorrencia = ocorr.DataOcorrencia,
                                         MercadoId = ocorr.MercadoId,
                                         MercadoNome = merc.Nome,
                                         EmpresaId = ocorr.EmpresaId,
                                         EmpresaLogo = Convert.ToBase64String(emp.LogoPicture),
                                         EmpresaNome = emp.Nome,
                                         GrupolojaId = ocorr.GrupolojaId,
                                         GrupolojaNome = grp.Nome,
                                         LojaId = ocorr.LojaId,
                                         LojaNome = loj.Nome
                                     };

                // filtrar searchValue
                if (!string.IsNullOrEmpty(searchValue))
                {
                    ocorrenciaData = ocorrenciaData.Where(x => x.MercadoNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                    x.EmpresaNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                    x.GrupolojaNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                    x.LojaNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase)
                    );
                }

                // ordenar lista
                var sortedOcorrenciaData = ocorrenciaData.AsQueryable();
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    sortedOcorrenciaData = sortedOcorrenciaData.OrderBy(sortColumn + " " + sortColumnDirection);
                }

                // retornar lista para a datatable
                recordsTotal = sortedOcorrenciaData.Count();
                var data = sortedOcorrenciaData.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - GetOcorrencias - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Ocorrencias.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar nova Ocorrencia
            {
                var ocorrenciaFolder = Guid.NewGuid().ToString();
                var folderCriado = CreateFolder(ocorrenciaFolder);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetCreateOrEdit - Foi criado o Folder temporário");

                var ocorrenciaViewModel = await InitNewOcorrenciaAsync(ocorrenciaFolder);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetCreateOrEdit - return _CreateOrEdit para criar nova Ocorrencia");
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", ocorrenciaViewModel) });
            }
            else // Editar Ocorrencia
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetCreateOrEdit - Entrou para editar claim id=" + id);

                var response = await _mediator.Send(new GetOcorrenciaByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var ocorrenciaViewModel = await MapperEntitieToModelOcorrenciaAsync(response.Data);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetCreateOrEdit - return _CreateOrEdit para editar Ocorrencia");
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", ocorrenciaViewModel) });
                }
                else
                {
                    _notify.Error(response.Message);
                    return new JsonResult(new { isValid = false, html = string.Empty });
                }
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Ocorrencias.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, OcorrenciaViewModel ocorrenciaViewModel)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Entrou para post da ocorrencia=" + id);

            // validar ModelState
            if (!ModelState.IsValid)
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - ModelState Not Valid");
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Total erros = " + ModelState.ErrorCount);

                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Error Key = " + key);
                    }
                }

                // Current Role
                ocorrenciaViewModel.CurrentRole = await GetCurrentRoleAsync(null);

                // Ocorrencia
                ocorrenciaViewModel.EditMode = false;
                ocorrenciaViewModel.Empresas = await InitEmpresasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.EmpresaId, false);
                ocorrenciaViewModel.GruposLojas = await InitGrupoLojasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.EmpresaId, ocorrenciaViewModel.GrupolojaId, false);
                ocorrenciaViewModel.Lojas = await InitLojasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.GrupolojaId, ocorrenciaViewModel.LojaId, false);
                ocorrenciaViewModel.Mercados = await MercadoController.GetSelectListAllMercadosAsync(ocorrenciaViewModel.MercadoId, _mapper, _mediator);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - returm _CreateOrEdit");
                var html1 = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", ocorrenciaViewModel);
                return new JsonResult(new { isValid = false, html = html1 });
            }

            // validar Categoria Interlojas
            if (ocorrenciaViewModel.CategoriaId == 2 && (ocorrenciaViewModel.ToLojaId == 0 || ocorrenciaViewModel.ToLojaId == ocorrenciaViewModel.LojaId))
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - InterLoja Not Valid");

                // Current Role
                ocorrenciaViewModel.CurrentRole = await GetCurrentRoleAsync(null);

                // Ocorrencia
                ocorrenciaViewModel.EditMode = false;
                ocorrenciaViewModel.Empresas = await InitEmpresasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.EmpresaId, false);
                ocorrenciaViewModel.GruposLojas = await InitGrupoLojasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.EmpresaId, ocorrenciaViewModel.GrupolojaId, false);
                ocorrenciaViewModel.Lojas = await InitLojasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.GrupolojaId, ocorrenciaViewModel.LojaId, false);
                ocorrenciaViewModel.Mercados = await MercadoController.GetSelectListAllMercadosAsync(ocorrenciaViewModel.MercadoId, _mapper, _mediator);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - returm _CreateOrEdit");
                var html2 = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", ocorrenciaViewModel);
                return new JsonResult(new { isValid = false, html = html2 });
            }



            if (id > 0)
            {
                // editar ocorrencia
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Entrou para post edit da ocorrencia=" + id);

                //converter dados das tabs em claim
                var ocorr = await MapperViewModelToEntitieOcorrenciaAsync(ocorrenciaViewModel);

                //Update Ocorrencia
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - vai fazer update da Ocorrencia editada");

                //atualizar CodeId
                ocorr.Id = id;
                ocorr.CodeId = await CodeGenerationAsync(ocorr.Id, ocorr.DataOcorrencia, ocorr.LojaId);

                // rename folder original se necessário
                if (!String.Equals(ocorr.CodeId, ocorrenciaViewModel.CodeId))
                {
                    var folderRenamed = RenameFolder(ocorrenciaViewModel.CodeId, ocorr.CodeId);
                    await UpdatePathFicheirosInOcorrenciaAsync(ocorrenciaViewModel.CodeId, ocorr.CodeId, ocorr.Id);
                }

                // update db
                var updateOcorrenciaCommand = _mapper.Map<UpdateOcorrenciaCommand>(ocorr);
                var result = await _mediator.Send(updateOcorrenciaCommand);
                if (result.Succeeded)
                {
                    _notify.Information($"{_localizer["A Ocorrência"]} {result.Data} {_localizer["foi atualizada com sucesso."]}");
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Claim editada foi escrita na db");
                }
                else
                {
                    _notify.Error($"{_localizer["A Ocorrência"]} {result.Data} {_localizer["não foi atualizada"]}");
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Erro:Claim não foi atualizada");
                }

            }
            else
            {
                // criar ocorrencia
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Entrou para post create da ocorrencia=" + id);
                // converter viewModel em ocorrencia
                var ocorr = await MapperViewModelToEntitieOcorrenciaAsync(ocorrenciaViewModel);

                // comando Criar Ocorrencia
                var createOcorrenciaCommand = _mapper.Map<CreateOcorrenciaCommand>(ocorr);
                var result = await _mediator.Send(createOcorrenciaCommand);
                if (!result.Succeeded)
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Erro:Ocorrência não foi criada: " + result.Message);
                    _notify.Error(_localizer["Erro:Ocorrência não foi criada"]);

                    // return Index View
                    var model1 = new OcorrenciaViewModel();
                    model1.Mercados = await MercadoController.GetSelectListAllMercadosAsync(0, _mapper, _mediator);
                    model1.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, _mapper, _mediator);
                    model1.Years = new List<string>() { "2021", "2022", "2023", "2024", "2025", "2026" };
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - Index - return viewModel");

                    var html1 = await _viewRenderer.RenderViewToStringAsync("_ViewAll", model1);
                    return new JsonResult(new { isValid = true, html = html1 });
                }

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Ocorrencia criada");

                ocorr.Id = result.Data;
                ocorr.CodeId = await CodeGenerationAsync(ocorr.Id, ocorr.DataOcorrencia, ocorr.LojaId);

                // rename folder temporário para CodeId
                RenameFolder(ocorrenciaViewModel.OcorrenciaFolder, ocorr.CodeId);
                ocorr.OcorrenciaFolder = ocorr.CodeId;

                // registar ficheiros na Ocorrencia
                ocorr.TotalFicheiros = await SetFicheirosInOcorrenciaAsync(ocorrenciaViewModel.OcorrenciaFolder, ocorr.CodeId, ocorr.Id);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Ficheiros registados na Ocorrencia (db)");

                var updateOcorrenciaCommand = _mapper.Map<UpdateOcorrenciaCommand>(ocorr);
                var resultUpdt = await _mediator.Send(updateOcorrenciaCommand);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Ocorrencia updated");

                if (!resultUpdt.Succeeded)
                {
                    _notify.Error($"{_localizer["A ocorrência com o Id"]} {resultUpdt.Data} {_localizer["não foi atualizada."]}");
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Erro:Ocorrência não foi criada");

                    // return Index View
                    var model2 = new OcorrenciaViewModel();
                    model2.Mercados = await MercadoController.GetSelectListAllMercadosAsync(0, _mapper, _mediator);
                    model2.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, _mapper, _mediator);
                    model2.Years = new List<string>() { "2021", "2022", "2023", "2024", "2025", "2026" };
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - Index - return viewModel");
                    var html2 = await _viewRenderer.RenderViewToStringAsync("_ViewAll", model2);
                    return new JsonResult(new { isValid = true, html = html2 });
                }

                _notify.Success($"{_localizer["A ocorrência com o Id"]} {resultUpdt.Data} {_localizer["foi criada com sucesso."]}");

                // handler da ocorrencia
                switch (ocorr.CategoriaId)
                {
                    case 1:
                        // Simples
                        await HandlerNewOcorrenciaSimplesAsync(ocorr);
                        break;
                    case 2:
                        // Interlojas
                                
                        // criar ocorrencia SLAVE em outra loja
                        var slaveOcorr = await GetOcorrenciaSlaveAsync(ocorrenciaViewModel);
                        slaveOcorr.InterOcorrenciaId = ocorr.Id;
                        var slaveOcorrenciaFolder = Guid.NewGuid().ToString();
                        var folderCriado = CreateFolder(slaveOcorrenciaFolder);
                        slaveOcorr.TotalFicheiros = CopyDocumentToSlaveOcorrFolder(ocorr.OcorrenciaFolder, slaveOcorrenciaFolder);

                        // comando Criar Ocorrencia
                        var createSlaveOcorrenciaCommand = _mapper.Map<CreateOcorrenciaCommand>(slaveOcorr);
                        var resultSlave = await _mediator.Send(createSlaveOcorrenciaCommand);
                        if (!resultSlave.Succeeded)
                        {
                            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostCreateOrEdit - Erro:Ocorrência remota não foi criada: " + resultSlave.Message);
                            _notify.Error(_localizer["Erro:Ocorrência remota não foi criada"]);

                            // return Index View
                            var model1 = new OcorrenciaViewModel();
                            model1.Mercados = await MercadoController.GetSelectListAllMercadosAsync(0, _mapper, _mediator);
                            model1.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, _mapper, _mediator);
                            model1.Years = new List<string>() { "2021", "2022", "2023", "2024", "2025", "2026" };
                            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - Index - return viewModel");

                            var html1 = await _viewRenderer.RenderViewToStringAsync("_ViewAll", model1);
                            return new JsonResult(new { isValid = true, html = html1 });
                        }

                        slaveOcorr.Id = resultSlave.Data;
                        slaveOcorr.CodeId = await CodeGenerationAsync(slaveOcorr.Id, slaveOcorr.DataOcorrencia, slaveOcorr.LojaId);
                        // rename folder temporário para CodeId
                        RenameFolder(ocorrenciaViewModel.OcorrenciaFolder, ocorr.CodeId);
                        // registar ficheiros na Ocorrencia
                        await SetFicheirosInOcorrenciaAsync(slaveOcorrenciaFolder, slaveOcorr.CodeId, slaveOcorr.Id);

                        await HandlerNewOcorrenciaInterlojasAsync(ocorr, slaveOcorr);
                        break;
                    case 3:
                        // Loja-Sede
                        HandlerNewOcorrenciaLojaSede(ocorr);
                        break;
                    default:
                        break;
                }

            }

            // return Index View
            var model = new OcorrenciaViewModel();
            model.Mercados = await MercadoController.GetSelectListAllMercadosAsync(0, _mapper, _mediator);
            model.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, _mapper, _mediator);
            model.Years = new List<string>() { "2021", "2022", "2023", "2024", "2025", "2026" };
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - Index - return viewModel");

            var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", model);
            return new JsonResult(new { isValid = true, html = html });
            
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Chamada por ajax call a partir da view _CreateOrEdit.
        /// É passado o Id da ocorrência a ser removida
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// IActionResult com o file e status code 200 ou 404 de file não encontrado.
        /// Se erro retorna staus code 500 com mensagem de erro.
        /// </returns>

        [Authorize(Policy = Permissions.Ocorrencias.Create)]
        public IActionResult DownloadFile(string fileName, string ocorrenciaFolder)
        {
            if (string.IsNullOrEmpty(fileName)) return BadRequest();
            try
            {
                var wwwPath = _environment.WebRootPath;
                var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");
                var dirPath = Path.Combine(contentPath, ocorrenciaFolder);
                var filePath = dirPath + "/" + fileName;

                if (!System.IO.File.Exists(filePath)) return NotFound();

                var contentType = GetContentType(filePath);
                return PhysicalFile(filePath, contentType, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DownloadFile - IO exception vai sair e retornar Error: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Chamada por ajax call a partir da view _CreateOrEdit.
        /// É passado o Id da ocorrência a ser removida
        /// </summary>
        /// <param name="id"></param>
        /// <returns>"JsonResult = _ViewAll"</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Ocorrencias.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            try
            {
                // verificar se ocorrência existe
                var ocorrenciaCommand = await _mediator.Send(new GetOcorrenciaByIdQuery { Id = id });
                if (!ocorrenciaCommand.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete - Erro ao ler ocorrencia: " + ocorrenciaCommand.Message);
                    _notify.Error($"{_localizer["A ocorrência com o Id"]} {id} {_localizer[" não foi removida:"]} {ocorrenciaCommand.Message}");
                    return new JsonResult(new { isValid = false, html = "" });
                }

                var ocorrenciaFolder = ocorrenciaCommand.Data.OcorrenciaFolder;

                // ler da db os documents da ocorrencia
                var allDocumentsCommand = await _mediator.Send(new GetAllOcorrenciaDocumentsByOcorrenciaIdQuery { OcorrenciaId = id });
                if (!allDocumentsCommand.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete -  Error: " + allDocumentsCommand.Message);
                    return new JsonResult(new { isValid = false, html = "" });
                }

                // remover documents da ocorrencia
                var allDocuments = _mapper.Map<List<OcorrenciaDocumentViewModel>>(allDocumentsCommand.Data);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete - Total de documents a remover: " + allDocuments.Count);
                foreach (var document in allDocuments)
                {
                    // remover documento da db
                    var deleteDocumentCommand = await _mediator.Send(new DeleteOcorrenciaDocumentCommand { Id = document.Id });
                    if (!deleteDocumentCommand.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete - Erro ao remover ficheiro da db: " + deleteDocumentCommand.Message);
                        _notify.Error(deleteDocumentCommand.Message);
                        return new JsonResult(new { isValid = false, html = "" });
                    }
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete - Ficheiro removido da db: " + document.FileName);
                }

                // apagar  folder da ocorrencia
                if (DeleteFolder(ocorrenciaFolder))
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete - Folder apagado: " + ocorrenciaFolder);
                }
                else
                {
                    _logger.LogWarning(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete - Folder não encontrado: " + ocorrenciaFolder);
                }

                // remover ocorrência
                var deleteCommand = await _mediator.Send(new DeleteOcorrenciaCommand { Id = id });
                if (!deleteCommand.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete - Erro ao remover ocorrência da db: " + deleteCommand.Message);
                    _notify.Error(deleteCommand.Message);
                    return new JsonResult(new { isValid = true, html = "" });
                }

                _notify.Information($"{_localizer["A ocorrência com o Id"]} {id} {_localizer[" foi removida."]}");

                // return _ViewAll
                var viewModel = new List<OcorrenciaViewModel>();
                var response = await _mediator.Send(new GetAllOcorenciasCachedQuery());
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<OcorrenciaViewModel>>(response.Data);

                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = false, html = "" });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnPostDelete - IO exception vai sair e retornar Error: " + ex.Message);
                return new JsonResult(new { isValid = false, html = "" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Chamada por ajax call a partir da view _CreateOrEdit.
        /// É passado o Id da ocorrência a ser editada e o folder
        /// com os documentos desta ocorrência. Prepara a lista 
        /// de todos os documents da ocorrência.
        /// </summary>
        /// <param name="ocorrenciaId"></param>
        /// <param name="ocorrenciaFolder"></param>
        /// <returns>jsonString com lista de documentos da ocorrencia</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Ocorrencias.Edit)]
        public async Task<JsonResult> OnGetViewDocuments(int ocorrenciaId = 0, string ocorrenciaFolder = "")
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Controller - OnGetViewDocumentsAsync - Entrou com  ocorrenciaId=" + ocorrenciaId + " ocorrenciaFolder=" + ocorrenciaFolder);
            var jsonString = string.Empty;

            try
            {
                var documentsviewModel = new List<OcorrenciaDocumentViewModel>();
                if (ocorrenciaId == 0)
                {
                    // ler documents da ocorrência existentes no folder temporário
                    var documentsListResponse = await _mediator.Send(new GetAllOcorrenciaDocumentsByFolderQuery() { Folder = ocorrenciaFolder });
                    if (documentsListResponse.Succeeded)
                    {
                        documentsviewModel = _mapper.Map<List<OcorrenciaDocumentViewModel>>(documentsListResponse.Data);
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetViewDocumentsAsync - get all documents in db in claimfolder=" + ocorrenciaFolder + " total=" + documentsviewModel.Count);
                    }
                    else
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetViewDocumentsAsync - Erro: Não foi possível obter a lista de documentos da ocorrência");
                    }
                }
                else
                {
                    // ler documents da ocorrência existentes no folder da ocorrência
                    var documentsListResponse = await _mediator.Send(new GetAllOcorrenciaDocumentsByOcorrenciaIdQuery() { OcorrenciaId = ocorrenciaId });
                    if (documentsListResponse.Succeeded)
                    {
                        documentsviewModel = _mapper.Map<List<OcorrenciaDocumentViewModel>>(documentsListResponse.Data);
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetViewDocumentsAsync - get all documents from ocorrência=" + ocorrenciaId + " total=" + documentsviewModel.Count);
                    }
                    else
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetViewDocumentsAsync - Erro: Não foi possível obter a lista de documentos da ocorrência");
                    }
                }

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetViewDocumentsAsync - vai sair e retornar lista de documents=" + documentsviewModel.Count);
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { success = true, documentList = documentsviewModel });
                return Json(jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - OnGetViewDocumentsAsync - IO exception vai sair e retornar lista. Error: " + ex.Message);
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { success = false, documentList = new List<OcorrenciaDocumentViewModel>() });
                return Json(jsonString);
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Atende o file upload do client.
        /// cria o folder temporário se necessário e copia para lá o ficheiro.
        /// O ficheiro é registado na bd.
        /// </summary>
        /// <param name="ocorrenciaId"></param>
        /// <param name="ocorrenciaFolder"></param>
        /// <param name="descrição"></param>
        /// <param name="fileDocument"></param>
        /// <returns>IActionResult => status = "error"/"success"</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Ocorrencias.Edit)]
        public async Task<IActionResult> UploadToFolderAsync(IFormFile fileDocument, int ocorrenciaId, string ocorrenciaFolder, string descrição)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Controller - UploadToFolderAsync folder = " + ocorrenciaFolder + " ocorreência =" + ocorrenciaId);

            if (ModelState.IsValid && fileDocument != null)
            {
                try
                {
                    var wwwPath = _environment.WebRootPath;
                    var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UploadToFolderAsync - Vai criar file no folder=" + ocorrenciaFolder + " file=" + fileDocument.FileName);

                    // colocar file no ocorrenciaFolder
                    var dirPath = Path.Combine(contentPath, ocorrenciaFolder);
                    var filePath = dirPath + "/" + fileDocument.FileName;
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        fileDocument.CopyTo(fs);
                    }
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UploadToFolderAsync - File criado com sucesso=" + filePath);

                    // registar o file na db
                    var documentViewModel = new OcorrenciaDocumentViewModel()
                    {
                        OcorrenciaId = ocorrenciaId > 0 ? ocorrenciaId : null,
                        OcorrenciaFolder = ocorrenciaFolder,
                        Descrição = descrição,
                        FileName = fileDocument.FileName,
                        FilePath = filePath,
                        UploadDate = DateTime.Today
                    };
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UploadToFolderAsync - Vai registar o file na db em Ocorrencia=" + documentViewModel.OcorrenciaId + " file=" + documentViewModel.FileName);
                    var createDocumentCommand = _mapper.Map<CreateOcorrenciaDocumentCommand>(documentViewModel);
                    var result = await _mediator.Send(createDocumentCommand);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UploadToFolderAsync - OcorrenciaDocument criado com sucesso na db=" + documentViewModel.FileName + " ocorrenciaDocumentId=" + result.Data);

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UploadToFolderAsync - Vai sair e retornar success");
                    return new ObjectResult(new { status = "success" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UploadToFolderAsync - IO exception vai sair e retornar Error: " + ex.Message);
                    return new ObjectResult(new { status = "error" });
                }
            }
            else
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UploadToFolderAsync - ModelState IsNot Valid ou FilePhoto == null");
                return new ObjectResult(new { status = "error" });
            }

        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Atende o delete document do client.
        /// Remove o ficheiro do folder e da bd.
        /// </summary>
        /// <returns>IActionResult => status = "error"/"success"</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Ocorrencias.Delete)]
        public async Task<IActionResult> DeleteOcorrenciaDocumentAsync(int documentId)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Controller - DeleteOcorrenciaDocumentAsync document =" + documentId);

            try
            {
                if (documentId == 0)
                {
                    return new ObjectResult(new { status = "error" });
                }

                // obter document
                var response = await _mediator.Send(new GetOcorrenciaDocumentByIdQuery() { Id = documentId });
                if (!response.Succeeded || response.Data == null)
                {
                    return new ObjectResult(new { status = "error" });
                }

                var ocorrenciaDocument = _mapper.Map<OcorrenciaDocumentViewModel>(response.Data);
                var ocorrenciaId = (int)ocorrenciaDocument.OcorrenciaId;

                // delete file no ocorrenciaFolder
                var wwwPath = _environment.WebRootPath;
                var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - Vai remover file = " + ocorrenciaDocument.FilePath);

                var dirPath = Path.Combine(contentPath, ocorrenciaDocument.OcorrenciaFolder);
                var filePath = dirPath + "/" + ocorrenciaDocument.FileName;
                if (Directory.Exists(dirPath))
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - Folder existe= " + dirPath);
                    DirectoryInfo diSource = new DirectoryInfo(dirPath);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - Existem " + diSource.GetFiles().Length + " em " + dirPath);

                    // delete document no file System
                    var fiDoc = diSource.GetFiles().Where(f => f.Name == ocorrenciaDocument.FileName).FirstOrDefault();
                    if (fiDoc != null)
                    {
                        fiDoc.Delete();
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - File deleted= " + fiDoc.Name);
                    }
                    else
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - File not found= " + ocorrenciaDocument.FileName);
                    }

                    // delete document na db
                    var deleteCommand = await _mediator.Send(new DeleteOcorrenciaDocumentCommand { Id = documentId });
                    if (deleteCommand.Succeeded)
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - Document na db deleted= " + documentId);

                        // atualizar total de ficheiros da ocorrência
                        // obter ocorrência
                        var responseOcorr = await _mediator.Send(new GetOcorrenciaByIdQuery() { Id = ocorrenciaId });
                        if (!responseOcorr.Succeeded || response.Data == null)
                        {
                            return new ObjectResult(new { status = "error" });
                        }
                        var ocorrencia = _mapper.Map<OcorrenciaViewModel>(responseOcorr.Data);
                        ocorrencia.TotalFicheiros = diSource.GetFiles().Length;

                        // escrever ocorrência na db
                        var updateOcorrenciaCommand = _mapper.Map<UpdateOcorrenciaCommand>(ocorrencia);
                        var resultUpdOcorr = await _mediator.Send(updateOcorrenciaCommand);
                        if (resultUpdOcorr.Succeeded)
                        {
                            _notify.Information($"{_localizer["A Ocorrência"]} {resultUpdOcorr.Data} {_localizer["foi atualizada com sucesso."]}");
                            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - Ocorrência foi escrita na db");
                        }
                        else
                        {
                            _notify.Error($"{_localizer["A Ocorrência"]} {resultUpdOcorr.Data} {_localizer["não foi atualizada"]}");
                            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - Erro:Ocorrência não foi atualizada");
                        }

                        return new ObjectResult(new { status = "success" });
                    }
                    else
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - Erro ao fazer delete do document na db : vai retornar error");
                        return new ObjectResult(new { status = "error" });
                    }
                }
                else
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Foto Contoller - DeleteFotoInFolderAsync - Diretoria não existe. Vai sair e retornar error");
                    return new ObjectResult(new { status = "error" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteOcorrenciaDocumentAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }

        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Handler da Notificação de Ocorrência Categoria = Simples.
        /// Apenas envia notificação para os destinatários.
        /// A Ocorrencia já foi criada na db e fica com Status Termina
        /// </summary>
        /// <returns></returns>

        internal async Task<bool> HandlerNewOcorrenciaSimplesAsync(Core.Entities.Ocorrencias.Ocorrencia ocorr)
        {
            try
            {
                //create new Notification
                var notification = new Core.Entities.Notifications.Notification
                {
                    FromUserId = _signInManager.UserManager.GetUserId(User),
                    Date = DateTime.Now,
                    Subject = ocorr.CodeId,
                    Text = await GetNotificationBodyAsync(ocorr)
                };

                // recolher users a notificar
                var userList = await GetUserListToNotify(ocorr);

                // notificar cada user
                return await NotificationController.SendNotification(notification, userList, _mapper, _mediator, _logger);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Controller - HandlerNewOcorrenciaSimplesAsync error = " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Handler da Notificação de Ocorrência Categoria = Interlojas.
        /// Envia notificação para os destinatários.
        /// As duas Ocorrencias já foram criadas na db e ficam com Status
        /// "PEDIDO ENVIADO" e "PEDIDO POR ATENDER" respectivamente.
        /// </summary>
        /// <returns></returns>

        internal async Task<bool> HandlerNewOcorrenciaInterlojasAsync(Core.Entities.Ocorrencias.Ocorrencia masterOcorr, Core.Entities.Ocorrencias.Ocorrencia slaveOcorr)
        {
            try
            {
                //create new Notification para a Ocorrência Master
                var notificationMaster = new Core.Entities.Notifications.Notification
                {
                    FromUserId = _signInManager.UserManager.GetUserId(User),
                    Date = DateTime.Now,
                    Subject = masterOcorr.CodeId + " : " + masterOcorr.OcorrenciaNome,
                    Text = masterOcorr.Descrição
                };

                // recolher users a notificar
                var userListMaster = await GetUserListToNotify(masterOcorr);

                // notificar cada user
                await NotificationController.SendNotification(notificationMaster, userListMaster, _mapper, _mediator, _logger);


                //create new Notification para a Ocorrência Slave
                var notificationSlave = new Core.Entities.Notifications.Notification
                {
                    FromUserId = _signInManager.UserManager.GetUserId(User),
                    Date = DateTime.Now,
                    Subject = slaveOcorr.CodeId + " : " + masterOcorr.OcorrenciaNome,
                    Text = slaveOcorr.Descrição
                };

                // recolher users a notificar
                var userListSlave = await GetUserListToNotify(masterOcorr);

                // notificar cada user
                await NotificationController.SendNotification(notificationSlave, userListSlave, _mapper, _mediator, _logger);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Controller - HandlerNewOcorrenciaInterlojasAsync error = " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Handler da Notificação de Ocorrência Categoria = Simples.
        /// Apenas envia notificação para os destinatários.
        /// A Ocorrencia já foi criada na db e fica com Status Termina
        /// </summary>
        /// <param name="ocorr"></param>
        /// <returns>success = bool</returns>

        internal bool HandlerNewOcorrenciaLojaSede(Core.Entities.Ocorrencias.Ocorrencia ocorr)
        {


            return true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria uma lista de todos os users a serem notificados
        /// </summary>
        /// <param name="tipoOcorrenciaId"></param>
        /// <returns>List<string> de users a notificar</returns>

        internal async Task<List<string>> GetUserListToNotify(Core.Entities.Ocorrencias.Ocorrencia ocorr)
        {
            //lista de userIds
            var userIds = new List<string>();

            //tipoOcorrencia is valid
            if (ocorr.TipoOcorrenciaId <= 0) return userIds;

            try
            {
                // ler NotificacaoOcorrencias pertencentes a tipoOcorrencia da db
                var response = await _mediator.Send(new GetNotificacoesOcorrenciasByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = ocorr.TipoOcorrenciaId});
                if (!response.Succeeded || response.Data == null) return userIds;

                //construir lista de users a notificar
                //userIds = response.Data.Select(o => o.ApplicationUserId).ToList();
                userIds = response.Data.Where(o => o.ApplicationUserId != null).Select(o => o.ApplicationUserId).ToList();
                foreach (var no in response.Data)
                {
                    switch (no.TipoDestino)
                    {
                        case NotificationDestinationType.ToMyLoja:
                            // obter users da loja
                            userIds.AddRange(NotificationController.GetToMyLojaUsers(ocorr.LojaId, _context, _logger, _sessionId, _sessionName));
                            break;
                        case NotificationDestinationType.ToMyGrupoloja:
                            // obter users do grupoloja
                            userIds.AddRange(NotificationController.GetToMyGrupoLojaUsers(ocorr.GrupolojaId, _context, _logger, _sessionId, _sessionName));
                            break;
                        case NotificationDestinationType.ToMyEmpresa:
                            // obter users da empresa
                            userIds.AddRange(NotificationController.GetToMyEmpresaUsers(ocorr.EmpresaId, _context, _logger, _sessionId, _sessionName));
                            break;
                        case NotificationDestinationType.ToMyGerenteloja:
                            // obter todos gerenteLoja da loja
                            userIds.AddRange(await NotificationController.GetToMyGerentelojaUsersAsync(ocorr.LojaId, _userManager, _logger, _sessionId, _sessionName));
                            break;
                        case NotificationDestinationType.ToMySupervisor:
                            // obter todos supervisor da loja
                            userIds.AddRange(await NotificationController.GetToMySupervisorUsersAsync(ocorr.GrupolojaId, _userManager, _logger, _sessionId, _sessionName));
                            break;
                        case NotificationDestinationType.ToRevisores:
                            // obter todos supervisor da loja
                            userIds.AddRange(await NotificationController.GetToRevisoresUsersAsync(_userManager, _logger, _sessionId, _sessionName));
                            break;
                        case NotificationDestinationType.ToAdmins:
                            // obter todos supervisor da loja
                            userIds.AddRange(await NotificationController.GetToAdminsUsersAsync(_userManager, _logger, _sessionId, _sessionName));
                            break;
                        case NotificationDestinationType.ToSuperAdmins:
                            // obter todos supervisor da loja
                            userIds.AddRange(await NotificationController.GetToSuperAdminsUsersAsync(_userManager, _logger, _sessionId, _sessionName));
                            break;
                        case NotificationDestinationType.ToAll:
                            // obter todos os users
                            userIds.AddRange(NotificationController.GetToAllUsers(_context, _logger, _sessionId, _sessionName));
                            break;
                    }
                }

                var noDupsList = new HashSet<string>(userIds).ToList();
                return noDupsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Controller - GetUserListToNotify error = " + ex.Message);
                return new List<string>();
            }

        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara a lista de ocorrencias existentes tendo em conta 
        /// o role do user corrente.
        /// a tabela de ocorrencias é carregada com esta lista em _ViewAll
        /// </summary>
        /// <param></param>
        /// <returns>IQueryable de OcorrenciaViewModel</returns>

        internal async Task<IQueryable<OcorrenciaViewModel>> GetOcorrenciaListAsync()
        {
            var userId = _signInManager.UserManager.GetUserId(User);
            var currentUser = await _signInManager.UserManager.FindByIdAsync(userId);

            var isSuperAdmin = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.SuperAdmin.ToString());
            var isAdmin = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Admin.ToString());
            var isSupervisor = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Supervisor.ToString());
            var isRevisor = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Revisor.ToString());
            var isGerenteLoja = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.GerenteLoja.ToString());
            var isColaborador = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Colaborador.ToString());
            var isBasic = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Basic.ToString());


            var empresaId = currentUser.EmpresaId == null ? 0 : (int)currentUser.EmpresaId;
            var grupolojaId = currentUser.GrupolojaId == null ? 0 : (int)currentUser.GrupolojaId;
            var lojaId = currentUser.LojaId == null ? 0 : (int)currentUser.LojaId;

            var viewModelList = new List<OcorrenciaViewModel>().AsQueryable();

            if (isSuperAdmin || isAdmin || isRevisor) // todas as ocorrencias
            {
                var response = await _mediator.Send(new GetAllOcorenciasCachedQuery());
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<OcorrenciaViewModel>>(response.Data).AsQueryable();
                }
            }

            if (isSupervisor) // ocorrencias de grupoloja
            {
                var response = await _mediator.Send(new GetOcorrenciasByGrupolojaIdQuery() { GrupolojaId = grupolojaId });
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<OcorrenciaViewModel>>(response.Data).AsQueryable();
                }
            }

            if (isGerenteLoja || isColaborador || isBasic) // ocorrencias de loja
            {
                var response = await _mediator.Send(new GetOcorrenciasByLojaIdQuery() { LojaId = lojaId });
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<OcorrenciaViewModel>>(response.Data).AsQueryable();
                }
            }

            return viewModelList;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza as NotificaçõesOcorrencia de um TipoOcorrencia
        /// verifica as que existem na db e as que são passadas do cliente
        /// remove as que já não são necessárias e cria as que faltam
        /// </summary>
        /// <param name="tipoOcorrenciaId"></param>
        /// <param name="strGroup"></param>
        /// <param name="strUserId"></param>
        /// <param name="strEmail"></param>
        /// <returns>success = bool</returns>

        internal async Task<bool> UpdateNotificationOcorr(int tipoOcorrenciaId, string strGroup, string strUserId, string strEmail)
        {
            // criar arrays de strings
            var arrayGroupDest = Array.Empty<string>();
            var arrayUserIdDest = Array.Empty<string>();
            var arrayEmailDest = Array.Empty<string>();

            if (!string.IsNullOrEmpty(strGroup))
            {
                // remover último ";" de ToUserGroups
                var userGroups = strGroup.Remove(strGroup.Length - 1);
                arrayGroupDest = userGroups.Split(";");
            }
            if (!string.IsNullOrEmpty(strUserId))
            {
                // remover último ";" de ToUserIds
                var userIds = strUserId.Remove(strUserId.Length - 1);
                arrayUserIdDest = userIds.Split(";");
            }
            if (!string.IsNullOrEmpty(strEmail))
            {
                // remover último ";" de ToUserEmails
                var userEmails = strEmail.Remove(strEmail.Length - 1);
                arrayEmailDest = userEmails.Split(";");
            }

            //ler da db todas as NotificacaoOcorrencia relativas a este TipoOcorrencia
            var notifOcorrListToDelete = new List<NotificacaoOcorrenciaViewModel>();
            var notifOcorrListToMantain = new List<NotificacaoOcorrenciaViewModel>();
            var notifOcorrListQuery = new GetNotificacoesOcorrenciasByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = tipoOcorrenciaId };
            var notifOcorrListResult = await _mediator.Send(notifOcorrListQuery);
            if (notifOcorrListResult.Succeeded)
            {
                //lista com notifOcorr que podem ter que ser removidas da db
                notifOcorrListToDelete = _mapper.Map<List<NotificacaoOcorrenciaViewModel>>(notifOcorrListResult.Data);
            }

            //verificar se as novas notifOcorr já existem
            for (var i = 0; i < arrayGroupDest.Length; i++)
            {
                var existNotifOcorr1 = notifOcorrListToDelete.Where(n => n.TipoDestino.ToString() == arrayGroupDest[i]).FirstOrDefault();
                var deleted = notifOcorrListToDelete.Remove(existNotifOcorr1);
                if (deleted) notifOcorrListToMantain.Add(existNotifOcorr1);
            }
            for (var i = 0; i < arrayUserIdDest.Length; i++)
            {
                var existNotifOcorr2 = notifOcorrListToDelete.Where(n => n.ApplicationUserId == arrayUserIdDest[i]).FirstOrDefault();
                var deleted = notifOcorrListToDelete.Remove(existNotifOcorr2);
                if (deleted) notifOcorrListToMantain.Add(existNotifOcorr2);
            }

            //verificar se é necessário criar mais notifOcorr
            for (var i = 0; i < arrayGroupDest.Length; i++)
            {
                var existNotifOcorr3 = notifOcorrListToMantain.Where(n => n.TipoDestino.ToString() == arrayGroupDest[i]).FirstOrDefault();
                if (existNotifOcorr3 == null)
                {
                    var createNotifOcorrCommand = new CreateNotificacaoOcorrenciaCommand() { TipoOcorrenciaId = tipoOcorrenciaId };
                    var destType = new NotificationDestinationType();
                    var success = Enum.TryParse(arrayGroupDest[i], out destType);
                    if (success) createNotifOcorrCommand.TipoDestino = destType;
                    createNotifOcorrCommand.ApplicationUserId = null;
                    createNotifOcorrCommand.ApplicationUserEmail = null;
                    var resultCNOC = await _mediator.Send(createNotifOcorrCommand);
                }
            }
            for (var i = 0; i < arrayUserIdDest.Length; i++)
            {
                var existNotifOcorr4 = notifOcorrListToMantain.Where(n => n.ApplicationUserId == arrayUserIdDest[i]).FirstOrDefault();
                if (existNotifOcorr4 == null)
                {
                    var createNotifOcorrCommand = new CreateNotificacaoOcorrenciaCommand() { TipoOcorrenciaId = tipoOcorrenciaId };
                    createNotifOcorrCommand.TipoDestino = NotificationDestinationType.ToSingleUser;
                    createNotifOcorrCommand.ApplicationUserId = arrayUserIdDest[i];
                    createNotifOcorrCommand.ApplicationUserEmail = arrayEmailDest[i];
                    var resultCNOC = await _mediator.Send(createNotifOcorrCommand);
                }
            }

            //verificar se é necessário remover alguma notifOcorr
            foreach (var notifOcorr in notifOcorrListToDelete)
            {
                var notifOcorrToDelete = new DeleteNotificacaoOcorrenciaCommand() { Id = notifOcorr.Id };
                var resultDelete = await _mediator.Send(notifOcorrToDelete);
            }

            return true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza as strings de tradução de um TipoOcorrencia
        /// </summary>
        /// <param name="tipoOcorrenciaId"></param>
        /// <param name="tipoOcorr"></param>
        /// <returns>success = bool</returns>

        internal async Task<bool> UpdateTipoOcorrenciaLocalized(int tipoOcorrenciaId, TipoOcorrenciaViewModel tipoOcorr)
        {
            // ler as traduções existentes
            var ocorrLocalizedListQuery = new GetTiposOcorrenciasLocalizedByTipoOcorrenciaIdQuery() { TipoOcorrenciaId = tipoOcorrenciaId };
            var ocorrLocalizedListResult = await _mediator.Send(ocorrLocalizedListQuery);
            if (ocorrLocalizedListResult.Succeeded)
            {
                var ocorrLocalizedList = _mapper.Map<List<TipoOcorrenciaLocalizedViewModel>>(ocorrLocalizedListResult.Data);

                //atualizar cada uma das traduções
                foreach (var trad in ocorrLocalizedList)
                {
                    switch (trad.Language)
                    {
                        case "pt":
                            trad.Name = tipoOcorr.DefaultName;
                            break;
                        case "es":
                            trad.Name = tipoOcorr.EsName;
                            break;
                        case "en":
                            trad.Name = tipoOcorr.EnName;
                            break;
                    }

                    //atualizar a tradução
                    var updateTOLCommand = _mapper.Map<UpdateTipoOcorrenciaLocalizedCommand>(trad);
                    var result = await _mediator.Send(updateTOLCommand);
                }
                return true;
            }
            return false;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// retorna o content type de um ficheiro
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Content type = string</returns>

        internal static string GetContentType(string path)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria um folder na estrutura de pastas das Ocorrências
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns>success = bool</returns>

        internal bool CreateFolder(string folderName)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - CreateFolder - Entrou para criar o Folder=" + folderName);

                var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");
                var dirPath = Path.Combine(contentPath, folderName);
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - CreateFolder - vai retornar true com folder criado=" + dirPath);
                    //Thread.Sleep(1000);
                    return true;
                }

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - CreateFolder - vai retornar False porque folder já existia");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - CreateFolder - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// faz rename de um folder na estrutura de ocorrencias da aplicação
        /// rename oldName para newName
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns>success = bool</returns>

        public bool RenameFolder(string oldName, string newName)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - RenameFolder - Entrou para rename de old Folder=" + oldName + " para=" + newName);

            try
            {
                var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");
                var fromPath = Path.Combine(contentPath, oldName);
                var toPath = Path.Combine(contentPath, newName);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - RenameFolder - from=" + fromPath + " topath=" + toPath);

                if (Directory.Exists(fromPath))
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - RenameFolder - fromPath existe e vai retornar True");

                    Thread.Sleep(1000);
                    Directory.Move(fromPath, toPath);

                    _logger.LogInformation(_sessionId + _sessionName + " | Ocorrencia Contoller - RenameFolder - Entrou para rename de old Folder=" + " | Foto Contoller - RenameFolder - Folder renomeado com sucesso para " + toPath + "Vai sair e retornar True");
                    return true;
                }
                else
                {
                    _logger.LogError(_sessionId + _sessionName + " | Ocorrencia Contoller - RenameFolder - from=" + " | Foto Contoller - RenameFolder - Folder " + fromPath + " não existe. Vai sair e retornar False");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + _sessionName + " | Ocorrencia Contoller - RenameFolder - from=" + " | Foto Contoller - RenameFolder - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// faz delete de um folder na estrutura de ocorrencias da aplicação
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns>success = bool</returns>

        public bool DeleteFolder(string folderName)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteFolder - Entrou para delete de Folder=" + folderName);

            try
            {
                var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");
                var folderPath = Path.Combine(contentPath, folderName);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteFolder path =" + folderPath);

                if (Directory.Exists(folderPath))
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - DeleteFolder - folderPath existe e vai retornar True");

                    Thread.Sleep(1000);
                    Directory.Delete(folderPath, true);

                    _logger.LogInformation(_sessionId + _sessionName + " | Ocorrencia Contoller - DeleteFolder -  - Folder deleted com sucesso = " + folderPath + " Vai sair e retornar True");
                    return true;
                }
                else
                {
                    _logger.LogError(_sessionId + _sessionName + " | Ocorrencia Contoller - DeleteFolder -" + " Folder " + folderPath + " não existe. Vai sair e retornar False");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + _sessionName + " | Ocorrencia Contoller - DeleteFolder - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// copia os documents do folder source para outro folder dest
        /// </summary>
        /// <param name="sourceFolderName"></param>
        /// <param name="destFolderName"></param>
        /// <returns></returns>

        public int CopyDocumentToSlaveOcorrFolder(string sourceFolderName, string destFolderName)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrência Contoller - CopyDocumentToSlaveOcorrFolder - Entrou para mover os files de source=" + sourceFolderName + " para dest=" + destFolderName);
                var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");
                var sourcePath = Path.Combine(contentPath, sourceFolderName);
                var destPath = Path.Combine(contentPath, destFolderName);

                if (Directory.Exists(sourcePath) && Directory.Exists(destPath))
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrência Contoller - CopyDocumentToSlaveOcorrFolder - Ambos os Folders existem");
                    DirectoryInfo diSource = new DirectoryInfo(sourcePath);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrência Contoller - CopyDocumentToSlaveOcorrFolder - Existem " + diSource.GetFiles().Length + " em " + sourcePath);
                    foreach (FileInfo fi in diSource.GetFiles())
                    {
                        fi.CopyTo(Path.Combine(destPath, fi.Name), true);
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrência Contoller - CopyDocumentToSlaveOcorrFolder - File " + fi.Name + " foi copiado para " + destPath);
                    }
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrência Contoller - CopyDocumentToSlaveOcorrFolder - Todos os files foram copiados com sucesso: vai retornar true");
                    return diSource.GetFiles().Length;
                }
                else
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrência Contoller - CopyDocumentToSlaveOcorrFolder - Diretoria não existe. Vai sair e retornar false");
                    return 0;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrência Contoller - CopyDocumentToSlaveOcorrFolder - IO exception vai sair e retornar false: " + ex.Message);
                return 0;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para registar na db os ficheiros uploaded.
        /// a ocorrencia foi criada, o folder dos ficheiros agora é o CodeId.
        /// os ficheiros ficam a apontar para a ocorrencia agora criada.
        /// os ficheiros ficam marcadas como definitivos com TempFolderGuid = null
        /// </summary>
        /// <param name="tempFolderGuid"></param>
        /// <param name="CodeId"></param>
        /// <param name="ocorrenciaId"></param>
        /// <returns>totalDocuments = int</returns>

        public async Task<int> SetFicheirosInOcorrenciaAsync(string tempFolderGuid, string CodeId, int ocorrenciaId)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - SetFicheirosInOcorrenciaAsync - Entrou para registar na db os ficheiros que existiam em " + tempFolderGuid + " e existem agora em " + CodeId + " Ficheiros de OcorrenciaId= " + ocorrenciaId);

            var filesCount = 0;
            var wwwPath = _environment.WebRootPath;
            var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");
            var dirPath = Path.Combine(contentPath, CodeId);

            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - SetFicheirosInOcorrenciaAsync - Ocorrencia dirPath=" + dirPath);

            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - SetFicheirosInOcorrenciaAsync - Vai ler na db todas os ficheiros que existem no tempFolder = " + tempFolderGuid);
                var filesResponse = await _mediator.Send(new GetAllOcorrenciaDocumentsByFolderQuery() { Folder = tempFolderGuid });
                var filesList = _mapper.Map<List<OcorrenciaDocumentViewModel>>(filesResponse.Data);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - SetFicheirosInOcorrenciaAsync - total de ficheiros lidas da db para atualizar=" + filesList.Count);

                // registar os ficheiros na db
                foreach (OcorrenciaDocumentViewModel file in filesList)
                {
                    file.OcorrenciaId = ocorrenciaId > 0 ? ocorrenciaId : null;
                    file.OcorrenciaFolder = CodeId;
                    file.FilePath = dirPath + "/" + file.FileName;

                    var updateFileCommand = _mapper.Map<UpdateOcorrenciaDocumentCommand>(file);
                    var result = await _mediator.Send(updateFileCommand);
                    if (result.Succeeded) filesCount = filesCount + 1;
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - SetFicheirosInOcorrenciaAsync - file atualizado=" + file.Id);
                }

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - SetFicheirosInOcorrenciaAsync - Todos os files foram atualizados. Vai sair e retornar True");
                return filesCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - SetFicheirosInOcorrenciaAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return filesCount;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para atualizar na bd a path dos ficheiros de uma ocorrencia.
        /// foi feito rename do folder da ocorrencia e é necessário registar
        /// na bd a nova localização dos ficheiros.
        /// </summary>
        /// <param name="oldFolder"></param>
        /// <param name="newFolder"></param>
        /// <returns>success = bool</returns>

        public async Task<bool> UpdatePathFicheirosInOcorrenciaAsync(string oldFolder, string newFolder, int ocorrenciaId)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UpdatePathFicheirosInOcorrenciaAsync - Entrou para registar na db os ficheiros que existiam em " + oldFolder + " e existem agora em " + newFolder + " Fotos de ClaimId= " + ocorrenciaId);

            var wwwPath = _environment.WebRootPath;
            var contentPath = Path.Combine(_environment.WebRootPath, "Ocorrências");
            var dirPath = Path.Combine(contentPath, newFolder);

            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UpdatePathFicheirosInOcorrenciaAsync - Ocorrencia dirPath=" + dirPath);

            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UpdatePathFicheirosInOcorrenciaAsync - Vai ler na db todas os files da OcorrenciaId = " + ocorrenciaId);
                var filesResponse = await _mediator.Send(new GetAllOcorrenciaDocumentsByOcorrenciaIdQuery() { OcorrenciaId = ocorrenciaId });
                var filesList = _mapper.Map<List<OcorrenciaDocumentViewModel>>(filesResponse.Data);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UpdatePathFicheirosInOcorrenciaAsync - total de files lidos da db para atualizar=" + filesList.Count);

                // registar os files na db
                foreach (OcorrenciaDocumentViewModel file in filesList)
                {
                    file.FilePath = dirPath + "/" + file.FileName;
                    var updateFileCommand = _mapper.Map<UpdateOcorrenciaDocumentCommand>(file);
                    var result = await _mediator.Send(updateFileCommand);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UpdatePathFicheirosInOcorrenciaAsync - file atualizado=" + file.Id);
                }

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UpdatePathFicheirosInOcorrenciaAsync - Todas os files foram atualizados. Vai sair e retornar True");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - UpdatePathFicheirosInOcorrenciaAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara uma estrutura OcorrenciaViewModel para ser enviada
        /// ao client. Tem em conta o Role do user corrente.
        /// evocada no atendimento GET OnGetCreateOrEdit (criar ocorrencia).
        /// </summary>
        /// <param name="ocorrenciaFolder"></param>
        /// <returns> type="OcorrenciaViewModel"</returns>

        internal async Task<OcorrenciaViewModel> InitNewOcorrenciaAsync(string ocorrenciaFolder)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - InitNewOcorrenciaAsync - Entrou para criar estrutura OcorrenciaViewModel com o Folder=" + ocorrenciaFolder);

                //criar modelView para retornar
                var ocorrenciaViewModel = new OcorrenciaViewModel();

                // Current Role
                ocorrenciaViewModel.CurrentRole = await GetCurrentRoleAsync(null);

                ocorrenciaViewModel.Logo = await EmpresaController.GetEmpresaLogoAsync(ocorrenciaViewModel.CurrentRole.EmpresaId, _mapper, _mediator);
                ocorrenciaViewModel.CodeId = "< Não Defenido >";
                ocorrenciaViewModel.DataEntradaSistemaOcorrencia = DateTime.Now.Date;
                ocorrenciaViewModel.EmailAutor = ocorrenciaViewModel.CurrentRole.Email;
                ocorrenciaViewModel.OcorrenciaFolder = ocorrenciaFolder;
                ocorrenciaViewModel.EditMode = false;
                ocorrenciaViewModel.DataOcorrencia = DateTime.Now.Date;

                ocorrenciaViewModel.LojaId = ocorrenciaViewModel.CurrentRole.LojaId;
                ocorrenciaViewModel.GrupolojaId = ocorrenciaViewModel.CurrentRole.GrupolojaId;
                ocorrenciaViewModel.EmpresaId = ocorrenciaViewModel.CurrentRole.EmpresaId;
                ocorrenciaViewModel.MercadoId = ocorrenciaViewModel.CurrentRole.MercadoId;

                ocorrenciaViewModel.ToLojaId = 0;
                ocorrenciaViewModel.ToGrupolojaId = 0;
                ocorrenciaViewModel.ToEmpresaId = 0;
                ocorrenciaViewModel.ToMercadoId = 0;

                ocorrenciaViewModel.CategoriaId = 0;
                ocorrenciaViewModel.TipoOcorrenciaId = 0;
                ocorrenciaViewModel.StatusId = 0;
                ocorrenciaViewModel.Logo = await EmpresaController.GetEmpresaLogoAsync(ocorrenciaViewModel.EmpresaId, _mapper, _mediator);
                ocorrenciaViewModel.Mercados = await InitMercadosByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.MercadoId, false);
                ocorrenciaViewModel.Empresas = await InitEmpresasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.EmpresaId, false);
                ocorrenciaViewModel.GruposLojas = await InitGrupoLojasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.EmpresaId, ocorrenciaViewModel.GrupolojaId, false);
                ocorrenciaViewModel.Lojas = await InitLojasByRoleAsync(ocorrenciaViewModel.CurrentRole, ocorrenciaViewModel.GrupolojaId, ocorrenciaViewModel.LojaId, false);

                // Retrieves the requested culture
                //var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
                // Culture contains the information of the requested culture
                var culture = _culture.RequestCulture.Culture;

                // get all Categorias
                ocorrenciaViewModel.Categorias = OcorrenciaCategoriaList.GetSelectListCategoria(culture.Name, 0);

                // get Tipos
                ocorrenciaViewModel.Tipos = await TipoOcorrenciaController.GetSelectListFromCategoriaAsync(ocorrenciaViewModel.CategoriaId, 0, _mapper, _mediator, _culture);

                // get Status
                ocorrenciaViewModel.Status = OcorrenciaStatusList.GetNextStatusOptions(1, 0, culture.Name);

                return ocorrenciaViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - CreateFolder - IO exception vai sair e retornar Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para criar uma SelectList de Mercados de acordo com o Role
        /// do user corrente e da ocorrencia estar a ser criada ou editada
        /// </summary>
        /// <param name="cRole"></param>
        /// <param name="selectedId"></param>
        /// <param name="editar"></param>
        /// <returns> type="SelectList"</returns>

        public async Task<SelectList> InitMercadosByRoleAsync(CurrentRole cRole, int selectedId, bool editar)
        {
            if (editar) return await MercadoController.GetSelectListOneMercadoAsync(selectedId, _mapper, _mediator);
            if (cRole.IsSuperAdmin || cRole.IsAdmin || cRole.IsRevisor)
            {
                //construir SelectList para qualquer Mercado
                return await MercadoController.GetSelectListAllMercadosAsync(selectedId, _mapper, _mediator);
            }
            return await MercadoController.GetSelectListOneMercadoAsync(selectedId, _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para criar uma SelectList de Empresas de acordo com o Role
        /// do user corrente e da ocorrencia estar a ser criada ou editada
        /// </summary>
        /// <param name="cRole"></param>
        /// <param name="selectedId"></param>
        /// <param name="editar"></param>
        /// <returns> type="SelectList"</returns>

        public async Task<SelectList> InitEmpresasByRoleAsync(CurrentRole cRole, int selectedId, bool editar)
        {
            if (editar) return await EmpresaController.GetSelectListOneEmpresaAsync(selectedId, _mapper, _mediator);
            if (cRole.IsSuperAdmin || cRole.IsAdmin || cRole.IsRevisor)
            {
                //construir SelectList para qualquer Empresa
                return await EmpresaController.GetSelectListAllEmpresasAsync(selectedId, _mapper, _mediator);
            }
            return await EmpresaController.GetSelectListOneEmpresaAsync(selectedId, _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para criar uma SelectList de GrupoLojas de acordo com o Role
        /// do user corrente e em função da Empresa selecionada
        /// </summary>
        /// <param name="cRole"></param>
        /// <param name="empresaId"></param>
        /// <param name="selectedId"></param>
        /// <param name="editar"></param>
        /// <returns> type="SelectList"</returns>

        public async Task<SelectList> InitGrupoLojasByRoleAsync(CurrentRole cRole, int empresaId, int selectedId, bool editar)
        {
            if (editar) return await GrupolojaController.GetSelectListOneGrupoLojaAsync(selectedId, _mapper, _mediator);
            if (cRole.IsSuperAdmin || cRole.IsAdmin || cRole.IsRevisor)
            {
                //construir SelectList para qualquer GrupoLoja de empresaId
                return await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(empresaId, selectedId, _mapper, _mediator);
            }
            return await GrupolojaController.GetSelectListOneGrupoLojaAsync(selectedId, _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para criar uma SelectList de Lojas de acordo com o Role
        /// do user corrente e em função do GrupoLoja selecionado
        /// </summary>
        /// <param name="cRole"></param>
        /// <param name="grupolojaId"></param>
        /// <param name="selectedId"></param>
        /// <param name="editar"></param>
        /// <returns> type="SelectList"</returns>

        public async Task<SelectList> InitLojasByRoleAsync(CurrentRole cRole, int grupolojaId, int selectedId, bool editar)
        {
            if (editar) return await LojaController.GetSelectListOneLojaAsync(selectedId, _mapper, _mediator);
            if (cRole.IsSuperAdmin || cRole.IsAdmin || cRole.IsRevisor || cRole.IsSupervisor)
            {
                //construir SelectList para qualquer Loja de grupolojaId
                return await LojaController.GetSelectListLojasFromGrupolojaAsync(grupolojaId, selectedId, _mapper, _mediator);
            }
            //cRole.IsGerenteLoja || cRole.IsColaborador || cRole.IsBasic
            //construir SelectList para Loja em selectedId
            return await LojaController.GetSelectListOneLojaAsync(selectedId, _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria uma select list com todas as categorias de uma ocorrência
        /// </summary>
        /// <param name="selectedId"></param>
        /// <param name="culture"></param>
        /// <returns> type="SelectList"</returns>

        public static SelectList GetSelectListAllCategorias(int selectedId, IRequestCultureFeature culture)
        {
            // Culture contains the information of the requested culture
            var lang = culture.RequestCulture.Culture;

            var categoriasViewModel = OcorrenciaCategoriaList.List[lang.Name];
            return new SelectList(categoriasViewModel, nameof(OcorrenciaCategoria.Id), nameof(OcorrenciaCategoria.Categoria), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria uma select list com todos os status de uma ocorrência
        /// </summary>
        /// <param name="selectedId"></param>
        /// <param name="culture"></param>
        /// <returns> type="SelectList"</returns>

        public static SelectList GetSelectListAllStatus(int selectedId, IRequestCultureFeature culture)
        {
            // Culture contains the information of the requested culture
            var lang = culture.RequestCulture.Culture;

            var statusViewModel = OcorrenciaStatusList.List[lang.Name];
            return new SelectList(statusViewModel, nameof(OcorrenciaStatus.Id), nameof(Core.Entities.Ocorrencias.OcorrenciaStatus.Status), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria uma select list com os next status possiveis,
        /// a partir de um status corrente
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <param name="currentStatusId"></param>
        /// <param name="culture"></param>
        /// <returns> type="SelectList"</returns>

        public static SelectList GetSelectListNextStatus(int categoriaId, int currentStatusId, IRequestCultureFeature culture)
        {
            // Culture contains the information of the requested culture
            var lang = culture.RequestCulture.Culture;

            return OcorrenciaStatusList.GetNextStatusOptions(categoriaId, currentStatusId, lang.Name);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria a lista com os status de uma categoria de ocorrências
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <returns>type="JsonResult"</returns>

        public JsonResult LoadStatusInCategoria(int categoriaId)
        {
            // Culture contains the information of the requested culture
            var lang = _culture.RequestCulture.Culture;

            if (categoriaId > 0)
            {
                var statusSelectList = OcorrenciaStatusList.GetNextStatusOptions(categoriaId, 0, lang.Name);
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { statusList = statusSelectList });
                return Json(jsonString);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria um CurrentRole para o utilizador corrente "User"
        /// </summary>
        /// <param></param>
        /// <returns>type="CurrentRole"</returns>

        internal async Task<CurrentRole> GetCurrentRoleAsync(string userId)
        {

            // CurrentRole
            var cr = new CurrentRole();
            var appUserId = string.Empty;

            try
            {
                appUserId = string.IsNullOrEmpty(userId) ? _signInManager.UserManager.GetUserId(User) : userId;
                var currentUser = await _signInManager.UserManager.FindByIdAsync(appUserId);

                cr.Email = currentUser.Email;
                cr.LojaId = currentUser.LojaId == null ? 0 : (int)currentUser.LojaId;
                cr.GrupolojaId = currentUser.GrupolojaId == null ? 0 : (int)currentUser.GrupolojaId;
                cr.EmpresaId = currentUser.EmpresaId == null ? 0 : (int)currentUser.EmpresaId;
                cr.MercadoId = await MercadoController.GetMercadoIdAsync(cr.LojaId, _mapper, _mediator);

                cr.IsSuperAdmin = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.SuperAdmin.ToString());
                cr.IsAdmin = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Admin.ToString());
                cr.IsSupervisor = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Supervisor.ToString());
                cr.IsRevisor = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Revisor.ToString());
                cr.IsGerenteLoja = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.GerenteLoja.ToString());
                cr.IsColaborador = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Colaborador.ToString());
                cr.IsBasic = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Basic.ToString());

                cr.RoleName = string.Empty;
                if (cr.IsSuperAdmin) cr.RoleName = Roles.SuperAdmin.ToString();
                if (cr.IsAdmin) cr.RoleName = Roles.Admin.ToString();
                if (cr.IsSupervisor) cr.RoleName = Roles.Supervisor.ToString();
                if (cr.IsRevisor) cr.RoleName = Roles.Revisor.ToString();
                if (cr.IsGerenteLoja) cr.RoleName = Roles.GerenteLoja.ToString();
                if (cr.IsColaborador) cr.RoleName = Roles.Colaborador.ToString();
                if (cr.IsBasic) cr.RoleName = Roles.Basic.ToString();

                return cr;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - GetCurrentRoleAsync - User exception vai sair e retornar Error: " + ex.Message);
                return cr;
            };
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// converte uma OcorrenciaCachedResponse em OcorrenciaViewModel
        /// a OcorrenciaViewModel enviada ao client integra estruturas de dados.
        /// Esta função converte a ocorrencia lida da db nas estruturas
        /// da OcorrenciaViewModel evocada no atendimento GET (edit ocorrencia).
        /// </summary>
        /// <param name="ocorr"></param>
        /// <returns>type="OcorrenciaViewModel"</returns>

        internal async Task<OcorrenciaViewModel> MapperEntitieToModelOcorrenciaAsync(OcorrenciaCachedResponse ocorr)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - MapperEntitieToModelOcorrenciaAsync - Entrou para criar estrutura OcorrenciaViewModel para a ocorr=" + ocorr.Id);

                // Culture contains the information of the requested culture
                var culture = _culture.RequestCulture.Culture;

                //criar modelView para retornar
                var model = new OcorrenciaViewModel();

                // Current Role
                model.CurrentRole = await GetCurrentRoleAsync(null);

                // Cabeçalho da Ocorrência
                model.Id = ocorr.Id;
                model.CodeId = ocorr.CodeId;     // Format : YYYYMMDD-OCORR-LLLL-XXXXX
                model.DataEntradaSistemaOcorrencia = ocorr.DataEntradaSistemaOcorrencia;
                model.EmailAutor = ocorr.EmailAutor;
                model.OcorrenciaFolder = ocorr.CodeId;
                model.Logo = await EmpresaController.GetEmpresaLogoAsync(ocorr.EmpresaId, _mapper, _mediator);
                model.DataOcorrencia = ocorr.DataOcorrencia;


                model.CategoriaId = ocorr.CategoriaId;
                model.CategoriaNome = OcorrenciaCategoriaList.GetCategoriaName(culture.Name, ocorr.CategoriaId);
                model.InterOcorrenciaId = ocorr.InterOcorrenciaId;          // interlojas
                model.MasterOcorrencia = ocorr.MasterOcorrencia;            // interlojas
                model.TipoOcorrenciaId = ocorr.TipoOcorrenciaId;
                model.StatusId = ocorr.StatusId;
                model.StatusNome = OcorrenciaStatusList.GetStatusName(culture.Name, ocorr.StatusId);


                model.OcorrenciaNome = ocorr.OcorrenciaNome;                // nome da ocorrencia
                model.Descrição = ocorr.Descrição;
                model.Comentário = ocorr.Comentário;
                

                model.MercadoId = ocorr.MercadoId;
                model.EmpresaId = ocorr.EmpresaId;
                model.GrupolojaId = ocorr.GrupolojaId;
                model.LojaId = ocorr.LojaId;


                model.EditMode = true;
                model.LojaNome = await LojaController.GetLojaNomeAsync(ocorr.LojaId, _mapper, _mediator);
                model.Empresas = await InitEmpresasByRoleAsync(model.CurrentRole, model.EmpresaId, true);
                model.GruposLojas = await InitGrupoLojasByRoleAsync(model.CurrentRole, model.EmpresaId, model.GrupolojaId, true);
                model.Lojas = await InitLojasByRoleAsync(model.CurrentRole, model.GrupolojaId, model.LojaId, true);
                model.Mercados = await MercadoController.GetSelectListAllMercadosAsync(model.MercadoId, _mapper, _mediator);
                model.TotalFicheiros = ocorr.TotalFicheiros;


                // get all Categorias
                model.Categorias = OcorrenciaCategoriaList.GetSelectListCategoria(culture.Name, 0);

                // get Tipos
                model.Tipos = await TipoOcorrenciaController.GetSelectListFromCategoriaAsync(model.CategoriaId, model.TipoOcorrenciaId, _mapper, _mediator, _culture);

                // get Status
                model.Status = OcorrenciaStatusList.GetNextStatusOptions(1, 0, culture.Name);

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - MapperEntitieToModelOcorrenciaAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// converte uma OcorrenciaViewModel em Ocorrencia
        /// a OcorrenciaViewModel posted pelo client integra estruturas de dados.
        /// Esta função retira todos os dados
        /// dessas estruturas e cria a entidade necessária para escrever na db.
        /// regista o novo status e o prvious status da ocorrencia.
        /// evocada no atendimento POST OnPostCreateOrEdit.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>type="Ocorrencia"</returns>

        internal async Task<Core.Entities.Ocorrencias.Ocorrencia> MapperViewModelToEntitieOcorrenciaAsync(OcorrenciaViewModel model)
        {
            var ocorr = new Core.Entities.Ocorrencias.Ocorrencia();
            ocorr.Id = model.Id;

            ocorr.CodeId = model.CodeId;                            // Format : YYYYMMDD-OCORR-LLLL-XXXXXX
            ocorr.CategoriaId = model.CategoriaId;
            ocorr.TipoOcorrenciaId = model.TipoOcorrenciaId;
            ocorr.StatusId = model.StatusId;
            ocorr.InterOcorrenciaId = 0;                            // Interlojas ID da ocorrencia master
            ocorr.MasterOcorrencia = true;                          // não é uma ocorrencia slave

            ocorr.OcorrenciaFolder = model.OcorrenciaFolder;        // folder onde estão os ficheiros
            ocorr.OcorrenciaNome = model.OcorrenciaNome;            // nome da ocorrencia
            ocorr.DataEntradaSistemaOcorrencia = model.DataEntradaSistemaOcorrencia;
            ocorr.DataOcorrencia = model.DataOcorrencia;
            ocorr.EmailAutor = model.EmailAutor;
            ocorr.Descrição = model.Descrição;
            ocorr.Comentário = model.Comentário;


            ocorr.MercadoId = model.MercadoId;
            ocorr.EmpresaId = model.EmpresaId;
            ocorr.GrupolojaId = model.GrupolojaId;
            ocorr.LojaId = model.LojaId;

            if (ocorr.MercadoId == 0)
            {
                var lj = await LojaController.GetLojaAsync(ocorr.LojaId, _mapper, _mediator);
                ocorr.MercadoId = lj.MercadoId;
            }


            //Ficheitos
            ocorr.TotalFicheiros = model.TotalFicheiros;

            return ocorr;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// converte uma OcorrenciaViewModel em Ocorrencia Interlojas SLAVE
        /// a OcorrenciaViewModel posted pelo client integra estruturas de dados.
        /// Esta função retira todos os dados
        /// dessas estruturas e cria a entidade necessária para escrever na db.
        /// regista o novo status e o prvious status da ocorrencia.
        /// evocada no atendimento POST OnPostCreateOrEdit.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>type="Ocorrencia"</returns>

        internal async Task<Core.Entities.Ocorrencias.Ocorrencia> GetOcorrenciaSlaveAsync(OcorrenciaViewModel ocorrMaster)
        {

            var ocorrSlave = new Core.Entities.Ocorrencias.Ocorrencia();
            ocorrSlave.Id = 0;

            ocorrSlave.CodeId = "< Não Defenido >";                         // Format : YYYYMMDD-OCORR-LLLL-XXXXXX
            ocorrSlave.CategoriaId = ocorrMaster.CategoriaId;
            ocorrSlave.TipoOcorrenciaId = ocorrMaster.TipoOcorrenciaId;
            ocorrSlave.StatusId = 2;                                        //{ Id = 2, Status = "PEDIDO POR ATENDER" }
            ocorrSlave.InterOcorrenciaId = 0;                               // Interlojas ID da ocorrencia master
            ocorrSlave.MasterOcorrencia = false;                            // é uma ocorrencia slave, não é master

            ocorrSlave.OcorrenciaFolder = ocorrMaster.OcorrenciaFolder;     // folder onde estão os ficheiros
            ocorrSlave.OcorrenciaNome = ocorrMaster.OcorrenciaNome;         // nome da ocorrencia
            ocorrSlave.DataEntradaSistemaOcorrencia = ocorrMaster.DataEntradaSistemaOcorrencia;
            ocorrSlave.DataOcorrencia = ocorrMaster.DataOcorrencia;
            ocorrSlave.EmailAutor = ocorrMaster.EmailAutor;
            ocorrSlave.Descrição = ocorrMaster.Descrição;
            ocorrSlave.Comentário = ocorrMaster.Comentário;


            ocorrSlave.MercadoId = ocorrMaster.ToMercadoId;
            ocorrSlave.EmpresaId = ocorrMaster.ToEmpresaId;
            ocorrSlave.GrupolojaId = ocorrMaster.ToGrupolojaId;
            ocorrSlave.LojaId = ocorrMaster.ToLojaId;

            if (ocorrSlave.MercadoId == 0)
            {
                var lj = await LojaController.GetLojaAsync(ocorrSlave.LojaId, _mapper, _mediator);
                ocorrSlave.MercadoId = lj.MercadoId;
            }


            //Ficheitos
            ocorrSlave.TotalFicheiros = ocorrMaster.TotalFicheiros;

            return ocorrSlave;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para gerar o CodeId identificador da ocorrencia
        /// o código é gerado a partir da data, loja e Id da ocorrencia.
        /// o Id obriga a que a ocorrencia seja primeiro criada na db e
        /// só depois o código pode ser gerado.
        /// Depois do CodeId ser gerado é necessário update da ocorrencia na db
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="date"></param>
        /// <param name="LojaId"></param>
        /// <returns>type="string"</returns>

        internal async Task<string> CodeGenerationAsync(int Id, DateTime date, int LojaId)
        {
            // Format : YYYYMMDD-OCORR-LLLL-XXXXXX
            var code = date.Date.ToString("yyyyMMdd");

            var lojaResponse = await _mediator.Send(new GetLojaByIdQuery() { Id = LojaId });
            if (lojaResponse.Succeeded)
            {
                code = code + "-OCORR-" + lojaResponse.Data.NomeCurto;
            }
            code = code + "-" + Id.ToString("000000");
            return code;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para gerar o corpo de uma notificação
        /// para notificar a criação de uma nova ocorrencia.
        /// </summary>
        /// <param ocorrência="ocorr"></param>
        /// <returns>type="string"</returns>

        internal async Task<string> GetNotificationBodyAsync(Core.Entities.Ocorrencias.Ocorrencia ocorr)
        {
            var lang = _culture.RequestCulture.Culture.Name;

            string[] body = {
                $"{_localizer["Código"]} : {ocorr.CodeId}",
                $"{_localizer["Data da Ocorrência"]} : {ocorr.DataOcorrencia.ToShortDateString()}",
                $"{_localizer["Categoria"]} : {OcorrenciaCategoriaList.GetCategoriaName(lang, ocorr.CategoriaId)}",
                $"{_localizer["Tipo"]} : {await TipoOcorrenciaController.GetTipoOcorrenciaNomeAsync(ocorr.TipoOcorrenciaId, lang, _mediator, _mapper)}",
                $"{_localizer["Status"]} : {OcorrenciaStatusList.GetStatusName(lang, ocorr.StatusId)}",
                "\n",
                $"{_localizer["Ocorrência"]} : {ocorr.OcorrenciaNome}",
                "\n",
                $"{_localizer["Descrição"]} : {ocorr.Descrição}",
                $"{_localizer["Comentário"]} : {ocorr.Comentário}"
            };

            return string.Join("\n", body);
        }


        //---------------------------------------------------------------------------------------------------

    }
}