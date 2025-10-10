using Core.Constants;
using Core.Entities.Charts;
using Core.Entities.Identity;
using Core.Enums;
using Core.Extensions;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Gruposlojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetById;
using Core.Features.Mercados.Queries.GetAllCached;
using Core.Features.VendasDiarias.Queries.GetAllCached;
using Core.Features.VendasDiarias.Queries.GetByGrupolojaId;
using Core.Features.VendasDiarias.Queries.GetByLojaId;
using Core.Features.VendasDiarias.Queries.GetByVendaSemanalId;
using Core.Features.VendasSemanais.Queries.GetAllCached;
using Core.Features.VendasSemanais.Queries.GetByGrupolojaId;
using Core.Features.VendasSemanais.Queries.GetByLojaId;
using Core.Features.VendasSemanais.Queries.GetByMercadoId;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Business.Controllers.Mercado;
using LVLgroupApp.Areas.Business.Models.Loja;
using LVLgroupApp.Areas.Dashboard.Models.DashboardVendasTops;
using LVLgroupApp.Areas.Vendas.Controllers;
using LVLgroupApp.Areas.Vendas.Models.VendaDiaria;
using LVLgroupApp.Areas.Vendas.Models.VendaSemanal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;


namespace LVLgroupApp.Areas.Dashboard.Controllers.DashboardVendasTops
{

    [Area("Dashboard")]
    [Authorize]
    public class DashboardVendasTopsController : BaseController<DashboardVendasTopsController>
    {

        //---------------------------------------------------------------------------------------------------


        private IWebHostEnvironment _environment;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<DashboardVendasTopsController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public DashboardVendasTopsController(IWebHostEnvironment environment, 
                                        IStringLocalizer<DashboardVendasTopsController> localizer,
                                        SignInManager<ApplicationUser> signInManager)
        {
            _localizer = localizer;
            _environment = environment;
            _signInManager = signInManager;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - Index - return view");


            // Iniciar viewModel
            var viewModel = new DashboardVendasTopsViewModel();
            viewModel.CurrentRole = await GetCurrentRoleAsync();
            viewModel.Mercados = await MercadoController.GetSelectListAllMercadosAsync(viewModel.CurrentRole.MercadoId, _mapper, _mediator);
            viewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(viewModel.CurrentRole.EmpresaId, _mapper, _mediator);
            viewModel.GruposLojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(viewModel.CurrentRole.EmpresaId, viewModel.CurrentRole.GrupolojaId, _mapper, _mediator);
            viewModel.Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(viewModel.CurrentRole.GrupolojaId, viewModel.CurrentRole.LojaId, _mapper, _mediator);

            viewModel.AllGruposLojas = await GrupolojaController.GetSelectListAllGruposlojasAsync(0, _mapper, _mediator);


            var cInfo = new CultureInfo("pt-PT");

            viewModel.DataInicialDaSemana = DateTime.Now.MondayOfWeek();
            viewModel.DataFinalDaSemana = viewModel.DataInicialDaSemana.AddDays(6);
            viewModel.AnoDashboard = viewModel.DataInicialDaSemana.Year;
            viewModel.SemanaDashboard = cInfo.Calendar.GetWeekOfYear(viewModel.DataInicialDaSemana, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
            if ((viewModel.SemanaDashboard == 1) && (viewModel.DataInicialDaSemana.Month == 12))
            {
                // VendaSemanal pertence à semana 1 do ano seguinte
                viewModel.AnoDashboard = viewModel.AnoDashboard + 1;
            }


            viewModel.LVL_TotalVendasSemana = 0.00;


            // ler da db os mercados existents
            foreach (var mercado in viewModel.Mercados)
            {
                const char P = 'P'; // Portugal
                const char E = 'E'; // Espanha
                const char C = 'C'; // Canárias

                int value = 0;
                if (!int.TryParse(mercado.Value, out value))
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Tops Controller - Index - Error: Mercado Value não é um inteiro válido.");
                    continue;
                }
                var vendasSemana = Math.Round(await VendaSemanalController.GetValorTotalDaVenda(value, 0, 0, 0, viewModel.SemanaDashboard, viewModel.AnoDashboard, _mediator, _mapper), 2);
                viewModel.LVL_TotalVendasSemana = Math.Round(viewModel.LVL_TotalVendasSemana + vendasSemana, 2);

                switch (mercado.Text.ToUpper().First())
                {
                    case P: // Portugal
                        viewModel.PT_TotalVendasSemana = vendasSemana;
                        break;
                    case E: // Espanha
                        viewModel.ES_TotalVendasSemana = vendasSemana;
                        break;
                    case C: // Canárias
                        viewModel.CAN_TotalVendasSemana = vendasSemana;
                        break;
                }
            }
       

            return View("Index", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a atender os updates
        /// dos contadores na pag de index.
        /// devolve a lista de VendaSemanal para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Vendas.View)]
        public async Task<JsonResult> loadTotaisSemanais()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var semanafilter = Request.Form["semanafilter"].FirstOrDefault();
                var anofilter = Request.Form["anofilter"].FirstOrDefault();


                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                int semanaDashboard = semanafilter != null ? Convert.ToInt32(semanafilter) : 0;
                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;



                // Iniciar viewModel
                var viewModel = new DashboardVendasTopsViewModel();
                viewModel.CurrentRole = await GetCurrentRoleAsync();
                viewModel.Mercados = await MercadoController.GetSelectListAllMercadosAsync(viewModel.CurrentRole.MercadoId, _mapper, _mediator);
                viewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(viewModel.CurrentRole.EmpresaId, _mapper, _mediator);
                viewModel.GruposLojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(viewModel.CurrentRole.EmpresaId, viewModel.CurrentRole.GrupolojaId, _mapper, _mediator);
                viewModel.Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(viewModel.CurrentRole.GrupolojaId, viewModel.CurrentRole.LojaId, _mapper, _mediator);



                var cInfo = new CultureInfo("pt-PT");

                viewModel.DataInicialDaSemana = VendaSemanalController.FirstDateOfWeek(anoDashboard, semanaDashboard);
                viewModel.DataFinalDaSemana = viewModel.DataInicialDaSemana.AddDays(6);
                viewModel.AnoDashboard = anoDashboard;
                viewModel.SemanaDashboard = semanaDashboard;

                // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
                if ((viewModel.SemanaDashboard == 1) && (viewModel.DataInicialDaSemana.Month == 12))
                {
                    // VendaSemanal pertence à semana 1 do ano seguinte
                    viewModel.AnoDashboard = viewModel.AnoDashboard + 1;
                }



                viewModel.LVL_TotalVendasSemana = 0.00;



                // ler da db os mercados existents
                foreach (var mercado in viewModel.Mercados)
                {
                    const char P = 'P'; // Portugal
                    const char E = 'E'; // Espanha
                    const char C = 'C'; // Canárias

                    int value = 0;
                    if (!int.TryParse(mercado.Value, out value))
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Tops Controller - loadTotaisSemanais - Error: Mercado Value não é um inteiro válido.");
                        continue;
                    }
                    var vendasSemana = Math.Round(await VendaSemanalController.GetValorTotalDaVenda(value, 0, 0, 0, viewModel.SemanaDashboard, viewModel.AnoDashboard, _mediator, _mapper), 2);
                    viewModel.LVL_TotalVendasSemana = Math.Round(viewModel.LVL_TotalVendasSemana + vendasSemana, 2);

                    switch (mercado.Text.ToUpper().First())
                    {
                        case P: // Portugal
                            viewModel.PT_TotalVendasSemana = vendasSemana;
                            break;
                        case E: // Espanha
                            viewModel.ES_TotalVendasSemana = vendasSemana;
                            break;
                        case C: // Canárias
                            viewModel.CAN_TotalVendasSemana = vendasSemana;
                            break;
                    }
                }



                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { data = viewModel });
                return Json(jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | VendaSemanal Contoller - GetVendas - Exception vai sair e retornar Error: " + ex.Message);
                return Json(Newtonsoft.Json.JsonConvert.SerializeObject(new { data = "" }));
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// os charts de vendas Mercados/Empresas/Gruposlojas da Semana.
        /// </summary>
        /// <returns>PartialView("_dashboardChart_MEG_PorSemana", Chart_MEG_SemanalViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChart_MEG_Semanal()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var semanafilter = Request.Form["semanafilter"].FirstOrDefault();
                var anofilter = Request.Form["anofilter"].FirstOrDefault();




                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                int semanaDashboard = semanafilter != null ? Convert.ToInt32(semanafilter) : 0;
                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;




                // Iniciar viewModel
                var viewModel = new Chart_MEG_SemanalViewModel();
                viewModel.NumeroDaSemana = semanaDashboard;
                viewModel.Ano = anoDashboard;
                viewModel.M_VendasDaSemanaList = new List<ChartPointVendas>();
                viewModel.E_VendasDaSemanaList = new List<ChartPointVendas>();
                viewModel.G1_VendasDaSemanaList = new List<ChartPointVendas>();
                viewModel.G2_VendasDaSemanaList = new List<ChartPointVendas>();




                // Chart: total de vendas Mercado na Semana
                var response_merc = await _mediator.Send(new GetAllMercadosCachedQuery());
                if (response_merc.Succeeded)
                {
                    foreach (var merc in response_merc.Data)
                    {
                        ChartPointVendas cp = new ChartPointVendas();
                        cp.Entity = merc.Nome;
                        cp.EntityCount = Math.Round(await VendaSemanalController.GetValorTotalDaVenda(merc.Id, 0, 0, 0, viewModel.NumeroDaSemana, viewModel.Ano, _mediator, _mapper), 2);
                        viewModel.M_VendasDaSemanaList.Add(cp);
                    }
                }



                // Chart: total de vendas Empresa na Semana
                var response_emp = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (response_emp.Succeeded)
                {
                    foreach (var emp in response_emp.Data)
                    {
                        ChartPointVendas cp = new ChartPointVendas();
                        cp.Entity = emp.Nome;
                        cp.EntityCount = Math.Round(await VendaSemanalController.GetValorTotalDaVenda(0, emp.Id, 0, 0, viewModel.NumeroDaSemana, viewModel.Ano, _mediator, _mapper), 2);
                        viewModel.E_VendasDaSemanaList.Add(cp);
                    }
                }



                // Chart: total de vendas Agrupamentos GEOX na Semana
                var response_gl1 = await _mediator.Send(new GetAllGruposlojasByEmpresaIdCachedQuery() { empresaId = 1 });
                if (response_gl1.Succeeded)
                {
                    foreach (var gl1 in response_gl1.Data)
                    {
                        ChartPointVendas cpg1 = new ChartPointVendas();
                        cpg1.Entity = gl1.Nome;
                        cpg1.EntityCount = Math.Round(await VendaSemanalController.GetValorTotalDaVenda(0, 1, gl1.Id, 0, viewModel.NumeroDaSemana, viewModel.Ano, _mediator, _mapper), 2);
                        viewModel.G1_VendasDaSemanaList.Add(cpg1);
                    }
                }



                // Chart: total de vendas Agrupamentos SKECHERS na Semana
                var response_gl2 = await _mediator.Send(new GetAllGruposlojasByEmpresaIdCachedQuery() { empresaId = 2 });
                if (response_gl2.Succeeded)
                {
                    foreach (var gl2 in response_gl2.Data)
                    {
                        ChartPointVendas cpg2 = new ChartPointVendas();
                        cpg2.Entity = gl2.Nome;
                        cpg2.EntityCount = Math.Round(await VendaSemanalController.GetValorTotalDaVenda(0, 2, gl2.Id, 0, viewModel.NumeroDaSemana, viewModel.Ano, _mediator, _mapper), 2);
                        viewModel.G2_VendasDaSemanaList.Add(cpg2);
                    }
                }



                return PartialView("_dashboardChart_MEG_PorSemana", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - LoadChartVendasDia - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart de um mercado de vendas por semana.
        /// </summary>
        /// <returns>PartialView("_dashboardChart_M_PorSemana", Chart_M_SemanalViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChart_M_Semanal()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var semanafilter = Request.Form["semanafilter"].FirstOrDefault();
                var anofilter = Request.Form["anofilter"].FirstOrDefault();
                var title = Request.Form["title"].FirstOrDefault();
                var subtitle = Request.Form["subtitle"].FirstOrDefault();
                var divfilter = Request.Form["divfilter"].FirstOrDefault();




                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int semanaDashboard = semanafilter != null ? Convert.ToInt32(semanafilter) : 0;
                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;



                // Iniciar viewModel
                var viewModel = new Chart_M_SemanalViewModel();
                viewModel.NumeroDaSemana = semanaDashboard;
                viewModel.Ano = anoDashboard;
                viewModel.Title = string.IsNullOrEmpty(title) ? _localizer["Vendas da semana"] : title;
                viewModel.Subtitle = string.IsNullOrEmpty(subtitle) ? _localizer["Vendas da semana"] : subtitle;
                viewModel.DivId = string.IsNullOrEmpty(divfilter) ? string.Empty : divfilter;
                viewModel.VendasSemanaisColumnsList = new List<ChartBarColumn>();
                viewModel.VendasSemanaisRowsList = new List<ChartBarVendasRow>();





                // validar Mercado/grupoloja
                if (mercadoId == 0 && grupolojaId == 0)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - LoadChart_GL_Semanal - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                    return new ObjectResult(new { status = "error" });
                }




                var listLoja = new List<LojaViewModel>();
                var listVendasSemanais = new List<VendaSemanalViewModel>();




                // ler vendas e lojas do mercado/grupoloja de uma semana
                if (mercadoId > 0)
                {
                    // ler vendas e lojas do mercado
                    var responseM = await _mediator.Send(new GetVendasSemanaisByMercadoIdSemanaQuery() { MercadoId = mercadoId, Ano = anoDashboard, NumeroDaSemana = semanaDashboard});
                    if (!responseM.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - LoadChart_GL_Semanal - vai sair e retornar Error: Leitura da db das Vendas Semanais de Mercado, falhou.");
                        return new ObjectResult(new { status = "error" });
                    }

                    var responseML = await _mediator.Send(new GetLojasByMercadoIdCachedQuery() { mercadoId = mercadoId });
                    if (!responseML.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - LoadChart_GL_Semanal - vai sair e retornar Error: Leitura da db das lojas de um Mercado, falhou.");
                        return new ObjectResult(new { status = "error" });
                    }

                    listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseM.Data);
                    listLoja = _mapper.Map<List<LojaViewModel>>(responseML.Data);
                }
                else
                {
                    // ler vendas e lojas do grupoloja
                    var responseGL = await _mediator.Send(new GetVendasSemanaisByGrupolojaIdSemanaQuery() { GrupolojaId = grupolojaId, Ano = anoDashboard, NumeroDaSemana = semanaDashboard });
                    if (!responseGL.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - LoadChart_GL_Semanal - vai sair e retornar Error: Leitura da db das Vendas Semanais de Grupoloja, falhou.");
                        return new ObjectResult(new { status = "error" });
                    }

                    var responseGLL = await _mediator.Send(new GetLojasByGrupolojaIdCachedQuery() { grupolojaId = grupolojaId });
                    if (!responseGLL.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - LoadChart_GL_Semanal - vai sair e retornar Error: Leitura da db das lojas de um Grupoloja, falhou.");
                        return new ObjectResult(new { status = "error" });
                    }

                    listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseGL.Data);
                    listLoja = _mapper.Map<List<LojaViewModel>>(responseGLL.Data);
                }




                // remover nulls se existirem
                listLoja.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vs => vs == null);



                // Chart: vendas por loja
                // add columns (loja + objetivo)
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = _localizer["Loja"],
                };
                viewModel.VendasSemanaisColumnsList.Add(lbl);
                var venda_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = viewModel.NumeroDaSemana.ToString() + " / " + viewModel.Ano.ToString(),
                };
                viewModel.VendasSemanaisColumnsList.Add(venda_col);
                var obj_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Objetivo"],
                };
                viewModel.VendasSemanaisColumnsList.Add(obj_col);





                // add rows (vendas por loja)
                foreach (var lj in listLoja)
                {
                    // a venda semanal de uma loja na semana 
                    var vendaSemanal = listVendasSemanais.Where(vs => vs.LojaId == lj.Id).FirstOrDefault();
                    if (vendaSemanal == null) continue;


                    var chartRow = new ChartBarVendasRow();
                    chartRow.Label = string.Join(" ", lj.Nome.Split().Skip(1));
                    //chartRow.Label = lj.Nome;
                    chartRow.ValuesList = new List<double>();


                    // calcular valores (vendas, objetivo)
                    var valVendas = Math.Round(vendaSemanal.ValorTotalDaVenda, 2);
                    var valObjetivo = Math.Round(vendaSemanal.ObjetivoDaVendaSemanal, 2);


                    chartRow.ValuesList.Add(valVendas);
                    chartRow.ValuesList.Add(valObjetivo);

                    // adicionar row ao chart
                    viewModel.VendasSemanaisRowsList.Add(chartRow);
                }


                return PartialView("_dashboardChart_M_PorSemana", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - LoadChartVendasSemana - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// a table de vendas por semana do mercado.
        /// </summary>
        /// <returns>PartialView("_dashboardTabelas_M_PorSemana", Table_M_SemanaViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTable_M_PorSemana(int id, string label, string div)
        {
            // Iniciar viewModel
            var viewModel = new Table_M_SemanaViewModel();
            viewModel.Label = label;
            viewModel.MercadoId = id;
            viewModel.DivId = div;
            return PartialView("_dashboardTabelas_M_PorSemana", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das tabelas de vendas por semana. (DataTables)
        /// devolve a lista de vendas por semana
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendas_M_PorSemana()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();

                var dateStr = Request.Form["date"].FirstOrDefault();




                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                var semanafilter = Request.Form["semanafilter"].FirstOrDefault();
                var anofilter = Request.Form["anofilter"].FirstOrDefault();




                DateTime dashDate = String.IsNullOrEmpty(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);
                int semanaDashboard = semanafilter != null ? Convert.ToInt32(semanafilter) : 0;
                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;




                // validar Mercado
                if (mercadoId == 0)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendas_M_PorSemana - vai sair e retornar Error: Mercado inválido.");
                    return new ObjectResult(new { status = "error" });
                }



                // Iniciar viewModel
                var viewModel = new List<VendaLojaViewModel>();




                var listLoja = new List<LojaViewModel>();
                var listVendasSemanais = new List<VendaSemanalViewModel>();
                var listVendasSemanais_AA = new List<VendaSemanalViewModel>();



                // ler vendas e lojas do mercado de uma semana
                var responseM = await _mediator.Send(new GetVendasSemanaisByMercadoIdSemanaQuery() { MercadoId = mercadoId, Ano = anoDashboard, NumeroDaSemana = semanaDashboard });
                var responseM_AA = await _mediator.Send(new GetVendasSemanaisByMercadoIdSemanaQuery() { MercadoId = mercadoId, Ano = anoDashboard - 1, NumeroDaSemana = semanaDashboard });
                if (!responseM.Succeeded || !responseM_AA.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendas_M_PorSemana - vai sair e retornar Error: Leitura da db das Vendas Semanais de Mercado, falhou.");
                    return new ObjectResult(new { status = "error" });
                }

                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseM.Data);
                listVendasSemanais_AA = _mapper.Map<List<VendaSemanalViewModel>>(responseM_AA.Data);





                // remover nulls se existirem
                //listLoja.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vs => vs == null);
                listVendasSemanais_AA.RemoveAll(vs => vs == null);




                // add rows (vendas por loja do ano)
                foreach (var vs in listVendasSemanais)
                {
                    // ler loja da db
                    var responseL = await _mediator.Send(new GetLojaByIdQuery() { Id = vs.LojaId });
                    if (!responseL.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendasPorSemana - vai sair e retornar Error: Leitura da loja, falhou.");
                        return new ObjectResult(new { status = "error" });
                    }
                    var lj = _mapper.Map<LojaViewModel>(responseL.Data);

                    // ler vendas diárias da db
                    var responseVD = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vs.Id});
                    if (!responseVD.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendasPorSemana - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                        return new ObjectResult(new { status = "error" });
                    }
                    //Todas as vendasSemanais num ano e no anterior
                    var vdList = _mapper.Map<List<VendaDiariaViewModel>>(responseVD.Data);

                    var Variacao = 0.00;
                    if (vs.ObjetivoDaVendaSemanal > 0)
                    {
                        Variacao = ((vs.ValorTotalDaVenda - vs.ObjetivoDaVendaSemanal) / vs.ObjetivoDaVendaSemanal) * 100;
                    }

                    // criar registo para a loja desta venda
                    var vendaLoja = new VendaLojaViewModel();
                    vendaLoja.LojaId = lj.Id;
                    vendaLoja.LojaNome = lj.Nome;
                    vendaLoja.Quantidade = vdList.Sum(vd => vd.TotalArtigos);
                    vendaLoja.Valor = vs.ValorTotalDaVenda;
                    vendaLoja.Objetivo = vs.ObjetivoDaVendaSemanal;
                    vendaLoja.Variacao = Math.Round(Variacao, 2);

                    viewModel.Add(vendaLoja);
                }

                // atualizar rows com vendas do ano anterior
                foreach (var vs in listVendasSemanais_AA)
                {
                    // ler vendas diárias da db
                    var responseVD = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vs.Id });
                    if (!responseVD.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendasPorSemana - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                        return new ObjectResult(new { status = "error" });
                    }
                    //Todas as vendasSemanais num ano e no anterior
                    var vdList = _mapper.Map<List<VendaDiariaViewModel>>(responseVD.Data);

                    // verificar se a loja desta venda já existe em viewModel
                    var vdLoja = viewModel.Where(v => v.LojaId == vs.LojaId).FirstOrDefault();
                    if (vdLoja == null)
                    {
                        // a loja desta venda ainda não existe em viewModel
                        // ler loja da db
                        var responseL = await _mediator.Send(new GetLojaByIdQuery() { Id = vs.LojaId });
                        if (!responseL.Succeeded)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendasPorSemana - vai sair e retornar Error: Leitura da loja, falhou.");
                            return new ObjectResult(new { status = "error" });
                        }
                        var lj = _mapper.Map<LojaViewModel>(responseL.Data);

                        
                        var vendaLoja = new VendaLojaViewModel();
                        vendaLoja.LojaId = vs.LojaId;
                        vendaLoja.LojaNome = string.Join(" ", lj.Nome.Split().Skip(1));
                        //vendaLoja.LojaNome = lj.Nome;
                        vendaLoja.AA_Quantidade = vdList.Sum(vd => vd.TotalArtigos);
                        vendaLoja.AA_Valor = vs.ValorTotalDaVenda;
                        vendaLoja.Objetivo = 0.00;
                        vendaLoja.Variacao = 0.00;

                        viewModel.Add(vendaLoja);
                    }
                    else
                    {
                        // a loja desta venda já existe em viewModel
                        vdLoja.AA_Valor = vs.ValorTotalDaVenda;
                        vdLoja.AA_Quantidade = vdList.Sum(vd => vd.TotalArtigos);
                    }
                }



                //adicionar ultima fila com o total de vendas
                var totalVendas = Math.Round(viewModel.Sum(v => v.Valor), 2);
                var objetivo = Math.Round(viewModel.Sum(v => v.Objetivo), 2);

                var totalVendasAno = new VendaLojaViewModel
                {
                    LojaId = 0,
                    LojaNome = _localizer["Total"],
                    Quantidade = viewModel.Sum(v => v.Quantidade),
                    AA_Quantidade = viewModel.Sum(v => v.AA_Quantidade),
                    Valor = totalVendas,
                    AA_Valor = Math.Round(viewModel.Sum(v => v.AA_Valor), 2),
                    Objetivo = objetivo,
                    Variacao = objetivo == 0 ? 0 : Math.Round(((totalVendas - objetivo) / objetivo) * 100, 2)
                };

                viewModel.Add(totalVendasAno);

                var jsonData = new { draw = draw, recordsFiltered = viewModel.Count, recordsTotal = viewModel.Count, data = viewModel };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendasPorSemana - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------

















        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart de um grupolojas de vendas por semana.
        /// </summary>
        /// <returns>PartialView("_dashboardChart_GL_PorSemana", Chart_GL_SemanalViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChart_GL_Semanal()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var semanafilter = Request.Form["semanafilter"].FirstOrDefault();
                var anofilter = Request.Form["anofilter"].FirstOrDefault();
                var title = Request.Form["title"].FirstOrDefault();
                var subtitle = Request.Form["subtitle"].FirstOrDefault();
                var divfilter = Request.Form["divfilter"].FirstOrDefault();




                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int semanaDashboard = semanafilter != null ? Convert.ToInt32(semanafilter) : 0;
                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;



                // Iniciar viewModel
                var viewModel = new Chart_GL_SemanalViewModel();
                viewModel.NumeroDaSemana = semanaDashboard;
                viewModel.Ano = anoDashboard;
                viewModel.Title = string.IsNullOrEmpty(title) ? _localizer["Vendas da semana"] : title;
                viewModel.Subtitle = string.IsNullOrEmpty(subtitle) ? _localizer["Vendas da semana"] : subtitle;
                viewModel.DivId = string.IsNullOrEmpty(divfilter) ? string.Empty : divfilter;
                viewModel.VendasSemanaisColumnsList = new List<ChartBarColumn>();
                viewModel.VendasSemanaisRowsList = new List<ChartBarVendasRow>();





                // validar grupoloja
                if (grupolojaId == 0)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - LoadChart_GL_Semanal - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                    return new ObjectResult(new { status = "error" });
                }




                var listLoja = new List<LojaViewModel>();
                var listVendasSemanais = new List<VendaSemanalViewModel>();




                // ler vendas e lojas do grupoloja de uma semana
                var responseGL = await _mediator.Send(new GetVendasSemanaisByGrupolojaIdSemanaQuery() { GrupolojaId = grupolojaId, Ano = anoDashboard, NumeroDaSemana = semanaDashboard });
                if (!responseGL.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - LoadChart_GL_Semanal - vai sair e retornar Error: Leitura da db das Vendas Semanais de Grupoloja, falhou.");
                    return new ObjectResult(new { status = "error" });
                }

                var responseGLL = await _mediator.Send(new GetLojasByGrupolojaIdCachedQuery() { grupolojaId = grupolojaId });
                if (!responseGLL.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - LoadChart_GL_Semanal - vai sair e retornar Error: Leitura da db das lojas de um Grupoloja, falhou.");
                    return new ObjectResult(new { status = "error" });
                }

                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseGL.Data);
                listLoja = _mapper.Map<List<LojaViewModel>>(responseGLL.Data);





                // remover nulls se existirem
                listLoja.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vs => vs == null);



                // Chart: vendas por loja
                // add columns (loja + objetivo)
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = _localizer["Loja"],
                };
                viewModel.VendasSemanaisColumnsList.Add(lbl);
                var venda_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = viewModel.NumeroDaSemana.ToString() + " / " + viewModel.Ano.ToString(),
                };
                viewModel.VendasSemanaisColumnsList.Add(venda_col);
                var obj_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Objetivo"],
                };
                viewModel.VendasSemanaisColumnsList.Add(obj_col);





                // add rows (vendas por loja)
                foreach (var lj in listLoja)
                {
                    // a venda semanal de uma loja na semana 
                    var vendaSemanal = listVendasSemanais.Where(vs => vs.LojaId == lj.Id).FirstOrDefault();
                    if (vendaSemanal == null) continue;


                    var chartRow = new ChartBarVendasRow();
                    chartRow.Label = string.Join(" ", lj.Nome.Split().Skip(1));
                    //chartRow.Label = lj.Nome;
                    chartRow.ValuesList = new List<double>();


                    // calcular valores (vendas, objetivo)
                    var valVendas = Math.Round(vendaSemanal.ValorTotalDaVenda, 2);
                    var valObjetivo = Math.Round(vendaSemanal.ObjetivoDaVendaSemanal, 2);


                    chartRow.ValuesList.Add(valVendas);
                    chartRow.ValuesList.Add(valObjetivo);

                    // adicionar row ao chart
                    viewModel.VendasSemanaisRowsList.Add(chartRow);
                }


                return PartialView("_dashboardChart_GL_PorSemana", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - LoadChartVendasSemana - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// a table de vendas por semana do grupolojas.
        /// </summary>
        /// <returns>PartialView("_dashboardTabelas_GL_PorSemana", Table_GL_SemanaViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTable_GL_PorSemana(int id, string label, string div)
        {
            // Iniciar viewModel
            var viewModel = new Table_GL_SemanaViewModel();
            viewModel.Label = label;
            viewModel.GrupoLojasId = id;
            viewModel.DivId = div;
            return PartialView("_dashboardTabelas_GL_PorSemana", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das tabelas de vendas por semana. (DataTables)
        /// devolve a lista de vendas por semana
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendas_GL_PorSemana()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();

                var dateStr = Request.Form["date"].FirstOrDefault();




                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                var semanafilter = Request.Form["semanafilter"].FirstOrDefault();
                var anofilter = Request.Form["anofilter"].FirstOrDefault();




                DateTime dashDate = String.IsNullOrEmpty(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);
                int semanaDashboard = semanafilter != null ? Convert.ToInt32(semanafilter) : 0;
                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;




                // validar grupoloja
                if (grupolojaId == 0)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendas_GL_PorSemana - vai sair e retornar Error: Grupoloja inválido.");
                    return new ObjectResult(new { status = "error" });
                }



                // Iniciar viewModel
                var viewModel = new List<VendaLojaViewModel>();




                var listLoja = new List<LojaViewModel>();
                var listVendasSemanais = new List<VendaSemanalViewModel>();
                var listVendasSemanais_AA = new List<VendaSemanalViewModel>();



                // ler vendas e lojas do grupoloja de uma semana
                var responseGL = await _mediator.Send(new GetVendasSemanaisByGrupolojaIdSemanaQuery() { GrupolojaId = grupolojaId, Ano = anoDashboard, NumeroDaSemana = semanaDashboard });
                var responseGL_AA = await _mediator.Send(new GetVendasSemanaisByGrupolojaIdSemanaQuery() { GrupolojaId = grupolojaId, Ano = anoDashboard - 1, NumeroDaSemana = semanaDashboard });
                if (!responseGL.Succeeded || !responseGL_AA.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendas_GL_PorSemana - vai sair e retornar Error: Leitura da db das Vendas Semanais de Grupoloja, falhou.");
                    return new ObjectResult(new { status = "error" });
                }

                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseGL.Data);
                listVendasSemanais_AA = _mapper.Map<List<VendaSemanalViewModel>>(responseGL_AA.Data);





                // remover nulls se existirem
                //listLoja.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vs => vs == null);
                listVendasSemanais_AA.RemoveAll(vs => vs == null);




                // add rows (vendas por loja do ano)
                foreach (var vs in listVendasSemanais)
                {
                    // ler loja da db
                    var responseL = await _mediator.Send(new GetLojaByIdQuery() { Id = vs.LojaId });
                    if (!responseL.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendas_GL_PorSemana - vai sair e retornar Error: Leitura da loja, falhou.");
                        return new ObjectResult(new { status = "error" });
                    }
                    var lj = _mapper.Map<LojaViewModel>(responseL.Data);

                    // ler vendas diárias da db
                    var responseVD = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vs.Id });
                    if (!responseVD.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendas_GL_PorSemana - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                        return new ObjectResult(new { status = "error" });
                    }
                    //Todas as vendasSemanais num ano e no anterior
                    var vdList = _mapper.Map<List<VendaDiariaViewModel>>(responseVD.Data);

                    var Variacao = 0.00;
                    if (vs.ObjetivoDaVendaSemanal > 0)
                    {
                        Variacao = ((vs.ValorTotalDaVenda - vs.ObjetivoDaVendaSemanal) / vs.ObjetivoDaVendaSemanal) * 100;
                    }

                    // criar registo para a loja desta venda
                    var vendaLoja = new VendaLojaViewModel();
                    vendaLoja.LojaId = lj.Id;
                    vendaLoja.LojaNome = lj.Nome;
                    vendaLoja.Quantidade = vdList.Sum(vd => vd.TotalArtigos);
                    vendaLoja.Valor = vs.ValorTotalDaVenda;
                    vendaLoja.Objetivo = vs.ObjetivoDaVendaSemanal;
                    vendaLoja.Variacao = Math.Round(Variacao, 2);

                    viewModel.Add(vendaLoja);
                }

                // atualizar rows com vendas do ano anterior
                foreach (var vs in listVendasSemanais_AA)
                {
                    // ler vendas diárias da db
                    var responseVD = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vs.Id });
                    if (!responseVD.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendas_GL_PorSemana - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                        return new ObjectResult(new { status = "error" });
                    }
                    //Todas as vendasSemanais num ano e no anterior
                    var vdList = _mapper.Map<List<VendaDiariaViewModel>>(responseVD.Data);

                    // verificar se a loja desta venda já existe em viewModel
                    var vdLoja = viewModel.Where(v => v.LojaId == vs.LojaId).FirstOrDefault();
                    if (vdLoja == null)
                    {
                        // a loja desta venda ainda não existe em viewModel
                        // ler loja da db
                        var responseL = await _mediator.Send(new GetLojaByIdQuery() { Id = vs.LojaId });
                        if (!responseL.Succeeded)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendas_GL_PorSemana - vai sair e retornar Error: Leitura da loja, falhou.");
                            return new ObjectResult(new { status = "error" });
                        }
                        var lj = _mapper.Map<LojaViewModel>(responseL.Data);


                        var vendaLoja = new VendaLojaViewModel();
                        vendaLoja.LojaId = vs.LojaId;
                        vendaLoja.LojaNome = string.Join(" ", lj.Nome.Split().Skip(1));
                        //vendaLoja.LojaNome = lj.Nome;
                        vendaLoja.AA_Quantidade = vdList.Sum(vd => vd.TotalArtigos);
                        vendaLoja.AA_Valor = vs.ValorTotalDaVenda;
                        vendaLoja.Objetivo = 0.00;
                        vendaLoja.Variacao = 0.00;

                        viewModel.Add(vendaLoja);
                    }
                    else
                    {
                        // a loja desta venda já existe em viewModel
                        vdLoja.AA_Valor = vs.ValorTotalDaVenda;
                        vdLoja.AA_Quantidade = vdList.Sum(vd => vd.TotalArtigos);
                    }
                }



                //adicionar ultima fila com o total de vendas
                var totalVendas = Math.Round(viewModel.Sum(v => v.Valor), 2);
                var objetivo = Math.Round(viewModel.Sum(v => v.Objetivo), 2);

                var totalVendasAno = new VendaLojaViewModel
                {
                    LojaId = 0,
                    LojaNome = _localizer["Total"],
                    Quantidade = viewModel.Sum(v => v.Quantidade),
                    AA_Quantidade = viewModel.Sum(v => v.AA_Quantidade),
                    Valor = totalVendas,
                    AA_Valor = Math.Round(viewModel.Sum(v => v.AA_Valor), 2),
                    Objetivo = objetivo,
                    Variacao = objetivo == 0 ? 0 : Math.Round(((totalVendas - objetivo) / objetivo) * 100, 2)
                };

                viewModel.Add(totalVendasAno);

                var jsonData = new { draw = draw, recordsFiltered = viewModel.Count, recordsTotal = viewModel.Count, data = viewModel };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Top Vendas Controller - GetVendasPorSemana - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria um CurrentRole para o utilizador corrente "User"
        /// </summary>
        /// <param></param>
        /// <returns>type="CurrentRole"</returns>

        internal async Task<CurrentRole> GetCurrentRoleAsync()
        {

            // CurrentRole
            var cr = new CurrentRole();
            var appUserId = string.Empty;

            try
            {
                appUserId =  _signInManager.UserManager.GetUserId(User);
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
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}
