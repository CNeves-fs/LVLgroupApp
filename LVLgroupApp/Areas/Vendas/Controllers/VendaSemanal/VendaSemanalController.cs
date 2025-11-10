using AutoMapper;
using ClosedXML.Excel;
using Core.Constants;
using Core.Entities.Business;
using Core.Entities.Identity;
using Core.Entities.Vendas;
using Core.Enums;
using Core.Extensions;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Gruposlojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetById;
using Core.Features.Mercados.Queries.GetAllCached;
using Core.Features.VendasDiarias.Commands.Create;
using Core.Features.VendasDiarias.Commands.Update;
using Core.Features.VendasDiarias.Queries.GetAllCached;
using Core.Features.VendasDiarias.Queries.GetByEmpresaId;
using Core.Features.VendasDiarias.Queries.GetByGrupolojaId;
using Core.Features.VendasDiarias.Queries.GetByLojaId;
using Core.Features.VendasDiarias.Queries.GetByMercadoId;
using Core.Features.VendasDiarias.Queries.GetByVendaSemanalId;
using Core.Features.VendasSemanais.Commands.Create;
using Core.Features.VendasSemanais.Commands.Delete;
using Core.Features.VendasSemanais.Commands.Update;
using Core.Features.VendasSemanais.Queries.GetAllCached;
using Core.Features.VendasSemanais.Queries.GetByEmpresaId;
using Core.Features.VendasSemanais.Queries.GetByGrupolojaId;
using Core.Features.VendasSemanais.Queries.GetById;
using Core.Features.VendasSemanais.Queries.GetByLojaId;
using Core.Features.VendasSemanais.Queries.GetByMercadoId;
using Infrastructure.Extensions;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Business.Controllers.Mercado;
using LVLgroupApp.Areas.Vendas.Models.VendaDiaria;
using LVLgroupApp.Areas.Vendas.Models.VendaMensal;
using LVLgroupApp.Areas.Vendas.Models.VendaSemanal;
using LVLgroupApp.Hubs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static Core.Constants.Permissions;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace LVLgroupApp.Areas.Vendas.Controllers
{
    [Area("Vendas")]
    [Authorize]
    public class VendaSemanalController : BaseController<VendaSemanalController>
    {

        //---------------------------------------------------------------------------------------------------


        private const int SEMANA_COLUMN = 1;    // Excel coluna A
        private const int DATA_COLUMN = 2;      // Excel coluna B
        private const int LOJA_COLUMN = 3;      // Excel coluna C
        private const int QNTPECAS_COLUMN = 4;  // Excel coluna D
        private const int CONV_COLUMN = 5;      // Excel coluna E
        private const int VALOR_COLUMN = 6;     // Excel coluna F
        private const int OBJETIVO_COLUMN = 7;  // Excel coluna G
        private const int QUARTER_COLUMN = 8;   // Excel coluna H


        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<VendaSemanalController> _localizer;

        private readonly IHubContext<ProgressHub> _hubContext;

        private IWebHostEnvironment _environment;

        private readonly SignInManager<ApplicationUser> _signInManager;


        //---------------------------------------------------------------------------------------------------


        public VendaSemanalController(IWebHostEnvironment environment, IStringLocalizer<VendaSemanalController> localizer, SignInManager<ApplicationUser> signInManager, IHubContext<ProgressHub> hubContext)
        {
            _localizer = localizer;
            _signInManager = signInManager;
            _environment = environment;
            _hubContext = hubContext;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Vendas.View)]
        public async Task<IActionResult> IndexAsync()
        {
            var model = new VendaSemanalComparaViewModel();
            model.CurrentRole = await GetCurrentRoleAsync();
            model.AnoAtual = new VendaSemanalViewModel();
            model.AnoAtual.Mercados = await MercadoController.GetSelectListAllMercadosAsync(model.CurrentRole.MercadoId, _mapper, _mediator);
            model.AnoAtual.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(model.CurrentRole.EmpresaId, _mapper, _mediator);
            model.AnoAtual.GruposLojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(model.CurrentRole.EmpresaId, model.CurrentRole.GrupolojaId, _mapper, _mediator);
            model.AnoAtual.Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(model.CurrentRole.GrupolojaId, model.CurrentRole.LojaId, _mapper, _mediator);
            model.AnoAtual.Years = new List<string>() { "2021", "2022", "2023", "2024", "2025", "2026" };
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Ocorrencia Contoller - Index - return viewModel");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Vendas.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - LoadAll - return lista vazia de VendaSemanalViewModel");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.View)]
        public async Task<IActionResult> OnGetBoxVendaSemanalAsync(int mercadoId, int empresaId, int grupolojaId, int lojaId, int numeroSemana, int ano)
        {
            var model = new BoxSemanalViewModel()
            {
                MercadoId = mercadoId,
                EmpresaId = empresaId,
                GrupolojaId = grupolojaId,
                LojaId = lojaId,
                NumeroDaSemana = numeroSemana,
                Ano = ano,
                ValorTotalDaVenda = await GetValorTotalDaVenda(mercadoId, empresaId, grupolojaId, lojaId, numeroSemana, ano, _mediator, _mapper),
                ObjetivoDaVendaSemanal = await GetObjetivoDaVendaSemanal(mercadoId, empresaId, grupolojaId, lojaId, numeroSemana, ano, _mediator, _mapper)
            };
            return ViewComponent("BoxVendaSemanal", new { model });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.View)]
        public async Task<IActionResult> OnGetBoxVendaSemanalFromDiaAsync(int mercadoId, int empresaId, int grupolojaId, int lojaId, int dia, int mes, int ano)
        {
            var numeroSemana = GetNumeroDaSemana(ano, mes, dia);

            if (numeroSemana == 0)
            {
                return ViewComponent("BoxVendaSemanal", new BoxSemanalViewModel());
            }
            var model = new BoxSemanalViewModel()
            {
                MercadoId = mercadoId,
                EmpresaId = empresaId,
                GrupolojaId = grupolojaId,
                LojaId = lojaId,
                NumeroDaSemana = numeroSemana,
                Ano = ano,
                ValorTotalDaVenda = await GetValorTotalDaVenda(mercadoId, empresaId, grupolojaId, lojaId, numeroSemana, ano, _mediator, _mapper),
                ObjetivoDaVendaSemanal = await GetObjetivoDaVendaSemanal(mercadoId, empresaId, grupolojaId, lojaId, numeroSemana, ano, _mediator, _mapper)
            };
            return ViewComponent("BoxVendaSemanal", new { model });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.View)]
        public async Task<IActionResult> OnGetBoxVendaMensalAsync(int mercadoId, int empresaId, int grupolojaId, int lojaId, int mes, int ano)
        {
            var mesLiteral = new CultureInfo(_localizer["pt-PT"]).DateTimeFormat.GetMonthName(mes);
            mesLiteral = mesLiteral.FirstCharToUpper();

            var model = new BoxMensalViewModel()
            {
                MercadoId = mercadoId,
                EmpresaId = empresaId,
                GrupolojaId = grupolojaId,
                LojaId = lojaId,
                Mes = mes,
                MesLiteral = mesLiteral,
                Ano = ano,
                ValorTotalMensalDaVenda = await GetValorTotalMensalDaVenda(mercadoId, empresaId, grupolojaId, lojaId, mes, ano, _mediator, _mapper),
                ObjetivoMensalDaVenda = await GetObjetivoMensalDaVenda(mercadoId, empresaId, grupolojaId, lojaId, mes, ano, _mediator, _mapper)
            };
            return ViewComponent("BoxVendaMensal", new { model });
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de VendaSemanal para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.View)]
        public async Task<IActionResult> GetVendasSemanais()
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

                DateTime dateDesde = string.IsNullOrEmpty(desdedateFilter) ? DateTime.MinValue : DateTime.Parse(desdedateFilter);
                DateTime dateAte = string.IsNullOrEmpty(atedateFilter) ? DateTime.MaxValue : DateTime.Parse(atedateFilter);

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

                // lista de vendas permitidas ao current user
                var allVendas = await GetVendaSemanalListAsync();

                // filtrar por mercado se necessário
                if (intFilterMercado > 0)
                {
                    allVendas = allVendas.Where(v => v.MercadoId == intFilterMercado);
                };
                // filtrar por empresa se necessário
                if (intFilterEmpresa > 0)
                {
                    allVendas = allVendas.Where(v => v.EmpresaId == intFilterEmpresa);
                };
                // filtrar por grupoloja se necessário
                if (intFilterGrupoloja > 0)
                {
                    allVendas = allVendas.Where(v => v.GrupolojaId == intFilterGrupoloja);
                };
                // filtrar por loja se necessário
                if (intFilterLoja > 0)
                {
                    allVendas = allVendas.Where(v => v.LojaId == intFilterLoja);
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
                            allVendas = allVendas.Where(v => v.Ano == intFilterAno1 || v.Ano == intFilterAno2 || v.Ano == intFilterAno3 || v.Ano == intFilterAno4 || v.Ano == intFilterAno5 || v.Ano == intFilterAno6);
                        };
                        
                        // filtrar por quarter se necessário
                        if (boolFilterQuarter1 || boolFilterQuarter2 || boolFilterQuarter3 || boolFilterQuarter4)
                        {
                            allVendas = allVendas.Where(v => (boolFilterQuarter1 && v.Quarter == 1) || (boolFilterQuarter2 && v.Quarter == 2) || (boolFilterQuarter3 && v.Quarter == 3) || (boolFilterQuarter4 && v.Quarter == 4));
                        };
                        
                        // filtrar por month se necessário
                        if (boolFilterJaneiro || boolFilterFevereiro || boolFilterMarço ||
                            boolFilterAbril || boolFilterMaio || boolFilterJunho ||
                            boolFilterJulho || boolFilterAgosto || boolFilterSetembro ||
                            boolFilterOutubro || boolFilterNovembro || boolFilterDezembro)
                        {
                            allVendas = allVendas.Where(v => (boolFilterJaneiro && v.Mes == 1) || (boolFilterFevereiro && v.Mes == 2) || (boolFilterMarço && v.Mes == 3) ||
                                                             (boolFilterAbril && v.Mes == 4) || (boolFilterMaio && v.Mes == 5) || (boolFilterJunho && v.Mes == 6) ||
                                                             (boolFilterJulho && v.Mes == 7) || (boolFilterAgosto && v.Mes == 8) || (boolFilterSetembro && v.Mes == 9) ||
                                                             (boolFilterOutubro && v.Mes == 10) || (boolFilterNovembro && v.Mes == 11) || (boolFilterDezembro && v.Mes == 12));
                        };
                        
                        // filtrar por Semana se necessário
                        if (intFilterDesdeSemana > 1)
                        {
                            allVendas = allVendas.Where(v => v.NumeroDaSemana >= intFilterDesdeSemana);
                        };
                        
                        if (intFilterAteSemana < 53)
                        {
                            allVendas = allVendas.Where(v => v.NumeroDaSemana <= intFilterAteSemana);
                        };
                        
                        // filtrar por Data se necessário
                        allVendas = allVendas.Where(v => v.DataFinalDaSemana >= dateDesde);
                        allVendas = allVendas.Where(v => v.DataInicialDaSemana <= dateAte);
                        break;
                    case 1:     // "Esta semana"
                        allVendas = allVendas.Where(v => (v.Ano == esteAno) && (v.NumeroDaSemana == estaSemana));
                        break;
                    case 2:     // "Semana anterior"
                        if (semanaAnterior == 0)
                        {
                            // semana anterior é a do ano passado
                            semanaAnterior = cal.GetWeekOfYear(semanaPassadaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                        };
                        allVendas = allVendas.Where(v => (v.Ano == esteAno) && (v.NumeroDaSemana == semanaAnterior));                    
                        break;
                    case 3:     // "Este mês"
                        allVendas = allVendas.Where(v => (v.Ano == esteAno) && (v.DataInicialDaSemana.Month == esteMes || v.DataFinalDaSemana.Month == esteMes));
                        break;
                    case 4:     // "Mês anterior"
                        if (mesAnterior == 0)
                        {
                            allVendas = allVendas.Where(v => (v.Ano == anoAnterior) && (v.DataInicialDaSemana.Month == 12 || v.DataFinalDaSemana.Month == 12));
                        }
                        else
                        {
                            allVendas = allVendas.Where(v => (v.Ano == esteAno) && (v.DataInicialDaSemana.Month == mesAnterior || v.DataFinalDaSemana.Month == mesAnterior));
                        };
                        break;
                    case 5:     // "Este trimestre"
                        allVendas = allVendas.Where(v => (v.Ano == esteAno) && (v.Quarter == esteTrimestre));
                        break;
                    case 6:     // "Trimestre anterior"
                        if (trimestreAntrior == 0)
                        {
                            allVendas = allVendas.Where(v => (v.Ano == anoAnterior) && (v.Quarter == 4));
                        }
                        else
                        {
                            allVendas = allVendas.Where(v => (v.Ano == esteAno) && (v.Quarter == trimestreAntrior));
                        }
                        ;
                        break;
                    case 7:     // "Este ano"
                        allVendas = allVendas.Where(v => v.Ano == esteAno);
                        break;
                    case 8:     // "Ano anterior"
                        allVendas = allVendas.Where(v => v.Ano == anoAnterior);
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


                var vendaSemanalData = from venda in allVendas
                                    join m in allMercados on venda.MercadoId equals m.Id into mlist
                                    from merc in mlist.DefaultIfEmpty()
                                    join e in allEmpresas on venda.EmpresaId equals e.Id into elist
                                    from emp in elist.DefaultIfEmpty()
                                    join g in allGruposlojas on venda.GrupolojaId equals g.Id into glist
                                    from grp in glist.DefaultIfEmpty()
                                    join l in allLojas on venda.LojaId equals l.Id into llist
                                    from loj in llist.DefaultIfEmpty()
                                    select new VendaSemanalListViewModel()
                                    {
                                        Id = venda.Id,
                                        DataInicialDaSemana = venda.DataInicialDaSemana,
                                        DataFinalDaSemana = venda.DataFinalDaSemana,
                                        ValorTotalDaVenda = venda.ValorTotalDaVenda,
                                        ObjetivoDaVendaSemanal = venda.ObjetivoDaVendaSemanal,
                                        VariaçaoAnual = venda.VariaçaoAnual,
                                        Ano = venda.Ano,
                                        Quarter = venda.Quarter,
                                        Mes = venda.Mes,
                                        NumeroDaSemana = venda.NumeroDaSemana,
                                        MercadoId = venda.MercadoId,
                                        MercadoNome = merc.Nome,
                                        EmpresaId = venda.EmpresaId,
                                        EmpresaLogo = Convert.ToBase64String(emp.LogoPicture),
                                        EmpresaNome = emp.Nome,
                                        GrupolojaId = venda.GrupolojaId,
                                        GrupolojaNome = grp.Nome,
                                        LojaId = venda.LojaId,
                                        LojaNome = loj.Nome
                                    };

                // filtrar searchValue
                if (!string.IsNullOrEmpty(searchValue))
                {
                    vendaSemanalData = vendaSemanalData.Where(x => x.MercadoNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                             x.EmpresaNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                             x.GrupolojaNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                             x.LojaNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase)
                                                       );
                }

                // ordenar lista
                var sortedVendaSemanalData = vendaSemanalData.AsQueryable();
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    sortedVendaSemanalData = sortedVendaSemanalData.OrderBy(sortColumn + " " + sortColumnDirection);
                }

                // retornar lista para a datatable
                recordsTotal = sortedVendaSemanalData.Count();
                var data = sortedVendaSemanalData.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - GetVendas - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Vendas.Create)]
        public async Task<IActionResult> OnGetLoadFromExcel()
        {
            var vendaSemanalViewModel = new VendaSemanalViewModel();
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_LoadFromExcel", vendaSemanalViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.Create)]
        public async Task<IActionResult> OnPostLoadFromExcel(IList<IFormFile> filesExcel)
        {
            var counter = 0;
            if (filesExcel == null) return new ObjectResult(new { status = "error" });

            foreach (IFormFile file in filesExcel)
            {
                try
                {
                    var fileextension = System.IO.Path.GetExtension(file.FileName);
                    var filename = Guid.NewGuid().ToString() + fileextension;
                    var contentPath = System.IO.Path.Combine(_environment.WebRootPath, "ExcelFiles");
                    var filepath = System.IO.Path.Combine(contentPath, filename);
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        file.CopyTo(fs);
                    }

                    XLWorkbook workbook = XLWorkbook.OpenFromTemplate(filepath);


                    foreach (IXLWorksheet ws in workbook.Worksheets)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveStartMessage", "File: " + file.FileName + " | " + ws.Name + " em processamento...");

                        int rowErrors = 0;
                        int rowno = 1;
                        var rows = ws.Rows().ToList();
                        var sheetSize = ws.Rows().Count() - 1;


                        foreach (var row in rows)
                        {
                            if (rowno != 1)
                            {
                                // criar lista de vendas semanais e diárias a partir do ficheiro excel
                                var vendaXL = GetVendaDiariaFromExcelRow(row);
                                if (vendaXL != null)
                                {
                                    // criar VendaSemanal para suportar a venda semanal do excel
                                    var vendaSemanal = await GetVendaSemanalFromExcelAsync(vendaXL);
                                    // criar VendaDiaria da venda diária
                                    var vendaDiaria = GetVendaDiariaFromExcelAsync(vendaXL);

                                    if (vendaSemanal != null)
                                    {
                                        // verificar se a venda semanal já existe na BD
                                        var VendaResponse = await _mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = vendaSemanal.LojaId, Ano = vendaSemanal.Ano, NumeroDaSemana = vendaSemanal.NumeroDaSemana });
                                        if (VendaResponse.Succeeded && VendaResponse.Data != null)
                                        {
                                            // já existe, atualizar
                                            vendaSemanal.Id = VendaResponse.Data.Id;
                                            var updateCommand = _mapper.Map<UpdateVendaSemanalCommand>(vendaSemanal);
                                            var result_updateVS = await _mediator.Send(updateCommand);
                                            if (!result_updateVS.Succeeded)
                                            {
                                                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error updating VS1: " + result_updateVS.Message);
                                            }
                                        }
                                        else
                                        {
                                            // não existe, criar
                                            var createCommand = _mapper.Map<CreateVendaSemanalCommand>(vendaSemanal);
                                            var result_createVS = await _mediator.Send(createCommand);
                                            if (!result_createVS.Succeeded)
                                            {
                                                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error creating VS: " + result_createVS.Message);
                                            }
                                            vendaSemanal.Id = result_createVS.Data;
                                        }
                                        vendaDiaria.VendaSemanalId = vendaSemanal.Id;
                                        vendaDiaria.MercadoId = vendaSemanal.MercadoId;
                                        vendaDiaria.EmpresaId = vendaSemanal.EmpresaId;
                                        vendaDiaria.GrupolojaId = vendaSemanal.GrupolojaId;

                                        // verificar se a venda diária já existe na BD
                                        var vendaDiariaResponse = await _mediator.Send(new GetVendaDiariaByLojaIdDataQuery() { LojaId = vendaDiaria.LojaId, Ano = vendaDiaria.Ano, Mês = vendaDiaria.Mês, DiaDoMês = vendaDiaria.DiaDoMês });
                                        if (vendaDiariaResponse.Succeeded && vendaDiariaResponse.Data != null)
                                        {
                                            // já existe, atualizar
                                            vendaDiaria.Id = vendaDiariaResponse.Data.Id;
                                            var updateCommandVD = _mapper.Map<UpdateVendaDiariaCommand>(vendaDiaria);
                                            var resultVD = await _mediator.Send(updateCommandVD);
                                            if (!resultVD.Succeeded)
                                            {
                                                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error updating VD: " + resultVD.Message);
                                            }
                                        }
                                        else
                                        {
                                            // não existe, criar
                                            var createCommandVD = _mapper.Map<CreateVendaDiariaCommand>(vendaDiaria);
                                            var resultVD = await _mediator.Send(createCommandVD);
                                            if (!resultVD.Succeeded)
                                            {
                                                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error creating VD: " + resultVD.Message);
                                            }
                                        }

                                        // atualizar ValorTotalDaVenda da venda semanal
                                        var vdResponse = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vendaSemanal.Id });
                                        if (vdResponse.Succeeded && vdResponse.Data != null)
                                        {
                                            var vdList = _mapper.Map<List<VendaDiaria>>(vdResponse.Data);
                                            vendaSemanal.ValorTotalDaVenda = vdList.Sum(v => v.ValorDaVenda);
                                            vendaSemanal.ValorTotalDaVenda = Math.Round(vendaSemanal.ValorTotalDaVenda, 2);

                                            // atualizar venda semanal
                                            var updateCommandVS = _mapper.Map<UpdateVendaSemanalCommand>(vendaSemanal);
                                            var resultVS = await _mediator.Send(updateCommandVS);
                                            if (!resultVS.Succeeded)
                                            {
                                                // venda diária inválida
                                                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error updating VS2: Invalid date in row " + row.RowNumber());
                                            }
                                        }
                                        else
                                        {
                                            // venda diária inválida
                                            _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error reading all VD from VS: Invalid data in row " + row.RowNumber());
                                        }

                                        // venda diária válida
                                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Valid data in row " + row.RowNumber());
                                    }
                                    else
                                    {
                                        // venda diária inválida
                                        _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error reading from XL1: Invalid data in row " + row.RowNumber());
                                    }
                                    rowErrors = 0; // reset row errors
                                }
                                else
                                {
                                    // venda diária inválida
                                    rowErrors++;
                                    _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error reading from XL2: Invalid data in row " + row.RowNumber());
                                }
                                counter++;
                                if (rowErrors > 3)
                                {
                                    // demasiados erros na worksheet, parar processamento
                                    _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Too many errors in file: " + file.FileName);
                                    break; ;
                                }
                            }
                            
                            await UpdateProgressBarAsync(rowno - 1, sheetSize, _hubContext);
                            rowno++;
                        }
                    }
                }
                catch
                {
                    return Json(new { status = "error", done = counter - 4}); // -1 cabeçalho -3 rowRerrors
                }
            }

            await StopProgressBarAsync(filesExcel.Count, _hubContext);
            return new ObjectResult(new { status = "success", files = filesExcel.Count, artigos = counter - 4 }); // -1 cabeçalho -3 rowRerrors
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Vendas.Create)]
        public async Task<IActionResult> OnGetLoadObjFromExcel()
        {
            var vendaSemanalViewModel = new VendaSemanalViewModel();
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_LoadObjFromExcel", vendaSemanalViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.Create)]
        public async Task<IActionResult> OnPostLoadObjFromExcel(IList<IFormFile> filesExcel)
        {
            var counter = 0;
            if (filesExcel == null) return new ObjectResult(new { status = "error" });

            foreach (IFormFile file in filesExcel)
            {
                try
                {
                    var fileextension = System.IO.Path.GetExtension(file.FileName);
                    var filename = Guid.NewGuid().ToString() + fileextension;
                    var contentPath = System.IO.Path.Combine(_environment.WebRootPath, "ExcelFiles");
                    var filepath = System.IO.Path.Combine(contentPath, filename);
                    using (FileStream fs = System.IO.File.Create(filepath))
                    {
                        file.CopyTo(fs);
                    }

                    XLWorkbook workbook = XLWorkbook.OpenFromTemplate(filepath);


                    foreach(IXLWorksheet ws in workbook.Worksheets)
                    {

                        //await hub1.SendStartProgressAsync(file.FileName);
                        await _hubContext.Clients.All.SendAsync("ReceiveStartMessage", "File: " + file.FileName + " | " + ws.Name + " em processamento...");

                        int rowno = 1;
                        var rows = ws.Rows().ToList();
                        var sheetSize = ws.Rows().Count() - 1;


                        foreach (var row in rows)
                        {
                            if (rowno != 1)
                            {
                                await UpdateProgressBarAsync(rowno - 1, sheetSize, _hubContext);

                                // ler a venda diária a partir da row do ficheiro excel
                                var venda = GetVendaDiariaFromExcelRow(row);
                                if (venda != null)
                                {
                                    // criar VendaSemanal para suportar a venda semanal
                                    var vendaSemanal = await GetVendaSemanalFromExcelAsync(venda);
                                    // criar VendaDiaria da venda diária lida
                                    var vendaDiaria = GetVendaDiariaFromExcelAsync(venda);

                                    // verificar se a venda semanal já existe na BD
                                    var VendaResponse = await _mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = vendaSemanal.LojaId, NumeroDaSemana = vendaSemanal.NumeroDaSemana });
                                    if (VendaResponse.Succeeded && VendaResponse.Data != null)
                                    {
                                        // já existe, atualizar
                                        vendaSemanal.Id = VendaResponse.Data.Id;
                                        var updateCommand = _mapper.Map<UpdateVendaSemanalCommand>(vendaSemanal);
                                        var result_updateVS = await _mediator.Send(updateCommand);
                                        if (!result_updateVS.Succeeded)
                                        {
                                            _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error: " + result_updateVS.Message);
                                        }
                                    }
                                    else
                                    {
                                        // não existe, criar
                                        var createCommand = _mapper.Map<CreateVendaSemanalCommand>(vendaSemanal);
                                        var result_createVS = await _mediator.Send(createCommand);
                                        if (!result_createVS.Succeeded)
                                        {
                                            _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error: " + result_createVS.Message);
                                        }
                                        vendaSemanal.Id = result_createVS.Data;
                                    }
                                    vendaDiaria.VendaSemanalId = vendaSemanal.Id;


                                    // verificar se a venda diária já existe na BD
                                    var vdResponse = await _mediator.Send(new GetVendaDiariaByVendaSemanalIdDiaQuery() { VendaSemanalId = vendaSemanal.Id,DiaDaSemana = vendaDiaria.DiaDaSemana });
                                    if (vdResponse.Succeeded && vdResponse.Data != null)
                                    {
                                        // já existe, atualizar
                                        vendaDiaria.Id = vdResponse.Data.Id;
                                        var updateCommandVD = _mapper.Map<UpdateVendaDiariaCommand>(vendaDiaria);
                                        var resultVD = await _mediator.Send(updateCommandVD);
                                        if (!resultVD.Succeeded)
                                        {
                                            _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error: " + resultVD.Message);
                                        }
                                    }
                                    else
                                    {
                                        // não existe, criar
                                        var createCommandVD = _mapper.Map<CreateVendaDiariaCommand>(vendaDiaria);
                                        var resultVD = await _mediator.Send(createCommandVD);
                                        if (!resultVD.Succeeded)
                                        {
                                            _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error: " + resultVD.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    // venda diária inválida
                                    _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostLoadFromExcel - Error: Invalid date in row " + row.RowNumber());
                                }
                                counter++;
                            }
                            rowno++;
                        }

                    }
                }
                catch
                {
                    return Json(new { status = "error", done = counter });
                }
            }

            await StopProgressBarAsync(filesExcel.Count, _hubContext);
            return new ObjectResult(new { status = "success", files = filesExcel.Count, artigos = counter });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Artigos.Create)]
        public JsonResult OnPostCancel()
        {
            DeleteExcelFolderContent(_environment);
            return new JsonResult(new { status = "success" });
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Vendas.Edit)]
        public async Task<JsonResult> OnGetEdit(int lojaId = 0, int numeroSemana = 0, int ano = 0)
        {
            if (lojaId == 0 || numeroSemana == 0 || ano == 0)
            {
                // retornar invalid
                // return new JsonResult(new { isValid = false, html = "" });
                return null;
            }
            else
            {
                var vscvm = new VendaSemanalComparaViewModel();
                vscvm.CurrentRole = await GetCurrentRoleAsync();

                try
                {
                    // editar venda semanal
                    vscvm.AnoAtual = await GetVendaSemanalWithDiárias(lojaId, ano, numeroSemana, _mapper, _mediator);
                    if (vscvm.AnoAtual == null)
                    {
                        // Venda semanal não existe para esta semana
                        _notify.Error(_localizer["Não existe Venda semanal."]);
                        return new JsonResult(new { isValid = false, html = "" });
                    }


                    vscvm.AnoAtual.Mercados = await MercadoController.GetSelectListOneMercadoAsync(vscvm.AnoAtual.MercadoId, _mapper, _mediator);
                    vscvm.AnoAtual.Empresas = await EmpresaController.GetSelectListOneEmpresaAsync(vscvm.AnoAtual.EmpresaId, _mapper, _mediator);
                    vscvm.AnoAtual.GruposLojas = await GrupolojaController.GetSelectListOneGrupoLojaAsync(vscvm.AnoAtual.GrupolojaId, _mapper, _mediator);
                    vscvm.AnoAtual.Lojas = await LojaController.GetSelectListOneLojaAsync(vscvm.AnoAtual.LojaId, _mapper, _mediator);



                    // Objetivo Mensal
                    var vendasDiariasMes = await GetVendasMensaisAsync(vscvm.CurrentRole.LojaId, DateTime.Now.Year, DateTime.Now.Month, _mapper, _mediator);
                    vscvm.AnoAtual.ValorTotalMensalDaVenda = vendasDiariasMes.Sum(v => v.ValorDaVenda);
                    vscvm.AnoAtual.ValorTotalMensalDaVenda = Math.Round(vscvm.AnoAtual.ValorTotalMensalDaVenda, 2);
                    //var prevMonday = vscvm.AnoAtual.DataInicialDaSemana.Day;
                    vendasDiariasMes.RemoveAll(v => v.VendaSemanalId == vscvm.AnoAtual.Id);
                    vscvm.AnoAtual.ValorAcumuladoMensal = vendasDiariasMes.Sum(v => v.ValorDaVenda);
                    var vendasDiariasMesAnoPassado = await VendaSemanalController.GetVendasMensaisAsync(vscvm.CurrentRole.LojaId, DateTime.Now.Year - 1, DateTime.Now.Month, _mapper, _mediator);
                    var totalLastYear = vendasDiariasMesAnoPassado.Sum(v => v.ValorDaVenda);
                    vscvm.AnoAtual.ObjetivoMensalDaVenda = totalLastYear + (totalLastYear * 0.1);
                    vscvm.AnoAtual.ObjetivoMensalDaVenda = Math.Round(vscvm.AnoAtual.ObjetivoMensalDaVenda, 2);



                    //verificar se existe ano anterior
                    vscvm.AnoAnterior = await GetVendaSemanalWithDiárias(vscvm.AnoAtual.LojaId, vscvm.AnoAtual.Ano - 1, vscvm.AnoAtual.NumeroDaSemana, _mapper, _mediator);
                    if (vscvm.AnoAnterior != null)
                    {
                        vscvm.AnoAtual.ValorTotalDaVendaDoAnoAnterior = Math.Round(vscvm.AnoAnterior.ValorTotalDaVenda, 2, MidpointRounding.AwayFromZero);
                        vscvm.AnoAtual.ObjetivoDaVendaSemanal = Math.Round(vscvm.AnoAtual.ObjetivoDaVendaSemanal, 2, MidpointRounding.AwayFromZero);
                        vscvm.AnoAtual.VariaçaoAnual = (((vscvm.AnoAtual.ObjetivoDaVendaSemanal - vscvm.AnoAtual.ValorTotalDaVendaDoAnoAnterior) * 100 / vscvm.AnoAtual.ValorTotalDaVendaDoAnoAnterior) * 100) / 100;
                        vscvm.AnoAtual.VariaçaoAnual = Math.Round(vscvm.AnoAtual.VariaçaoAnual, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        vscvm.AnoAnterior = await GetVendaSemanalEmptyAnoAnteriorAsync(vscvm.AnoAtual.DataInicialDaSemana, _mapper, _mediator);
                        vscvm.AnoAtual.ValorTotalDaVendaDoAnoAnterior = 0.00;
                    }

                    // retornar VendaSemanalComparaViewModel
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Edit", vscvm) });
                }
                catch (Exception ex)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnGetCreateOrEdit - Exception: " + ex.Message);
                    // retornar VendaSemanalComparaViewModel
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Edit", vscvm) });
                }
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.Edit)]
        public async Task<JsonResult> OnPostEdit(int id, VendaSemanalComparaViewModel venda)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // atualizar DataFinalDaSemana
                    venda.AnoAtual.DataFinalDaSemana = venda.AnoAtual.DataInicialDaSemana.AddDays(6);

                    // atualizar a venda semanal com Id = id
                    venda.AnoAtual.Id = id;
                    var updateVendaCommand = _mapper.Map<UpdateVendaSemanalCommand>(venda.AnoAtual);
                    var resultUVS = await _mediator.Send(updateVendaCommand);
                    if (resultUVS.Succeeded)
                    {
                        _notify.Information($"{_localizer["Venda semanal com ID"]} {resultUVS.Data} {_localizer["atualizada."]}");

                        // atualizar vendas diárias
                        var success = await UpdateVendasDiáriasFromSemanalAsync(venda.AnoAtual, _mapper, _mediator);
                    }
                    else
                    {
                        _notify.Error(resultUVS.Message);
                        return new JsonResult(new { isValid = false });
                    }

                    // return _ViewAll
                    //var html = await _viewRenderer.RenderViewToStringAsync("Ocorrencia/Ocorrencia/_ViewAll", new List<VendaSemanalViewModel>());
                    //return new JsonResult(new { isValid = true, html = html });
                    return new JsonResult(new { isValid = true });
                }
                else
                {
                    // ModelState not valid
                    // devolver VendaSemanalComparaViewModel para continuar edição

                    // atualizar DataFinalDaSemana
                    venda.AnoAtual.DataFinalDaSemana = venda.AnoAtual.DataInicialDaSemana.AddDays(6);

                    var vendasDiariasMes = await GetVendasMensaisAsync(venda.CurrentRole.LojaId, venda.AnoAtual.Ano, venda.AnoAtual.DataInicialDaSemana.Month, _mapper, _mediator);
                    venda.AnoAtual.ValorTotalMensalDaVenda = vendasDiariasMes.Sum(v => v.ValorDaVenda);
                    venda.AnoAtual.ValorTotalMensalDaVenda = Math.Round(venda.AnoAtual.ValorTotalMensalDaVenda, 2);
                    var prevMonday = venda.AnoAtual.DataInicialDaSemana.Day;
                    venda.AnoAtual.ValorAcumuladoMensal = vendasDiariasMes.Where(v => v.DiaDoMês < venda.AnoAtual.DataInicialDaSemana.Day).Sum(v => v.ValorDaVenda);
                    var vendasDiariasMesAnoPassado = await VendaSemanalController.GetVendasMensaisAsync(venda.CurrentRole.LojaId, venda.AnoAtual.Ano - 1, venda.AnoAtual.DataInicialDaSemana.Month, _mapper, _mediator);
                    var totalLastYear = vendasDiariasMesAnoPassado.Sum(v => v.ValorDaVenda);
                    venda.AnoAtual.ObjetivoMensalDaVenda = totalLastYear + (totalLastYear * 0.1);
                    venda.AnoAtual.ObjetivoMensalDaVenda = Math.Round(venda.AnoAtual.ObjetivoMensalDaVenda, 2);

                    var responseVendaAnoAnterior = await _mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = venda.AnoAtual.LojaId, Ano = venda.AnoAtual.Ano - 1, NumeroDaSemana = venda.AnoAtual.NumeroDaSemana });
                    if (responseVendaAnoAnterior.Succeeded && responseVendaAnoAnterior.Data != null)
                    {
                        venda.AnoAnterior = _mapper.Map<VendaSemanalViewModel>(responseVendaAnoAnterior.Data);
                        venda.AnoAtual.ValorTotalDaVendaDoAnoAnterior = venda.AnoAnterior.ValorTotalDaVenda;
                    }
                    else
                    {
                        venda.AnoAtual.ValorTotalDaVendaDoAnoAnterior = 0.00;
                    }

                    // return _ViewAll
                    var html = await _viewRenderer.RenderViewToStringAsync("Ocorrencia/Ocorrencia/_ViewAll", new List<VendaSemanalViewModel>());
                    return new JsonResult(new { isValid = true });
                }
            }
            catch (Exception ex)
            {
                // devolver VendaSemanalViewModel para continuar edição
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostCreateOrEdit - Exception: " + ex.Message);
                _notify.Error($"{_localizer["Erro ao salvar a Venda semanal. Possível duplicado."]}");

                // atualizar DataFinalDaSemana
                venda.AnoAtual.DataFinalDaSemana = venda.AnoAtual.DataInicialDaSemana.AddDays(6);

                // atualizar a venda semanal com Id = id
                venda.AnoAtual.Id = id;

                var responseVendaAnoAnterior = await _mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = venda.AnoAtual.LojaId, Ano = venda.AnoAtual.Ano - 1, NumeroDaSemana = venda.AnoAtual.NumeroDaSemana });
                if (responseVendaAnoAnterior.Succeeded && responseVendaAnoAnterior.Data != null)
                {
                    venda.AnoAnterior = _mapper.Map<VendaSemanalViewModel>(responseVendaAnoAnterior.Data);
                    venda.AnoAtual.ValorTotalDaVendaDoAnoAnterior = venda.AnoAnterior.ValorTotalDaVenda;
                }
                else
                {
                    venda.AnoAtual.ValorTotalDaVendaDoAnoAnterior = 0.00;
                }

                // return _ViewAll
                //var html = await _viewRenderer.RenderViewToStringAsync("Ocorrencia/Ocorrencia/_ViewAll", new List<VendaSemanalViewModel>());
                //return new JsonResult(new { isValid = true, html = html });
                return new JsonResult(new { isValid = true });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Vendas.Edit)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            var vscvm = new VendaSemanalComparaViewModel();
            vscvm.CurrentRole = await GetCurrentRoleAsync();

            try
            {
                if (id == 0)
                {
                    // criar nova venda semanal vazia para edição
                    vscvm.AnoAtual = await GetVendaSemanalEmptyAsync(_mapper, _mediator);
                    vscvm.AnoAnterior = await GetVendaSemanalEmptyAnoAnteriorAsync(DateTime.Now.AddDays(7).MondayOfWeek(), _mapper, _mediator);
                }
                else
                {
                    // editar venda semanal
                    vscvm.AnoAtual = await GetVendaSemanalWithDiárias(id, _mapper, _mediator);
                    if (vscvm.AnoAtual == null) return null;

                    var vendasDiariasMes = await GetVendasMensaisAsync(vscvm.CurrentRole.LojaId, vscvm.AnoAtual.Ano, vscvm.AnoAtual.DataInicialDaSemana.Month, _mapper, _mediator);
                    vscvm.AnoAtual.ValorTotalMensalDaVenda = vendasDiariasMes.Sum(v => v.ValorDaVenda);
                    var vendasDiariasMesAnoPassado = await VendaSemanalController.GetVendasMensaisAsync(vscvm.CurrentRole.LojaId, vscvm.AnoAtual.Ano - 1, vscvm.AnoAtual.DataInicialDaSemana.Month, _mapper, _mediator);
                    var totalLastYear = vendasDiariasMesAnoPassado.Sum(v => v.ValorDaVenda);
                    vscvm.AnoAtual.ObjetivoMensalDaVenda = totalLastYear + (totalLastYear * 0.1);
                    vscvm.AnoAtual.ObjetivoMensalDaVenda = Math.Round(vscvm.AnoAtual.ObjetivoMensalDaVenda, 2);

                    vscvm.AnoAtual.Mercados = await MercadoController.GetSelectListOneMercadoAsync(vscvm.AnoAtual.MercadoId, _mapper, _mediator);
                    vscvm.AnoAtual.Empresas = await EmpresaController.GetSelectListOneEmpresaAsync(vscvm.AnoAtual.EmpresaId, _mapper, _mediator);
                    vscvm.AnoAtual.GruposLojas = await GrupolojaController.GetSelectListOneGrupoLojaAsync(vscvm.AnoAtual.GrupolojaId, _mapper, _mediator);
                    vscvm.AnoAtual.Lojas = await LojaController.GetSelectListOneLojaAsync(vscvm.AnoAtual.LojaId, _mapper, _mediator);

                    //verificar se existe ano anterior
                    vscvm.AnoAnterior = await GetVendaSemanalWithDiárias(vscvm.AnoAtual.LojaId, vscvm.AnoAtual.Ano - 1, vscvm.AnoAtual.NumeroDaSemana, _mapper, _mediator);
                    if (vscvm.AnoAnterior != null)
                    {
                        vscvm.AnoAtual.ValorTotalDaVendaDoAnoAnterior = Math.Round(vscvm.AnoAnterior.ValorTotalDaVenda, 2, MidpointRounding.AwayFromZero);
                        vscvm.AnoAtual.ObjetivoDaVendaSemanal = Math.Round(vscvm.AnoAtual.ObjetivoDaVendaSemanal, 2, MidpointRounding.AwayFromZero);
                        vscvm.AnoAtual.VariaçaoAnual = (((vscvm.AnoAtual.ObjetivoDaVendaSemanal - vscvm.AnoAtual.ValorTotalDaVendaDoAnoAnterior) * 100 / vscvm.AnoAtual.ValorTotalDaVendaDoAnoAnterior) * 100) / 100;
                        vscvm.AnoAtual.VariaçaoAnual = Math.Round(vscvm.AnoAtual.VariaçaoAnual, 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        vscvm.AnoAnterior = await GetVendaSemanalEmptyAnoAnteriorAsync(vscvm.AnoAtual.DataInicialDaSemana, _mapper, _mediator);
                        vscvm.AnoAtual.ValorTotalDaVendaDoAnoAnterior = 0.00;
                    }
                }

                // retornar VendaSemanalComparaViewModel
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", vscvm) });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnGetCreateOrEdit - Exception: " + ex.Message);
                // retornar VendaSemanalComparaViewModel
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", vscvm) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.Edit)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, VendaSemanalComparaViewModel venda)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id == 0)
                    {
                        // verificar empresaId e grupolojaId
                        if (venda.AnoAtual.EmpresaId == 0 || venda.AnoAtual.GrupolojaId == 0 || venda.AnoAtual.MercadoId == 0)
                        {
                            var lj = await LojaController.GetLojaAsync(venda.AnoAtual.LojaId, _mapper, _mediator);
                            venda.AnoAtual.EmpresaId = lj.EmpresaId;
                            venda.AnoAtual.GrupolojaId = lj.GrupolojaId;
                            venda.AnoAtual.MercadoId = lj.MercadoId;
                        }

                        // atualizar DataFinalDaSemana
                        venda.AnoAtual.DataFinalDaSemana = venda.AnoAtual.DataInicialDaSemana.AddDays(6);

                        // criar nova venda semanal
                        var createVendaCommand = _mapper.Map<CreateVendaSemanalCommand>(venda.AnoAtual);
                        var resultCVS = await _mediator.Send(createVendaCommand);
                        if (resultCVS.Succeeded)
                        {
                            _notify.Success($"{_localizer["Venda semanal com ID"]} {resultCVS.Data} {_localizer["criada."]}");

                            // criar vendas diárias
                            for (var i = 1; i < 8; i++)
                            {
                                var valorDaVenda = 0.0;
                                var totalArtigos = 0;
                                var percentConv = 0.00;
                                var observacoes = string.Empty;
                                var weather = new Weather();
                                switch (i)
                                {
                                    case 1:
                                        valorDaVenda = venda.AnoAtual.Valor2Feira;
                                        totalArtigos = venda.AnoAtual.TotalArtigos2Feira;
                                        percentConv = venda.AnoAtual.Conv2Feira;
                                        observacoes = venda.AnoAtual.Observacoes2Feira;
                                        weather = venda.AnoAtual.Weather2Feira;
                                        break;
                                    case 2:
                                        valorDaVenda = venda.AnoAtual.Valor3Feira;
                                        totalArtigos = venda.AnoAtual.TotalArtigos3Feira;
                                        percentConv = venda.AnoAtual.Conv3Feira;
                                        observacoes = venda.AnoAtual.Observacoes3Feira;
                                        weather = venda.AnoAtual.Weather3Feira;
                                        break;
                                    case 3:
                                        valorDaVenda = venda.AnoAtual.Valor4Feira;
                                        totalArtigos = venda.AnoAtual.TotalArtigos4Feira;
                                        percentConv = venda.AnoAtual.Conv4Feira;
                                        observacoes = venda.AnoAtual.Observacoes4Feira;
                                        weather = venda.AnoAtual.Weather4Feira;
                                        break;
                                    case 4:
                                        valorDaVenda = venda.AnoAtual.Valor5Feira;
                                        totalArtigos = venda.AnoAtual.TotalArtigos5Feira;
                                        percentConv = venda.AnoAtual.Conv5Feira;
                                        observacoes = venda.AnoAtual.Observacoes5Feira;
                                        weather = venda.AnoAtual.Weather5Feira;
                                        break;
                                    case 5:
                                        valorDaVenda = venda.AnoAtual.Valor6Feira;
                                        totalArtigos = venda.AnoAtual.TotalArtigos6Feira;
                                        percentConv = venda.AnoAtual.Conv6Feira;
                                        observacoes = venda.AnoAtual.Observacoes6Feira;
                                        weather = venda.AnoAtual.Weather6Feira;
                                        break;
                                    case 6:
                                        valorDaVenda = venda.AnoAtual.ValorSábado;
                                        totalArtigos = venda.AnoAtual.TotalArtigosSab;
                                        percentConv = venda.AnoAtual.ConvSab;
                                        observacoes = venda.AnoAtual.ObservacoesSab;
                                        weather = venda.AnoAtual.WeatherSab;
                                        break;
                                    case 7:
                                        valorDaVenda = venda.AnoAtual.ValorDomingo;
                                        totalArtigos = venda.AnoAtual.TotalArtigosDom;
                                        percentConv = venda.AnoAtual.ConvDom;
                                        observacoes = venda.AnoAtual.ObservacoesDom;
                                        weather = venda.AnoAtual.WeatherDom;
                                        break;
                                }

                                var dataDaVenda = venda.AnoAtual.DataInicialDaSemana.AddDays(i - 1);
                                var vd = new VendaDiariaViewModel
                                {
                                    DiaDaSemana = i,
                                    DataDaVenda = dataDaVenda,
                                    Ano = dataDaVenda.Year,
                                    Mês = dataDaVenda.Month,
                                    DiaDoMês = dataDaVenda.Day,                                    
                                    VendaSemanalId = resultCVS.Data,
                                    LojaId = venda.AnoAtual.LojaId,
                                    ValorDaVenda = valorDaVenda,
                                    TotalArtigos = totalArtigos,
                                    PercentConv = percentConv,
                                    Observacoes = observacoes,
                                    Weather = weather
                                };
                                var createVendaDiariaCommand = _mapper.Map<CreateVendaDiariaCommand>(vd);
                                var resultCVD = await _mediator.Send(createVendaDiariaCommand);
                                if (!resultCVD.Succeeded)
                                {
                                    // venda diária não foi criada
                                    _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostCreateOrEdit - Error creating CVD: " + resultCVD.Message);
                                }
                                else
                                {
                                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostCreateOrEdit - CVD created: " + resultCVD.Data);
                                }
                            }
                        }
                        else
                        {
                            _notify.Error(resultCVS.Message);
                        }
                    }
                    else
                    {
                        // atualizar DataFinalDaSemana
                        venda.AnoAtual.DataFinalDaSemana = venda.AnoAtual.DataInicialDaSemana.AddDays(6);

                        // atualizar a venda semanal com Id = id
                        venda.AnoAtual.Id = id;
                        var updateVendaCommand = _mapper.Map<UpdateVendaSemanalCommand>(venda.AnoAtual);
                        var resultUVS = await _mediator.Send(updateVendaCommand);
                        if (resultUVS.Succeeded)
                        {
                            _notify.Information($"{_localizer["Venda semanal com ID"]} {resultUVS.Data} {_localizer["atualizada."]}");

                            // atualizar vendas diárias
                            var success = await UpdateVendasDiáriasFromSemanalAsync(venda.AnoAtual, _mapper, _mediator);
                        }
                        else _notify.Error(resultUVS.Message);
                    }

                    // return _ViewAll
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new List<VendaSemanalViewModel>());
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    // ModelState not valid
                    // devolver VendaSemanalComparaViewModel para continuar edição
                    
                    // atualizar DataFinalDaSemana
                    venda.AnoAtual.DataFinalDaSemana = venda.AnoAtual.DataInicialDaSemana.AddDays(6);

                    // atualizar a venda semanal com Id = id
                    venda.AnoAtual.Id = id;
                    venda.AnoAtual.Mercados = await MercadoController.GetSelectListAllMercadosAsync(venda.AnoAtual.MercadoId, _mapper, _mediator);
                    venda.AnoAtual.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(venda.AnoAtual.EmpresaId, _mapper, _mediator);
                    venda.AnoAtual.GruposLojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(venda.AnoAtual.EmpresaId, venda.AnoAtual.GrupolojaId, _mapper, _mediator);
                    venda.AnoAtual.Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(venda.AnoAtual.GrupolojaId, venda.AnoAtual.LojaId, _mapper, _mediator);

                    var responseVendaAnoAnterior = await _mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = venda.AnoAtual.LojaId, Ano = venda.AnoAtual.Ano - 1, NumeroDaSemana = venda.AnoAtual.NumeroDaSemana });
                    if (responseVendaAnoAnterior.Succeeded && responseVendaAnoAnterior.Data != null)
                    {
                        venda.AnoAnterior = _mapper.Map<VendaSemanalViewModel>(responseVendaAnoAnterior.Data);
                        venda.AnoAtual.ValorTotalDaVendaDoAnoAnterior = venda.AnoAnterior.ValorTotalDaVenda;
                    }
                    else
                    {
                        venda.AnoAtual.ValorTotalDaVendaDoAnoAnterior = 0.00;
                    }

                    var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", venda);
                    return new JsonResult(new { isValid = false, html = html });
                }
            }
            catch (Exception ex)
            {
                // devolver VendaSemanalViewModel para continuar edição
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - OnPostCreateOrEdit - Exception: " + ex.Message);
                _notify.Error($"{_localizer["Erro ao salvar a Venda semanal. Possível duplicado."]}");

                // atualizar DataFinalDaSemana
                venda.AnoAtual.DataFinalDaSemana = venda.AnoAtual.DataInicialDaSemana.AddDays(6);

                // atualizar a venda semanal com Id = id
                venda.AnoAtual.Id = id;

                venda.AnoAtual.Mercados = await MercadoController.GetSelectListAllMercadosAsync(venda.AnoAtual.MercadoId, _mapper, _mediator);
                venda.AnoAtual.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(venda.AnoAtual.EmpresaId, _mapper, _mediator);
                venda.AnoAtual.GruposLojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(venda.AnoAtual.EmpresaId, venda.AnoAtual.GrupolojaId, _mapper, _mediator);
                venda.AnoAtual.Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(venda.AnoAtual.GrupolojaId, venda.AnoAtual.LojaId, _mapper, _mediator);

                var responseVendaAnoAnterior = await _mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = venda.AnoAtual.LojaId, Ano = venda.AnoAtual.Ano - 1, NumeroDaSemana = venda.AnoAtual.NumeroDaSemana });
                if (responseVendaAnoAnterior.Succeeded && responseVendaAnoAnterior.Data != null)
                {
                    venda.AnoAnterior = _mapper.Map<VendaSemanalViewModel>(responseVendaAnoAnterior.Data);
                    venda.AnoAtual.ValorTotalDaVendaDoAnoAnterior = venda.AnoAnterior.ValorTotalDaVenda;
                }
                else
                {
                    venda.AnoAtual.ValorTotalDaVendaDoAnoAnterior = 0.00;
                }

                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", venda);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteVendaSemanalCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Venda semanal com ID"]} {id} {_localizer["removida."]}");
                var response = await _mediator.Send(new GetAllVendasSemanaisCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<VendaSemanalViewModel>>(response.Data);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    return null;
                }
            }
            else
            {
                _notify.Error(deleteCommand.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        public static bool DeleteExcelFolderContent(IWebHostEnvironment env)
        {
            try
            {
                var contentPath = System.IO.Path.Combine(env.WebRootPath, "ArtigosExcel");
                DirectoryInfo di = new DirectoryInfo(contentPath);
                var files = di.GetFiles();
                foreach (var file in files)
                {
                    file.Delete();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.Edit)]
        public JsonResult GetMondayDate(int ano = 0, int semana = 0)
        {
            var weekStartDate = string.Empty;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { weekStartDate = weekStartDate });

            if (ano > 0 && semana > 0)
            {
                weekStartDate = FirstDateOfWeek(ano, semana).ToShortDateString();
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { weekStartDate = weekStartDate });

            }
            return Json(jsonString);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve a venda semanal defenida por lojaId, ano, semana
        /// </summary>
        /// <returns>retorna a VendaSemanalViewModel em Jason</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.Edit)]
        public async Task<JsonResult> GetVendaSemanal(int lojaId = 0, int ano = 0, int semana = 0)
        {
            var jsonString = string.Empty;
            var vsvm = new VendaSemanalViewModel();
            if (lojaId == 0 || ano == 0 || semana == 0)
            {
                // retornar venda semanal vazia
                vsvm = await GetVendaSemanalEmptyAsync(_mapper, _mediator);
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { success = false, vendaSemanal = vsvm });
                return Json(jsonString);
            }
            var vendaSemanal = await GetVendaSemanalWithDiárias( lojaId, ano, semana, _mapper, _mediator);
            if (vendaSemanal == null)
            {
                // retornar venda semanal vazia
                vsvm = await GetVendaSemanalEmptyAsync(_mapper, _mediator);
                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { success = false, vendaSemanal = vsvm });
                return Json(jsonString);
            }

            // retornar venda semanal
            jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { success = true, vendaSemanal = vendaSemanal });
            return Json(jsonString);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve a venda semanal defenida por VendaSemanalId
        /// </summary>
        /// <returns>retorna a VendaSemanalViewModel com o id=VendaSemanalId</returns>

        public static async Task<VendaSemanalViewModel> GetVendaSemanalByIdAsync(int VendaSemanalId, IMapper mapper, IMediator mediator)
        {
            if (VendaSemanalId == 0) return null;
            var vendaSemanal = await GetVendaSemanalWithDiárias(VendaSemanalId, mapper, mediator);
            return vendaSemanal;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Actualiza a VendaSemanalViewModel (Id) com as vendas diárias.
        /// </summary>
        /// <returns>VendaSemanalViewModel preenchida</returns>

        internal static async Task<VendaSemanalViewModel> GetVendaSemanalWithDiárias(int Id, IMapper mapper, IMediator mediator)
        {
            try
            {
                if (Id > 0)
                {
                    // ler a VendaSemanal da db
                    var VendaSemanalResponse = await mediator.Send(new GetVendaSemanalByIdQuery() { Id = Id });
                    if (!VendaSemanalResponse.Succeeded || VendaSemanalResponse.Data == null) return null;
                    var vsvm = mapper.Map<VendaSemanalViewModel>(VendaSemanalResponse.Data);

                    // ler as VendaDiárias da db
                    var VendasDiáriasResponse = await mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vsvm.Id });
                    if (!VendasDiáriasResponse.Succeeded || VendasDiáriasResponse.Data == null) return null;
                    var vdList = mapper.Map<List<VendaDiariaViewModel>>(VendasDiáriasResponse.Data);

                    // atualizar vendas diárias
                    for (var i = 1; i < 8; i++)
                    {
                        var vd = vdList.Find(v => v.DiaDaSemana == i);
                        var valorDaVenda = vd == null ? 0.0 : vd.ValorDaVenda;

                        switch (i)
                        {
                            case 1:
                                vsvm.Valor2Feira = valorDaVenda;
                                vsvm.Observacoes2Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos2Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv2Feira = vd == null ? 0 : vd.PercentConv;
                                vsvm.Weather2Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 2:
                                vsvm.Valor3Feira = valorDaVenda;
                                vsvm.Observacoes3Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos3Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv3Feira = vd == null ? 0 : vd.PercentConv;
                                vsvm.Weather3Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 3:
                                vsvm.Valor4Feira = valorDaVenda;
                                vsvm.Observacoes4Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos4Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv4Feira = vd == null ? 0 : vd.PercentConv;
                                vsvm.Weather4Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 4:
                                vsvm.Valor5Feira = valorDaVenda;
                                vsvm.Observacoes5Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos5Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv5Feira = vd == null ? 0 : vd.PercentConv;
                                vsvm.Weather5Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 5:
                                vsvm.Valor6Feira = valorDaVenda;
                                vsvm.Observacoes6Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos6Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv6Feira = vd == null ? 0 : vd.PercentConv;
                                vsvm.Weather6Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 6:
                                vsvm.ValorSábado = valorDaVenda;
                                vsvm.ObservacoesSab = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigosSab = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.ConvSab = vd == null ? 0 : vd.PercentConv;
                                vsvm.WeatherSab = vd == null ? 0 : vd.Weather;
                                break;
                            case 7:
                                vsvm.ValorDomingo = valorDaVenda;
                                vsvm.ObservacoesDom = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigosDom = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.ConvDom = vd == null ? 0 : vd.PercentConv;
                                vsvm.WeatherDom = vd == null ? 0 : vd.Weather;
                                break;
                        }
                    }

                    return vsvm;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Actualiza a VendaSemanalViewModel (Id) com as vendas diárias.
        /// </summary>
        /// <returns>VendaSemanalViewModel preenchida</returns>

        internal static async Task<VendaSemanalViewModel> GetVendaSemanalWithDiárias(int lojaId, int ano, int semana, IMapper mapper, IMediator mediator)
        {
            try
            {
                if (lojaId > 0 && ano > 0 && semana > 0)
                {
                    // ler a VendaSemanal da
                    var VendaSemanalResponse = await mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = lojaId, Ano = ano, NumeroDaSemana = semana });
                    if (!VendaSemanalResponse.Succeeded || VendaSemanalResponse.Data == null) return null;
                    var vsvm = mapper.Map<VendaSemanalViewModel>(VendaSemanalResponse.Data);

                    // ler as VendaDiárias da db
                    var VendasDiáriasResponse = await mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vsvm.Id });
                    if (!VendasDiáriasResponse.Succeeded || VendasDiáriasResponse.Data == null) return null;
                    var vdList = mapper.Map<List<VendaDiariaViewModel>>(VendasDiáriasResponse.Data);

                    // atualizar vendas diárias
                    for (var i = 1; i < 8; i++)
                    {
                        var vd = vdList.Find(v => v.DiaDaSemana == i);
                        var valorDaVenda = vd == null ? 0.0 : vd.ValorDaVenda;

                        switch (i)
                        {
                            case 1:
                                vsvm.Valor2Feira = valorDaVenda;
                                vsvm.Observacoes2Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos2Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv2Feira = vd == null ? 0.00 : vd.PercentConv;
                                vsvm.Weather2Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 2:
                                vsvm.Valor3Feira = valorDaVenda;
                                vsvm.Observacoes3Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos3Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv3Feira = vd == null ? 0.00 : vd.PercentConv;
                                vsvm.Weather3Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 3:
                                vsvm.Valor4Feira = valorDaVenda;
                                vsvm.Observacoes4Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos4Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv4Feira = vd == null ? 0.00 : vd.PercentConv;
                                vsvm.Weather4Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 4:
                                vsvm.Valor5Feira = valorDaVenda;
                                vsvm.Observacoes5Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos5Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv5Feira = vd == null ? 0.00 : vd.PercentConv;
                                vsvm.Weather5Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 5:
                                vsvm.Valor6Feira = valorDaVenda;
                                vsvm.Observacoes6Feira = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigos6Feira = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.Conv6Feira = vd == null ? 0.00 : vd.PercentConv;
                                vsvm.Weather6Feira = vd == null ? 0 : vd.Weather;
                                break;
                            case 6:
                                vsvm.ValorSábado = valorDaVenda;
                                vsvm.ObservacoesSab = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigosSab = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.ConvSab = vd == null ? 0.00 : vd.PercentConv;
                                vsvm.WeatherSab = vd == null ? 0 : vd.Weather;
                                break;
                            case 7:
                                vsvm.ValorDomingo = valorDaVenda;
                                vsvm.ObservacoesDom = vd == null ? "" : vd.Observacoes;
                                vsvm.TotalArtigosDom = vd == null ? 0 : vd.TotalArtigos;
                                vsvm.ConvDom = vd == null ? 0.00 : vd.PercentConv;
                                vsvm.WeatherDom = vd == null ? 0 : vd.Weather;
                                break;
                        }
                    }

                    return vsvm;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve lista de vendas diarias pertencentes a um mes.
        /// </summary>
        /// <returns>List<VendaDiariaViewModel></returns>

        public static async Task<List<VendaDiariaViewModel>> GetVendasMensaisAsync(int lojaId, int ano, int mes, IMapper mapper, IMediator mediator)
        {
            var vdList = new List<VendaDiariaViewModel>();

            try
            {                
                if (lojaId > 0 && ano > 0 && mes > 0)
                {
                    // ler as VendaDiárias da db
                    var VendasDiáriasResponse = await mediator.Send(new GetVendasDiariasByLojaIdMesQuery() { LojaId = lojaId, Ano = ano, Mês = mes });
                    if (!VendasDiáriasResponse.Succeeded || VendasDiáriasResponse.Data == null) return vdList;
                    return mapper.Map<List<VendaDiariaViewModel>>(VendasDiáriasResponse.Data);
                }
                else
                {
                    return vdList;
                }

            }
            catch (Exception ex)
            {
                return vdList;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara a lista de vendas semanais existentes tendo em conta 
        /// o role do user corrente.
        /// a tabela de vendas semanais é carregada com esta lista em _ViewAll
        /// </summary>
        /// <returns></returns>

        internal async Task<IQueryable<VendaSemanalListViewModel>> GetVendaSemanalListAsync()
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

            var viewModelList = new List<VendaSemanalListViewModel>().AsQueryable();

            if (isSuperAdmin || isAdmin || isRevisor) // todas as vendas
            {
                var response = await _mediator.Send(new GetAllVendasSemanaisCachedQuery());
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<VendaSemanalListViewModel>>(response.Data).AsQueryable();
                }
            }

            if (isSupervisor) // vendas de grupoloja
            {
                var response = await _mediator.Send(new GetVendasSemanaisByGrupolojaIdQuery() { GrupolojaId = grupolojaId });
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<VendaSemanalListViewModel>>(response.Data).AsQueryable();
                }
            }

            if (isGerenteLoja || isColaborador || isBasic) // vendas de loja
            {
                var response = await _mediator.Send(new GetVendasSemanaisByLojaIdQuery() { LojaId = lojaId });
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<VendaSemanalListViewModel>>(response.Data).AsQueryable();
                }
            }

            return viewModelList;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// calcula o valor total semanal em vendas da loja, grupo, empresa ou mercado
        /// </summary>
        /// <returns>double ValorTotalDaVenda</returns>

        internal static async Task<double> GetValorTotalDaVenda(int mercadoId, int empresaId, int grupolojaId, int lojaId, int numeroSemana, int ano, IMediator mediator, IMapper mapper)
        {
            // lista de vendas semanais 
            var viewModelList = new List<VendaSemanalListViewModel>().AsQueryable();

            if (mercadoId == 0)
            {
                // nenhum mercado selecionado
                // verificar se empresa selecionada
                if (empresaId == 0)
                {
                    // nenhum mercado nem empresa selecionada => devolver 0.00 ??
                    // ler da db todas as vendas semanais de todos os mercados na semana
                    var responseMercados = await mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = ano, NumeroDaSemana = numeroSemana });
                    if (responseMercados.Succeeded)
                    {
                        viewModelList = mapper.Map<List<VendaSemanalListViewModel>>(responseMercados.Data).AsQueryable();
                        // devolver calculo de todos os mercados
                        return Math.Round(viewModelList.Sum(v => v.ValorTotalDaVenda), 2);
                    }
                    return 0.00; // se as respostas não forem bem sucedidas, devolver 0.00
                }
                // ler da db todas as vendas semanis da empresa
                var responseEmpresa = await mediator.Send(new GetVendasSemanaisByEmpresaIdSemanaQuery() { EmpresaId = empresaId, Ano = ano, NumeroDaSemana = numeroSemana });
                if (responseEmpresa.Succeeded)
                {
                    viewModelList = mapper.Map<List<VendaSemanalListViewModel>>(responseEmpresa.Data).AsQueryable();

                    // verificar se grupolojaId selecionado
                    if (grupolojaId == 0)
                    {
                        // nenhum grupo selecionado => devolver calculo de toda a empresa
                        return Math.Round(viewModelList.Sum(v => v.ValorTotalDaVenda), 2);
                    }
                    else
                    {
                        // grupolojaId selecionado
                        // filtrar todas as vendas semanais do grupoloja
                        viewModelList = viewModelList.Where(v => v.GrupolojaId == grupolojaId).AsQueryable();
                        
                        // verificar se lojaId selecionado
                        if (lojaId == 0)
                        {
                            // nenhuma loja selecionada => devolver calculo de todo o grupoLoja
                            return Math.Round(viewModelList.Sum(v => v.ValorTotalDaVenda), 2);
                        }
                        else
                        {
                            // lojaId selecionada
                            // filtrar todas as vendas semanais da loja
                            viewModelList = viewModelList.Where(v => v.LojaId == lojaId).AsQueryable();

                            // devolver calculo da loja
                            return Math.Round(viewModelList.Sum(v => v.ValorTotalDaVenda), 2);
                        }
                    }
                }
                // se a resposta não foi bem sucedida, devolver 0.00
                return 0.00;
            }
            else
            {
                // mercadoId selecionado
                // ler da db todas as vendas semanis do mercado
                var responseMercado = await mediator.Send(new GetVendasSemanaisByMercadoIdSemanaQuery() { MercadoId = mercadoId, Ano = ano, NumeroDaSemana = numeroSemana });
                if (responseMercado.Succeeded)
                {
                    viewModelList = mapper.Map<List<VendaSemanalListViewModel>>(responseMercado.Data).AsQueryable();
                    
                    // verificar se lojaId selecionado
                    if (lojaId == 0)
                    {
                        // nenhuma loja selecionada => devolver calculo de todo o mercado
                        return Math.Round(viewModelList.Sum(v => v.ValorTotalDaVenda), 2);
                    }
                    else
                    {
                        // lojaId selecionada
                        // filtrar todas as vendas semanais da loja
                        viewModelList = viewModelList.Where(v => v.LojaId == lojaId).AsQueryable();

                        // devolver calculo da loja
                        return Math.Round(viewModelList.Sum(v => v.ValorTotalDaVenda), 2);
                    }
                }
                // se a resposta não foi bem sucedida, devolver 0.00
                return 0.00;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// calcula o objetivo semanal em vendas da loja, grupo, empresa ou mercado
        /// </summary>
        /// <returns>double ObjetivoDaVendaSemanal</returns>

        internal static async Task<double> GetObjetivoDaVendaSemanal(int mercadoId, int empresaId, int grupolojaId, int lojaId, int numeroSemana, int ano, IMediator mediator, IMapper mapper)
        {
            // lista de vendas semanais 
            var viewModelList = new List<VendaSemanalListViewModel>().AsQueryable();

            if (mercadoId == 0)
            {
                // nenhum mercado selecionado
                // verificar se empresa selecionada
                if (empresaId == 0)
                {
                    // nenhum mercado nem empresa selecionada => devolver 0.00 ??
                    // ler da db todas as vendas semanais de todos os mercados na semana
                    var responseMercados = await mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = ano, NumeroDaSemana = numeroSemana });
                    if (responseMercados.Succeeded)
                    {
                        viewModelList = mapper.Map<List<VendaSemanalListViewModel>>(responseMercados.Data).AsQueryable();
                        // devolver calculo de todos os mercados
                        return Math.Round(viewModelList.Sum(v => v.ObjetivoDaVendaSemanal), 2);
                    }
                    return 0.00; // se as respostas não forem bem sucedidas, devolver 0.00



                }
                // ler da db todas as vendas semanis da empresa
                var responseEmpresa = await mediator.Send(new GetVendasSemanaisByEmpresaIdSemanaQuery() { EmpresaId = empresaId, Ano = ano, NumeroDaSemana = numeroSemana });
                if (responseEmpresa.Succeeded)
                {
                    viewModelList = mapper.Map<List<VendaSemanalListViewModel>>(responseEmpresa.Data).AsQueryable();

                    // verificar se grupolojaId selecionado
                    if (grupolojaId == 0)
                    {
                        // nenhum grupo selecionado => devolver calculo do objetivo para toda a empresa
                        return Math.Round(viewModelList.Sum(v => v.ObjetivoDaVendaSemanal), 2);
                    }
                    else
                    {
                        // grupolojaId selecionado
                        // filtrar todas as vendas semanais do grupoloja
                        viewModelList = viewModelList.Where(v => v.GrupolojaId == grupolojaId).AsQueryable();

                        // verificar se lojaId selecionado
                        if (lojaId == 0)
                        {
                            // nenhuma loja selecionada => devolver calculo do objetivo de todo o grupoLoja
                            return Math.Round(viewModelList.Sum(v => v.ObjetivoDaVendaSemanal), 2);
                        }
                        else
                        {
                            // lojaId selecionada
                            // filtrar todas as vendas semanais da loja
                            viewModelList = viewModelList.Where(v => v.LojaId == lojaId).AsQueryable();

                            // devolver calculo do objetivo da loja
                            return Math.Round(viewModelList.Sum(v => v.ObjetivoDaVendaSemanal), 2);
                        }
                    }
                }
                // se a resposta não foi bem sucedida, devolver 0.00
                return 0.00;
            }
            else
            {
                // mercadoId selecionado
                // ler da db todas as vendas semanis do mercado
                var responseMercado = await mediator.Send(new GetVendasSemanaisByMercadoIdSemanaQuery() { MercadoId = mercadoId, Ano = ano, NumeroDaSemana = numeroSemana });
                if (responseMercado.Succeeded)
                {
                    viewModelList = mapper.Map<List<VendaSemanalListViewModel>>(responseMercado.Data).AsQueryable();

                    // verificar se lojaId selecionado
                    if (lojaId == 0)
                    {
                        // nenhuma loja selecionada => devolver calculo do objetivo para todo o mercado
                        return Math.Round(viewModelList.Sum(v => v.ObjetivoDaVendaSemanal), 2);
                    }
                    else
                    {
                        // lojaId selecionada
                        // filtrar todas as vendas semanais da loja
                        viewModelList = viewModelList.Where(v => v.LojaId == lojaId).AsQueryable();

                        // devolver calculo do objetivo da loja
                        return Math.Round(viewModelList.Sum(v => v.ObjetivoDaVendaSemanal), 2);
                    }
                }
                // se a resposta não foi bem sucedida, devolver 0.00
                return 0.00;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// calcula o valor total mensal em vendas da loja, grupo, empresa ou mercado
        /// </summary>
        /// <returns>double ValorTotalMensalDaVenda</returns>

        internal static async Task<double> GetValorTotalMensalDaVenda(int mercadoId, int empresaId, int grupolojaId, int lojaId, int mes, int ano, IMediator mediator, IMapper mapper)
        {
            // lista de vendas mensais 
            var viewModelList = new List<VendaDiariaListViewModel>().AsQueryable();

            if (mercadoId == 0)
            {
                // nenhum mercado selecionado
                // verificar se empresa selecionada
                if (empresaId == 0)
                {
                    // nenhum mercado nem empresa selecionada => devolver 0.00 ??
                    // ler da db todas as vendas diárias de todos os mercados
                    var responseMercados = await mediator.Send(new GetVendasDiariasMesCachedQuery() { Ano = ano, Mes = mes });
                    if (responseMercados.Succeeded)
                    {
                        viewModelList = mapper.Map<List<VendaDiariaListViewModel>>(responseMercados.Data).AsQueryable();
                        // devolver calculo de todos os mercados
                        return Math.Round(viewModelList.Sum(v => v.ValorDaVenda), 2);
                    }
                    return 0.00; // se as respostas não forem bem sucedidas, devolver 0.00
                }
                // ler da db todas as vendas diárias da empresa
                var responseEmpresa = await mediator.Send(new GetVendasDiariasByEmpresaIdMesQuery() { EmpresaId = empresaId, Ano = ano, Mes = mes });
                if (responseEmpresa.Succeeded)
                {
                    viewModelList = mapper.Map<List<VendaDiariaListViewModel>>(responseEmpresa.Data).AsQueryable();

                    // verificar se grupolojaId selecionado
                    if (grupolojaId == 0)
                    {
                        // nenhum grupo selecionado => devolver calculo de toda a empresa
                        return Math.Round(viewModelList.Sum(v => v.ValorDaVenda), 2);
                    }
                    else
                    {
                        // grupolojaId selecionado
                        // ler da db todas as vendas diárias do grupoloja
                        var responseGrupoloja = await mediator.Send(new GetVendasDiariasByGrupolojaIdMesQuery() { GrupolojaId = grupolojaId, Ano = ano, Mes = mes });
                        if (responseGrupoloja.Succeeded)
                        {
                            viewModelList = mapper.Map<List<VendaDiariaListViewModel>>(responseGrupoloja.Data).AsQueryable();

                            // verificar se lojaId selecionado
                            if (lojaId == 0)
                            {
                                // nenhuma loja selecionada => devolver calculo de todo o grupoLoja
                                return Math.Round(viewModelList.Sum(v => v.ValorDaVenda), 2);
                            }
                            else
                            {
                                // lojaId selecionada
                                // filtrar todas as vendas semanais da loja
                                viewModelList = viewModelList.Where(v => v.LojaId == lojaId).AsQueryable();

                                // devolver calculo da loja
                                return Math.Round(viewModelList.Sum(v => v.ValorDaVenda), 2);
                            }
                        }
                    }
                }
                // se as respostas não forem bem sucedidas, devolver 0.00
                return 0.00;
            }
            else
            {
                // mercadoId selecionado
                // ler da db todas as vendas diarias do mercado
                var responseMercado = await mediator.Send(new GetVendasDiariasByMercadoIdMesQuery() { MercadoId = mercadoId, Ano = ano, Mes = mes });
                if (responseMercado.Succeeded)
                {
                    viewModelList = mapper.Map<List<VendaDiariaListViewModel>>(responseMercado.Data).AsQueryable();

                    // verificar se lojaId selecionado
                    if (lojaId == 0)
                    {
                        // nenhuma loja selecionada => devolver calculo de todo o mercado
                        return Math.Round(viewModelList.Sum(v => v.ValorDaVenda), 2);
                    }
                    else
                    {
                        // lojaId selecionada
                        // filtrar todas as vendas semanais da loja
                        viewModelList = viewModelList.Where(v => v.LojaId == lojaId).AsQueryable();

                        // devolver calculo da loja
                        return Math.Round(viewModelList.Sum(v => v.ValorDaVenda), 2);
                    }
                }
                // se a resposta não foi bem sucedida, devolver 0.00
                return 0.00;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// calcula o objetivo mensal em vendas da loja, grupo, empresa ou mercado
        /// o objetivo mensal é calculado como o objetivo mensal do ano anterior
        /// acrescido de 10%
        /// </summary>
        /// <returns>double ObjetivoMensalDaVenda</returns>

        internal static async Task<double> GetObjetivoMensalDaVenda(int mercadoId, int empresaId, int grupolojaId, int lojaId, int mes, int ano, IMediator mediator, IMapper mapper)
        {
            // lista de vendas mensais do ano anterior
            var viewModelList = new List<VendaDiariaListViewModel>().AsQueryable();

            if (mercadoId == 0)
            {
                // nenhum mercado selecionado
                // verificar se empresa selecionada
                if (empresaId == 0)
                {
                    // nenhum mercado nem empresa selecionada => devolver 0.00 ??
                    // ler da db todas as vendas diárias de todos os mercados no ano anterior
                    var responseMercados = await mediator.Send(new GetVendasDiariasMesCachedQuery() { Ano = ano - 1, Mes = mes });
                    if (responseMercados.Succeeded)
                    {
                        viewModelList = mapper.Map<List<VendaDiariaListViewModel>>(responseMercados.Data).AsQueryable();
                        // devolver calculo do objetivo para todos os mercados
                        var totalVenda = viewModelList.Sum(v => v.ValorDaVenda);
                        return Math.Round(totalVenda * 1.10, 2); // acrescido de 10%
                    }
                    return 0.00; // se as respostas não forem bem sucedidas, devolver 0.00
                }
                // ler da db todas as vendas diarias da empresa no ano anterior
                var responseEmpresa = await mediator.Send(new GetVendasDiariasByEmpresaIdMesQuery() { EmpresaId = empresaId, Ano = ano - 1, Mes = mes });
                if (responseEmpresa.Succeeded)
                {
                    viewModelList = mapper.Map<List<VendaDiariaListViewModel>>(responseEmpresa.Data).AsQueryable();

                    // verificar se grupolojaId selecionado
                    if (grupolojaId == 0)
                    {
                        // nenhum grupo selecionado => devolver calculo do objetivo para toda a empresa
                        var totalVenda = viewModelList.Sum(v => v.ValorDaVenda);
                        return Math.Round(totalVenda * 1.10, 2); // acrescido de 10%
                    }
                    else
                    {
                        // grupolojaId selecionado
                        // ler da db todas as vendas diárias do grupoloja
                        var responseGrupoloja = await mediator.Send(new GetVendasDiariasByGrupolojaIdMesQuery() { GrupolojaId = grupolojaId, Ano = ano - 1, Mes = mes });
                        if (responseGrupoloja.Succeeded)
                        {
                            viewModelList = mapper.Map<List<VendaDiariaListViewModel>>(responseGrupoloja.Data).AsQueryable();

                            // verificar se lojaId selecionado
                            if (lojaId == 0)
                            {
                                // nenhuma loja selecionada => devolver calculo do objetivo de todo o grupoLoja
                                var totalVenda = viewModelList.Sum(v => v.ValorDaVenda);
                                return Math.Round(totalVenda * 1.10, 2); // acrescido de 10%
                            }
                            else
                            {
                                // lojaId selecionada
                                // filtrar todas as vendas diarias da loja
                                viewModelList = viewModelList.Where(v => v.LojaId == lojaId).AsQueryable();

                                // devolver calculo do objetivo da loja
                                var totalVenda = viewModelList.Sum(v => v.ValorDaVenda);
                                return Math.Round(totalVenda * 1.10, 2); // acrescido de 10%
                            }
                        }
                    }
                }
                // se a resposta não foi bem sucedida, devolver 0.00
                return 0.00;
            }
            else
            {
                // mercadoId selecionado
                // ler da db todas as vendas diarias do mercado no ano anterior
                var responseMercado = await mediator.Send(new GetVendasDiariasByMercadoIdMesQuery() { MercadoId = mercadoId, Ano = ano - 1, Mes = mes });
                if (responseMercado.Succeeded)
                {
                    viewModelList = mapper.Map<List<VendaDiariaListViewModel>>(responseMercado.Data).AsQueryable();

                    // verificar se lojaId selecionado
                    if (lojaId == 0)
                    {
                        // nenhuma loja selecionada => devolver calculo do objetivo para todo o mercado
                        var totalVenda = viewModelList.Sum(v => v.ValorDaVenda);
                        return Math.Round(totalVenda * 1.10, 2); // acrescido de 10%
                    }
                    else
                    {
                        // lojaId selecionada
                        // filtrar todas as vendas semanais da loja
                        viewModelList = viewModelList.Where(v => v.LojaId == lojaId).AsQueryable();

                        // devolver calculo do objetivo da loja
                        var totalVenda = viewModelList.Sum(v => v.ValorDaVenda);
                        return Math.Round(totalVenda * 1.10, 2); // acrescido de 10%
                    }
                }
                // se a resposta não foi bem sucedida, devolver 0.00
                return 0.00;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma VendaSemanalViewModel a partir de uma VendaDiariaExcelModel.
        /// A VendaSemanalViewModel é necessária para conter a venda diária lida do excel.
        /// </summary>
        /// <returns>
        /// retorna a VendaDiariaViewModel necessária para conter a venda diária lida do excel
        /// </returns>

        internal async Task<VendaSemanalViewModel> GetVendaSemanalFromExcelAsync(VendaDiariaExcelModel venda)
        {
            try
            {
                // validar loja
                var response_loja = await _mediator.Send(new GetLojaByIdQuery() { Id = venda.LojaId });
                if (!response_loja.Succeeded || response_loja.Data == null) return null;

                // verificar transição de ano
                var yearVendaSemanal = venda.Ano;
                if (venda.NumeroDaSemana == 1 && venda.Mês > 1)
                {
                    //semana 1 com venda do ano anterior
                    yearVendaSemanal = yearVendaSemanal + 1;
                }
                if ((venda.NumeroDaSemana > 51) && (venda.Mês == 1))
                {

                    // VendaSemanal pertence à semana 52 ou 53 do ano anterior
                    yearVendaSemanal = yearVendaSemanal - 1;
                }

                // calcular variação anual do objetivo
                Double AnualGoalVariation = 0;
                var response_prevYear = await _mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = venda.LojaId, Ano = yearVendaSemanal - 1, NumeroDaSemana = venda.NumeroDaSemana});
                if (response_prevYear.Succeeded)
                {
                    if (response_prevYear.Data != null && response_prevYear.Data.ValorTotalDaVenda > 0)
                    {
                        AnualGoalVariation = (venda.ObjetivoDaVendaSemanal - response_prevYear.Data.ValorTotalDaVenda) / response_prevYear.Data.ValorTotalDaVenda;
                    }
                }

                var vendaSemanal = new VendaSemanalViewModel
                {
                    DataInicialDaSemana = venda.DataDaVenda.MondayOfWeek(),
                    DataFinalDaSemana = venda.DataDaVenda.MondayOfWeek().AddDays(6),
                    ValorTotalDaVenda = 0,
                    ObjetivoDaVendaSemanal = venda.ObjetivoDaVendaSemanal,
                    VariaçaoAnual = AnualGoalVariation,
                    Ano = yearVendaSemanal,
                    Quarter = venda.Quarter,
                    Mes = venda.DataDaVenda.Month,
                    NumeroDaSemana = venda.NumeroDaSemana,
                    MercadoId = (int)response_loja.Data.MercadoId,
                    EmpresaId = await EmpresaController.GetEmpresaIdAsync(response_loja.Data.Id, _mapper, _mediator),
                    GrupolojaId = response_loja.Data.GrupolojaId,
                    LojaId = venda.LojaId
                };


                // atualizar ObjetivoDaVendaSemanal da venda semanal
                if (venda.ObjetivoDaVendaSemanal <= 0)
                {
                    var responseVendaAnoAnterior = await _mediator.Send(new GetVendaSemanalByLojaIdSemanaQuery() { LojaId = vendaSemanal.LojaId, Ano = vendaSemanal.Ano - 1, NumeroDaSemana = vendaSemanal.NumeroDaSemana });
                    if (responseVendaAnoAnterior.Succeeded && responseVendaAnoAnterior.Data != null)
                    {
                        var Vsvaa = _mapper.Map<VendaSemanalViewModel>(responseVendaAnoAnterior.Data);
                        vendaSemanal.ValorTotalDaVendaDoAnoAnterior = Vsvaa.ValorTotalDaVenda;
                        vendaSemanal.ObjetivoDaVendaSemanal = Vsvaa.ValorTotalDaVenda * 1.10;
                    }
                    else
                    {
                        vendaSemanal.ValorTotalDaVendaDoAnoAnterior = 0.00;
                    }
                }

                return vendaSemanal;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - GetVendaSemanalFromExcelAsync - Exception: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma VendaSemanalViewModel vazia.
        /// A VendaSemanalViewModel vai ser editada no lado do cliente.
        /// </summary>
        /// <returns>retorna a VendaDiariaViewModel</returns>

        internal static async Task<VendaSemanalViewModel> GetVendaSemanalEmptyAsync(IMapper mapper, IMediator mediator)
        {
            var cal = new CultureInfo("pt-PT").Calendar;
            var proxSemanaDataInicial = DateTime.Now.AddDays(7).MondayOfWeek();

            var venda = new VendaSemanalViewModel
            {
                DataInicialDaSemana = proxSemanaDataInicial,
                DataFinalDaSemana = proxSemanaDataInicial.AddDays(6),
                Valor2Feira = 0,
                TotalArtigos2Feira = 0,
                Conv2Feira = 0,
                Weather2Feira = Weather.Undefined,
                Observacoes2Feira = string.Empty,
                Valor3Feira = 0,
                TotalArtigos3Feira = 0,
                Conv3Feira = 0,
                Weather3Feira = Weather.Undefined,
                Observacoes3Feira = string.Empty,
                Valor4Feira = 0,
                TotalArtigos4Feira = 0,
                Conv4Feira = 0,
                Weather4Feira = Weather.Undefined,
                Observacoes4Feira = string.Empty,
                Valor5Feira = 0,
                TotalArtigos5Feira = 0,
                Conv5Feira = 0,
                Weather5Feira = Weather.Undefined,
                Observacoes5Feira = string.Empty,
                Valor6Feira = 0,
                TotalArtigos6Feira = 0,
                Conv6Feira = 0,
                Weather6Feira = Weather.Undefined,
                Observacoes6Feira = string.Empty,
                ValorSábado = 0,
                TotalArtigosSab = 0,
                ConvSab = 0,
                WeatherSab = Weather.Undefined,
                ObservacoesSab = string.Empty,
                ValorDomingo = 0,
                TotalArtigosDom = 0,
                ConvDom = 0,
                WeatherDom = Weather.Undefined,
                ObservacoesDom = string.Empty,
                ValorTotalDaVenda = 0,
                ObjetivoDaVendaSemanal = 0,
                VariaçaoAnual = 10,
                Ano = proxSemanaDataInicial.Year,
                Quarter = (proxSemanaDataInicial.Month + 2) / 3,
                Mes = proxSemanaDataInicial.Month,
                NumeroDaSemana = cal.GetWeekOfYear(proxSemanaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                MercadoId = 0,
                Mercados = await MercadoController.GetSelectListAllMercadosAsync(0, mapper, mediator),
                EmpresaId = 0,
                Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, mapper, mediator),
                GrupolojaId = 0,
                LojaId = 0
            };

            // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
            if ((venda.NumeroDaSemana == 1) && (venda.Mes == 12))
            {
                // VendaSemanal pertence à semana 1 do ano seguinte
                venda.Ano = venda.Ano + 1;
            }

            return venda;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma VendaSemanalViewModel vazia com a data do ano anterior.
        /// A VendaSemanalViewModel vai ser editada no lado do cliente.
        /// </summary>
        /// <returns>retorna a VendaDiariaViewModel</returns>

        internal static async Task<VendaSemanalViewModel> GetVendaSemanalEmptyAnoAnteriorAsync(DateTime semanaDataInicial, IMapper mapper, IMediator mediator)
        {
            var cal = new CultureInfo("pt-PT").Calendar;
            //var proxSemanaDataInicial = DateTime.Now.AddDays(7).MondayOfWeek();
            var numeroDaSemana = cal.GetWeekOfYear(semanaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var ano = semanaDataInicial.Year;
            // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
            if ((numeroDaSemana == 1) && (semanaDataInicial.Month == 12))
            {
                // data pertence à semana 1 do ano seguinte
                ano = ano + 1;
            }

            var dataAnoAnterior = FirstDateOfWeek(ano - 1, numeroDaSemana);

            var venda = new VendaSemanalViewModel
            {
                DataInicialDaSemana = dataAnoAnterior,
                DataFinalDaSemana = dataAnoAnterior.AddDays(6),
                Valor2Feira = 0,
                TotalArtigos2Feira = 0,
                Weather2Feira = Weather.Undefined,
                Observacoes2Feira = string.Empty,
                Valor3Feira = 0,
                TotalArtigos3Feira = 0,
                Weather3Feira = Weather.Undefined,
                Observacoes3Feira = string.Empty,
                Valor4Feira = 0,
                TotalArtigos4Feira = 0,
                Weather4Feira = Weather.Undefined,
                Observacoes4Feira = string.Empty,
                Valor5Feira = 0,
                TotalArtigos5Feira = 0,
                Weather5Feira = Weather.Undefined,
                Observacoes5Feira = string.Empty,
                Valor6Feira = 0,
                TotalArtigos6Feira = 0,
                Weather6Feira = Weather.Undefined,
                Observacoes6Feira = string.Empty,
                ValorSábado = 0,
                TotalArtigosSab = 0,
                WeatherSab = Weather.Undefined,
                ObservacoesSab = string.Empty,
                ValorDomingo = 0,
                TotalArtigosDom = 0,
                WeatherDom = Weather.Undefined,
                ObservacoesDom = string.Empty,
                ValorTotalDaVenda = 0,
                ObjetivoDaVendaSemanal = 0,
                VariaçaoAnual = 10,
                Ano = dataAnoAnterior.Year,
                Quarter = (dataAnoAnterior.Month + 2) / 3,
                Mes = dataAnoAnterior.Month,
                NumeroDaSemana = numeroDaSemana,
                MercadoId = 0,
                Mercados = await MercadoController.GetSelectListAllMercadosAsync(0, mapper, mediator),
                EmpresaId = 0,
                Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, mapper, mediator),
                GrupolojaId = 0,
                LojaId = 0
            };

            // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
            if ((venda.NumeroDaSemana == 1) && (venda.Mes == 12))
            {
                // VendaSemanal pertence à semana 1 do ano seguinte
                venda.Ano = venda.Ano + 1;
            }

            return venda;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma VendaSemanalViewModel vazia para a semana defenida nos parametros.
        /// A VendaSemanalViewModel vai ser editada no lado do cliente.
        /// </summary>
        /// <returns>retorna a VendaDiariaViewModel</returns>

        internal static async Task<VendaSemanalViewModel> GetVendaSemanalEmptyAsync(int ano, int semana, IMapper mapper, IMediator mediator)
        {
            var cal = new CultureInfo("pt-PT").Calendar;
            var semanaDataInicial = DateTime.Now.AddDays(7).MondayOfWeek();

            if (ano > 2000 && semana > 0 && semana <= 53) semanaDataInicial = FirstDateOfWeek(ano, semana);

            var venda = new VendaSemanalViewModel
            {
                DataInicialDaSemana = semanaDataInicial,
                DataFinalDaSemana = semanaDataInicial.AddDays(6),
                Valor2Feira = 0,
                TotalArtigos2Feira = 0,
                Conv2Feira = 0,
                Valor3Feira = 0,
                TotalArtigos3Feira = 0,
                Conv3Feira = 0,
                Valor4Feira = 0,
                TotalArtigos4Feira = 0,
                Conv4Feira = 0,
                Valor5Feira = 0,
                TotalArtigos5Feira = 0,
                Conv5Feira = 0,
                Valor6Feira = 0,
                TotalArtigos6Feira = 0,
                Conv6Feira = 0,
                ValorSábado = 0,
                TotalArtigosSab = 0,
                ConvSab = 0,
                ValorDomingo = 0,
                TotalArtigosDom = 0,
                ConvDom = 0,
                ValorTotalDaVenda = 0,
                ObjetivoDaVendaSemanal = 0,
                VariaçaoAnual = 10,
                Ano = semanaDataInicial.Year,
                Quarter = (semanaDataInicial.Month + 2) / 3,
                Mes = semanaDataInicial.Month,
                NumeroDaSemana = cal.GetWeekOfYear(semanaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                MercadoId = 0,
                Mercados = await MercadoController.GetSelectListAllMercadosAsync(0, mapper, mediator),
                EmpresaId = 0,
                Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, mapper, mediator),
                GrupolojaId = 0,
                LojaId = 0
            };

            // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
            if ((venda.NumeroDaSemana == 1) && (venda.Mes == 12))
            {
                // VendaSemanal pertence à semana 1 do ano seguinte
                venda.Ano = venda.Ano + 1;
            }

            return venda;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma VendaDiariaExcelModel lida de uma linha do ficheiro excel.
        /// </summary>
        /// <returns>true se sucesso</returns>

        internal bool UpdateVendaSemanalWithDiárias(ref VendaSemanalViewModel venda, List<VendaDiariaViewModel> vdList)
        {
            var success = false;
            try
            {
                // atualizar vendas diárias
                for (var i = 1; i < 8; i++)
                {
                    var vd = vdList.Find(v => v.DiaDaSemana == i);
                    var valorDaVenda = vd == null ? 0.0 : vd.ValorDaVenda;

                    switch (i)
                    {
                        case 1:
                            venda.Valor2Feira = valorDaVenda;
                            break;
                        case 2:
                            venda.Valor3Feira = valorDaVenda;
                            break;
                        case 3:
                            venda.Valor4Feira = valorDaVenda;
                            break;
                        case 4:
                            venda.Valor5Feira = valorDaVenda;
                            break;
                        case 5:
                            venda.Valor6Feira = valorDaVenda;
                            break;
                        case 6:
                            venda.ValorSábado = valorDaVenda;
                            break;
                        case 7:
                            venda.ValorDomingo = valorDaVenda;
                            break;
                    }
                }
                // atualizar ValorTotalDaVenda da venda semanal
                venda.ValorTotalDaVenda = vdList.Sum(v => v.ValorDaVenda);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendaSemanalWithDiáriasAsync - Exception: " + ex.Message);
                return success;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve uma VendaDiariaExcelModel lida de uma linha do ficheiro excel.
        /// </summary>
        /// <returns>
        /// retorna a VendaDiariaExcelModel necessária para conter a venda diária lida
        /// </returns>

        internal VendaDiariaExcelModel GetVendaDiariaFromExcelRow(IXLRow row)
        {
            try
            {
                var DataDaVenda = new DateTime();
                var dateParseResult = DateTime.TryParse(row.Cell(DATA_COLUMN).CachedValue.ToString().Trim(), out DataDaVenda);

                var NumeroDaSemana = 0;
                var semanaParseResult = int.TryParse(row.Cell(SEMANA_COLUMN).CachedValue.ToString().Trim(), out NumeroDaSemana);

                var LojaId = 0;
                var lojaIdParseResult = int.TryParse(row.Cell(LOJA_COLUMN).CachedValue.ToString().Trim(), out LojaId);

                var Qntpecas = 0;
                var QntpecasParseResult = int.TryParse(row.Cell(QNTPECAS_COLUMN).CachedValue.ToString().Trim(), out Qntpecas);

                var ValorDaVenda = 0.0;
                var valorDaVendaParseResult = double.TryParse(row.Cell(VALOR_COLUMN).CachedValue.ToString().Trim(), out ValorDaVenda);
                ValorDaVenda = Math.Round(ValorDaVenda, 2, MidpointRounding.AwayFromZero);

                var PercentConv = 0.00;
                var PercentConvParseResult = double.TryParse(row.Cell(CONV_COLUMN).CachedValue.ToString().Trim(), out PercentConv);
                PercentConv = Math.Round(PercentConv, 2, MidpointRounding.AwayFromZero);

                var Quarter = 0;
                var quarterParseResult = int.TryParse(row.Cell(QUARTER_COLUMN).CachedValue.ToString().Trim(), out Quarter);

                var ObjetivoDaVendaSemanal = 0.0;
                var cellText = row.Cell(OBJETIVO_COLUMN).CachedValue.ToString();
                var objetivoParseResult = cellText.IsNullOrEmpty() ? true : double.TryParse(cellText.Trim(), out ObjetivoDaVendaSemanal);
                ObjetivoDaVendaSemanal = Math.Round(ObjetivoDaVendaSemanal, 2, MidpointRounding.AwayFromZero);

                if (!dateParseResult || !semanaParseResult || !lojaIdParseResult || !QntpecasParseResult || !valorDaVendaParseResult || !PercentConvParseResult || !quarterParseResult || !objetivoParseResult)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - GetDailySaleFromExcelRow - Error: Invalid date in row " + row.RowNumber());
                    return null;
                }

                var cal = new CultureInfo("pt-PT").Calendar;
                var venda = new VendaDiariaExcelModel
                {
                    DataDaVenda = DataDaVenda,
                    LojaId = LojaId,
                    Ano = DataDaVenda.Year,
                    Mês = DataDaVenda.Month,
                    DiaDoMês = DataDaVenda.Day,
                    DiaDaSemana = DataDaVenda.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DataDaVenda.DayOfWeek,
                    NumeroDaSemana = NumeroDaSemana,
                    TotalArtigos = Qntpecas,
                    PercentConv = PercentConv,
                    ValorDaVenda = ValorDaVenda,
                    Quarter = Quarter,
                    ObjetivoDaVendaSemanal = ObjetivoDaVendaSemanal,
                    VendaSemanalId = 0
                };

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - GetDailySaleFromExcelRow - Excel Data is valid in row " + row.RowNumber());
                return venda;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - GetDailySaleFromExcelRow - Exception: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Converte VendaDiariaExcelModel em VendaDiariaViewModel
        /// </summary>
        /// <returns>
        /// retorna a VendaDiariaViewModel necessária para conter a venda diária lida do excel
        /// </returns>

        internal VendaDiariaViewModel GetVendaDiariaFromExcelAsync(VendaDiariaExcelModel venda)
        {
            try
            {
                //var cal = new CultureInfo("pt-PT").Calendar;
                var vendaDiaria = new VendaDiariaViewModel
                {
                    DataDaVenda = venda.DataDaVenda,
                    Ano = venda.DataDaVenda.Year,
                    Mês = venda.DataDaVenda.Month,
                    DiaDoMês = venda.DataDaVenda.Day,
                    DiaDaSemana = venda.DataDaVenda.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)venda.DataDaVenda.DayOfWeek,
                    ValorDaVenda = venda.ValorDaVenda,
                    LojaId = venda.LojaId,
                    GrupolojaId = venda.GrupolojaId,
                    EmpresaId = venda.EmpresaId,
                    MercadoId = venda.MercadoId,
                    VendaSemanalId = 0,
                    PercentConv = venda.PercentConv,
                    TotalArtigos = venda.TotalArtigos,
                    Weather = 0,
                    Observacoes = string.Empty
                };
                return vendaDiaria;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - GetDailySaleFromExcelRow - Exception: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Atualiza as Vendas Diárias de uma Venda Semanal.
        /// As vendas diárias são criadas ou atualizadas com base nos valores
        /// existentes na Venda Semanal.
        /// </summary>
        /// <returns>true se sucesso</returns>

        internal static async Task<bool> UpdateVendasDiáriasFromSemanalAsync(VendaSemanalViewModel venda, IMapper mapper, IMediator mediator)
        {
            try
            {
                var vendasDiariasResponse = await mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = venda.Id });
                var vdiariaList= mapper.Map<List<VendaDiariaViewModel>>(vendasDiariasResponse.Data);
                // atualizar vendas diárias
                //_logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiáriasFromSemanalAsync - venda = " + venda.Id);

                // criar lista de vendas diárias para gravar na base de dados
                var vdList = new List<VendaDiariaViewModel>();
                for (var i = 1; i < 8; i++)
                {
                    var dbVendaDiaria = vdiariaList.Find(v => v.DiaDaSemana == i);
                    var valorDaVenda = GetValorVendaDiária(venda, i);
                    var percentConvDaVenda = GetPercentConvVendaDiária(venda, i);
                    var totalArtigosDaVenda = GetTotalArtigosVendaDiária(venda, i);
                    var weatherDaVenda = GetWeatherVendaDiária(venda, i);
                    var obsDaVenda = GetObsVendaDiária(venda, i);
                    var dataDaVenda = venda.DataInicialDaSemana.AddDays(i - 1);
                    var vd = new VendaDiariaViewModel
                    {
                        Id = dbVendaDiaria == null ? 0 : dbVendaDiaria.Id,
                        DiaDaSemana = i,
                        DataDaVenda = dataDaVenda,
                        Ano = dataDaVenda.Year,
                        Mês = dataDaVenda.Month,
                        DiaDoMês = dataDaVenda.Day,
                        ValorDaVenda = valorDaVenda,
                        PercentConv = percentConvDaVenda,
                        TotalArtigos = totalArtigosDaVenda,
                        Weather = weatherDaVenda,
                        Observacoes = obsDaVenda,
                        VendaSemanalId = venda.Id,
                        LojaId = venda.LojaId,
                        GrupolojaId = venda.GrupolojaId,
                        EmpresaId = venda.EmpresaId,
                        MercadoId = venda.MercadoId
                    };
                    vdList.Add(vd);
                }

                //_logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiáriasFromSemanalAsync - Lista de vendas diárias criadas = " + vdList.Count);

                // gravar lista na base de dados
                foreach (var vd in vdList)
                {
                    // atualizar ou criar nova Venda diária
                    if(vd.Id > 0)
                    {
                        //_logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiáriasFromSemanalAsync - Venda diária vai ser atualizada id = " + vd.Id);
                        //atualizar vd na base de dados
                        var updateVendaCommand = mapper.Map<UpdateVendaDiariaCommand>(vd);
                        var resultUVD = await mediator.Send(updateVendaCommand);
                        if (resultUVD.Succeeded)
                        {
                            //_notify.Information($"{_localizer["Venda diária com ID"]} {resultUVD.Data} {_localizer["atualizada."]}");
                            //_logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiáriasFromSemanalAsync - Venda diária atualizada id = " + vd.Id);
                        }
                        else
                        {
                            //_notify.Error(resultUVD.Message);
                            //_logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiáriasFromSemanalAsync - Erro : Venda diária não atualizada id = " + vd.Id);
                        }
                    }
                    else
                    {
                        // criar vd na base de dados
                        var createVendaCommand = mapper.Map<CreateVendaDiariaCommand>(vd);
                        var resultCVD = await mediator.Send(createVendaCommand);
                        if (resultCVD.Succeeded)
                        {
                            //_notify.Information($"{_localizer["Nova Venda diária com ID"]} {resultCVD.Data} {_localizer["inserida."]}");
                            //_logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiáriasFromSemanalAsync - Venda diária inserida id = " + resultCVD.Data);
                        }
                        else
                        {
                            //_notify.Error(resultCVD.Message);
                            //_logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiáriasFromSemanalAsync - Erro : Venda diária não inserida id = " + resultCVD.Data);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //_logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiáriasAsync - Exception: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve o valor da venda diária existente em vs,
        /// que corresponde ao dia da semana passado em diaDaSemana.
        /// </summary>
        /// <returns>double Valor da Venda Diária</returns>

        internal static double GetValorVendaDiária(VendaSemanalViewModel vs, int diaDaSemana)
        {
            switch (diaDaSemana)
            {
                case 1:
                    return vs.Valor2Feira;
                case 2:
                    return vs.Valor3Feira;
                case 3:
                    return vs.Valor4Feira;
                case 4:
                    return vs.Valor5Feira;
                case 5:
                    return vs.Valor6Feira;
                case 6:
                    return vs.ValorSábado;
                case 7:
                    return vs.ValorDomingo;
                default:
                    return 0.0;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve o valor da venda diária existente em vs,
        /// que corresponde ao dia da semana passado em diaDaSemana.
        /// </summary>
        /// <returns>double Valor da Venda Diária</returns>

        internal static int GetTotalArtigosVendaDiária(VendaSemanalViewModel vs, int diaDaSemana)
        {
            switch (diaDaSemana)
            {
                case 1:
                    return vs.TotalArtigos2Feira;
                case 2:
                    return vs.TotalArtigos3Feira;
                case 3:
                    return vs.TotalArtigos4Feira;
                case 4:
                    return vs.TotalArtigos5Feira;
                case 5:
                    return vs.TotalArtigos6Feira;
                case 6:
                    return vs.TotalArtigosSab;
                case 7:
                    return vs.TotalArtigosDom;
                default:
                    return 0;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve o valor da conv existente em vs,
        /// que corresponde ao dia da semana passado em diaDaSemana.
        /// </summary>
        /// <returns>double Valor da Venda Diária</returns>

        internal static double GetPercentConvVendaDiária(VendaSemanalViewModel vs, int diaDaSemana)
        {
            switch (diaDaSemana)
            {
                case 1:
                    return vs.Conv2Feira;
                case 2:
                    return vs.Conv3Feira;
                case 3:
                    return vs.Conv4Feira;
                case 4:
                    return vs.Conv5Feira;
                case 5:
                    return vs.Conv6Feira;
                case 6:
                    return vs.ConvSab;
                case 7:
                    return vs.ConvDom;
                default:
                    return 0;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve as condiçoes atmosféricas da venda diária existente em vs,
        /// que corresponde ao dia da semana passado em diaDaSemana.
        /// </summary>
        /// <returns>Weather da Venda Diária</returns>

        internal static Weather GetWeatherVendaDiária(VendaSemanalViewModel vs, int diaDaSemana)
        {
            switch (diaDaSemana)
            {
                case 1:
                    return vs.Weather2Feira;
                case 2:
                    return vs.Weather3Feira;
                case 3:
                    return vs.Weather4Feira;
                case 4:
                    return vs.Weather5Feira;
                case 5:
                    return vs.Weather6Feira;
                case 6:
                    return vs.WeatherSab;
                case 7:
                    return vs.WeatherDom;
                default:
                    return Weather.Undefined;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Devolve as observações da venda diária existente em vs,
        /// que corresponde ao dia da semana passado em diaDaSemana.
        /// </summary>
        /// <returns>string com as observações da Venda Diária</returns>

        internal static string GetObsVendaDiária(VendaSemanalViewModel vs, int diaDaSemana)
        {
            switch (diaDaSemana)
            {
                case 1:
                    return vs.Observacoes2Feira;
                case 2:
                    return vs.Observacoes3Feira;
                case 3:
                    return vs.Observacoes4Feira;
                case 4:
                    return vs.Observacoes5Feira;
                case 5:
                    return vs.Observacoes6Feira;
                case 6:
                    return vs.ObservacoesSab;
                case 7:
                    return vs.ObservacoesDom;
                default:
                    return string.Empty;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task StartProgressBarAsync(string fileName, IHubContext<ProgressHub> hubContext)
        {
            var startMessage = _localizer["Ficheiro:"] + fileName + _localizer["em processamento ..."];
            await ProgressBar.StartProgressBarAsync(startMessage, hubContext);
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task UpdateProgressBarAsync(int idx, int total, IHubContext<ProgressHub> hubContext)
        {
            var percentage = total == 0 ? 0 : idx * 100 / total;
            var progressMessage = _localizer["Venda #"] + idx + " " + _localizer["de"] + " " + total + " " + _localizer["carregada com sucesso."];
            await ProgressBar.SetProgressBarAsync(progressMessage, percentage, hubContext);
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task StopProgressBarAsync(int totalFiles, IHubContext<ProgressHub> hubContext)
        {
            var endMessage = totalFiles + _localizer["ficheiro(s) Excel carregado(s) com sucesso."];
            await ProgressBar.StopProgressBarAsync(endMessage, hubContext);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Calcula a data do primeiro dia da semana passada em weekOfYear.
        /// </summary>
        /// <returns>Data da 2ª feira da semana defenida em weekOfYear</returns>

        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            // Use first Thursday in January to get first week of the year as
            // it will never be in Week 52/53
            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            // As we're adding days to a date in Week 1,
            // we need to subtract 1 in order to get the right date for week #1
            if (firstWeek == 1)
            {
                weekNum -= 1;
            }

            // Using the first Thursday as starting week ensures that we are starting in the right year
            // then we add number of weeks multiplied with days
            var result = firstThursday.AddDays(weekNum * 7);

            // Subtract 3 days from Thursday to get Monday, which is the first weekday in ISO8601
            return result.AddDays(-3);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Calcula o numero da semana de uma data.
        /// </summary>
        /// <returns>NumeroDaSemana</returns>

        public static int GetNumeroDaSemana(int ano = 0, int mes = 0, int dia = 0)
        {
            var cal = new CultureInfo("pt-PT").Calendar;
            var weekNumber = 0;

            if (ano < 2000 || ano > 2100 || mes < 1 || mes > 12 || dia < 1 || dia > 31)
            {
                return weekNumber; // retorna 0 se os parametros não forem válidos
            }

            var SemanaData = new DateTime(ano, mes, dia);
            weekNumber = cal.GetWeekOfYear(SemanaData, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return weekNumber;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Vendas.Edit)]
        public async Task<JsonResult> UpdateVendasDiarias()
        {
            try
            {
                // ler da db todas as vendas semanAis
                var response = await _mediator.Send(new GetAllVendasSemanaisCachedQuery() { });
                if (!response.Succeeded) return Json(new { status = "fail" });

                var vsList = _mapper.Map<List<VendaSemanalListViewModel>>(response.Data).AsQueryable();
                foreach (var vs in vsList)
                {
                    // ler as vendas diárias da venda semanal
                    var responseVendasDiarias = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vs.Id });
                    if (!responseVendasDiarias.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiarias - Erro ao ler Venda semanal " + vs.Id + ": " + responseVendasDiarias.Message);
                        continue;
                    }

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiarias - Atualização da Venda semanal " + vs.Id + " em curso.");
                    var vdList = _mapper.Map<List<VendaDiariaViewModel>>(responseVendasDiarias.Data);
                    
                    // atualizar vendas diárias com base na venda semanal
                    foreach(var vd in vdList)
                    {
                        // atualizar mercado/empresa/grupoloja/loja
                        vd.MercadoId = vs.MercadoId;
                        vd.EmpresaId = vs.EmpresaId;
                        vd.GrupolojaId = vs.GrupolojaId;
                        vd.LojaId = vs.LojaId;

                        // atualizar vd na base de dados
                        var updateVendaCommand = _mapper.Map<UpdateVendaDiariaCommand>(vd);
                        var resultUVD = await _mediator.Send(updateVendaCommand);
                        if (!resultUVD.Succeeded)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiarias - Erro ao atualizar Venda diária id = " + vd.Id);
                        }
                    }
                }

                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiarias - Exception vai sair e retornar Error: " + ex.Message);
                return Json(new { status = "fail" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Vendas.Edit)]
        public async Task<JsonResult> UpdateVendasDiarias2()
        {
            try
            {
                // ler da db todas as vendas semanAis
                var response = await _mediator.Send(new GetAllVendasDiariasCachedQuery() { });
                if (!response.Succeeded) return Json(new { status = "fail" });

                var vdList = _mapper.Map<List<VendaDiariaListViewModel>>(response.Data).AsQueryable();
                vdList = vdList.Where(vd => (vd.MercadoId == 1) && (vd.EmpresaId == 1) && (vd.GrupolojaId == 1));
                
                foreach (var vd in vdList)
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiarias2 - Atualização da Venda diária " + vd.Id + " em curso.");

                    // ler a venda semanal à qual pertence a venda diária
                    var responseVendaSemanal = await _mediator.Send(new GetVendaSemanalByIdQuery() { Id = vd.VendaSemanalId });
                    if (!responseVendaSemanal.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiarias2 - Erro ao ler Venda semanal " + vd.Id + ": " + responseVendaSemanal.Message);
                        continue;
                    }

                    // atualizar venda diária com base na venda semanal
                    // atualizar mercado/empresa/grupoloja/loja
                    vd.MercadoId = responseVendaSemanal.Data.MercadoId;
                    vd.EmpresaId = responseVendaSemanal.Data.EmpresaId;
                    vd.GrupolojaId = responseVendaSemanal.Data.GrupolojaId;
                    vd.LojaId = responseVendaSemanal.Data.LojaId;

                    // atualizar vd na base de dados
                    var updateVendaCommand = _mapper.Map<UpdateVendaDiariaCommand>(vd);
                    var resultUVD = await _mediator.Send(updateVendaCommand);
                    if (!resultUVD.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiarias2 - Erro ao atualizar Venda diária id = " + vd.Id);
                    }

                }

                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - UpdateVendasDiarias - Exception vai sair e retornar Error: " + ex.Message);
                return Json(new { status = "fail" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria um CurrentRole para o utilizador corrente "User"
        /// </summary>
        /// <param></param>
        /// <returns>CurrentRole</returns>

        internal async Task<CurrentRole> GetCurrentRoleAsync()
        {

            // CurrentRole
            var cr = new CurrentRole();

            try
            {
                var userId = _signInManager.UserManager.GetUserId(User);
                var currentUser = await _signInManager.UserManager.FindByIdAsync(userId);

                cr.Email = currentUser.Email;
                cr.LojaId = currentUser.LojaId == null ? 0 : (int)currentUser.LojaId;
                cr.GrupolojaId = currentUser.GrupolojaId == null ? 0 : (int)currentUser.GrupolojaId;
                cr.EmpresaId = currentUser.EmpresaId == null ? 0 : (int)currentUser.EmpresaId;

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
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - GetCurrentRoleAsync - Exception: " + ex.Message);
                return cr;
            };
        }


        //---------------------------------------------------------------------------------------------------

    }
}