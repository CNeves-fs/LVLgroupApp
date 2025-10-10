using Core.Constants;
using Core.Entities.Charts;
using Core.Entities.Identity;
using Core.Enums;
using Core.Extensions;
using Core.Features.VendasDiarias.Queries.GetAllCached;
using Core.Features.VendasDiarias.Queries.GetByGrupolojaId;
using Core.Features.VendasDiarias.Queries.GetByLojaId;
using Core.Features.VendasDiarias.Queries.GetByVendaSemanalId;
using Core.Features.VendasSemanais.Queries.GetAllCached;
using Core.Features.VendasSemanais.Queries.GetByGrupolojaId;
using Core.Features.VendasSemanais.Queries.GetByLojaId;
using Infrastructure.Extensions;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Business.Controllers.Mercado;
using LVLgroupApp.Areas.Dashboard.Models.DashboardVendasGeral;
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


namespace LVLgroupApp.Areas.Dashboard.Controllers.DashboardVendasGeral
{

    [Area("Dashboard")]
    [Authorize]
    public class DashboardVendasGeralController : BaseController<DashboardVendasGeralController>
    {

        //---------------------------------------------------------------------------------------------------


        private IWebHostEnvironment _environment;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<DashboardVendasGeralController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public DashboardVendasGeralController(IWebHostEnvironment environment, 
                                        IStringLocalizer<DashboardVendasGeralController> localizer,
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
            var viewModel = new DashboardVendasGeralViewModel();
            viewModel.CurrentRole = await GetCurrentRoleAsync();
            viewModel.Mercados = await MercadoController.GetSelectListAllMercadosAsync(viewModel.CurrentRole.MercadoId, _mapper, _mediator);
            viewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(viewModel.CurrentRole.EmpresaId, _mapper, _mediator);
            viewModel.GruposLojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(viewModel.CurrentRole.EmpresaId, viewModel.CurrentRole.GrupolojaId, _mapper, _mediator);
            viewModel.Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(viewModel.CurrentRole.GrupolojaId, viewModel.CurrentRole.LojaId, _mapper, _mediator);



            var cInfo = new CultureInfo("pt-PT");

            viewModel.DataDashboard = DateTime.Now;
            viewModel.Ano = viewModel.DataDashboard.Year;
            viewModel.Mes = viewModel.DataDashboard.Month;
            viewModel.MesLiteral = cInfo.DateTimeFormat.GetMonthName(viewModel.Mes);
            viewModel.MesLiteral = viewModel.MesLiteral.FirstCharToUpper();
            viewModel.Dia = viewModel.DataDashboard.Day;
            viewModel.NumeroDaSemana = cInfo.Calendar.GetWeekOfYear(viewModel.DataDashboard, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
            if ((viewModel.NumeroDaSemana == 1) && (viewModel.Mes == 12))
            {
                // VendaSemanal pertence à semana 1 do ano seguinte
                viewModel.Ano = viewModel.Ano + 1;
            }
            viewModel.AA_DataDashboard = new DateTime(viewModel.Ano - 1, viewModel.Mes, viewModel.Dia);




            // ler vendas diárias do dia da db
            var vendasDoDia = await GetVendaDiariaDiaListAsync(viewModel.CurrentRole, viewModel.Ano, viewModel.Mes, viewModel.Dia);


            // ler vendas diárias do dia no ano anterior da db
            var AA_vendasDoDia = await GetVendaDiariaDiaListAsync(viewModel.CurrentRole, viewModel.Ano - 1, viewModel.Mes, viewModel.Dia);


            // ler vendas semanais da db
            var vendasDaSemana = await GetVendaSemanalDiaListAsync(viewModel.CurrentRole, viewModel.Ano, viewModel.NumeroDaSemana);


            // ler vendas semanais no ano anterior da db
            var AA_vendasDaSemana = await GetVendaSemanalDiaListAsync(viewModel.CurrentRole, viewModel.Ano - 1, viewModel.NumeroDaSemana);


            // ler vendas diárias do mês da db
            var vendasDoMes = await GetVendaDiariaMesListAsync(viewModel.CurrentRole, viewModel.Ano, viewModel.Mes);


            // ler vendas diárias do mês no ano anterior da db
            var AA_vendasDoMes = await GetVendaDiariaMesListAsync(viewModel.CurrentRole, viewModel.Ano - 1, viewModel.Mes);





            // total de unidades dia/semana/mês
            viewModel.TotalUnidadesDia = vendasDoDia.Sum(vd => vd.TotalArtigos);
            foreach (var vendaSemanal in vendasDaSemana)
            {
                var vendasDaSemanaId = await GetVendasDiariasAsync(vendaSemanal.Id);
                // total de unidades semana
                viewModel.TotalUnidadesSemana += vendasDaSemanaId.Sum(vd => vd.TotalArtigos);
            }
            viewModel.TotalUnidadesMes = vendasDoMes.Sum(vd => vd.TotalArtigos);



            // total de unidades dia/semana/mês do ano anterior
            viewModel.AA_TotalUnidadesDia = AA_vendasDoDia.Sum(vd => vd.TotalArtigos);
            foreach (var vendaSemanal in AA_vendasDaSemana)
            {
                var vendasDaSemanaId = await GetVendasDiariasAsync(vendaSemanal.Id);
                // total de unidades semana ano anterior
                viewModel.AA_TotalUnidadesSemana += vendasDaSemanaId.Sum(vd => vd.TotalArtigos);
            }
            viewModel.AA_TotalUnidadesMes = AA_vendasDoMes.Sum(vd => vd.TotalArtigos);



            // total de vendas
            viewModel.TotalVendasDia = Math.Round(vendasDoDia.Sum(vd => vd.ValorDaVenda), 2);
            viewModel.TotalVendasSemana = Math.Round(await VendaSemanalController.GetValorTotalDaVenda(
                viewModel.CurrentRole.MercadoId,
                viewModel.CurrentRole.EmpresaId,
                viewModel.CurrentRole.GrupolojaId,
                viewModel.CurrentRole.LojaId,
                viewModel.NumeroDaSemana,
                viewModel.Ano, _mediator, _mapper), 2);
            var totalVendasSemana = Math.Round(vendasDaSemana.Sum(vs => vs.ValorTotalDaVenda), 2);
            viewModel.TotalVendasMes = Math.Round(vendasDoMes.Sum(vd => vd.ValorDaVenda), 2);



            // total de vendas do ano anterior
            viewModel.AA_TotalVendasDia = Math.Round(AA_vendasDoDia.Sum(vd => vd.ValorDaVenda), 2);
            viewModel.AA_TotalVendasSemana = Math.Round(AA_vendasDaSemana.Sum(vs => vs.ValorTotalDaVenda), 2);
            viewModel.AA_TotalVendasMes = Math.Round(AA_vendasDoMes.Sum(vd => vd.ValorDaVenda), 2);



            // objetivos = vendas ano anterior + 10%
            viewModel.ObjetivoVendasDia = Math.Round(viewModel.AA_TotalVendasDia * 1.10, 2);
            viewModel.ObjetivoVendasSemana = Math.Round(await VendaSemanalController.GetObjetivoDaVendaSemanal(
                viewModel.CurrentRole.MercadoId,
                viewModel.CurrentRole.EmpresaId,
                viewModel.CurrentRole.GrupolojaId,
                viewModel.CurrentRole.LojaId,
                viewModel.NumeroDaSemana,
                viewModel.Ano, _mediator, _mapper), 2);
            var objetivoVendasSemana = Math.Round(viewModel.AA_TotalVendasSemana * 1.10, 2);
            viewModel.ObjetivoVendasMes = Math.Round(viewModel.AA_TotalVendasMes * 1.10, 2);


            return View("Index", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart de vendas por dia.
        /// </summary>
        /// <returns>PartialView("_dashboardChartPorDia", ChartDiarioViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChartVendasDia()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();
                var title = Request.Form["title"].FirstOrDefault();
                var subtitle = Request.Form["subtitle"].FirstOrDefault();



                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                DateTime dateDashboard = string.IsNullOrEmpty(diafilter) ? DateTime.Now : DateTime.Parse(diafilter);



                // Iniciar viewModel
                var viewModel = new ChartDiarioViewModel();
                viewModel.Title = string.IsNullOrEmpty(title) ? _localizer["Vendas por dia"] : title;
                viewModel.Subtitle = string.IsNullOrEmpty(subtitle) ? _localizer["Vendas por dia"] : subtitle;
                viewModel.VendasDiariasColumnsList = new List<ChartBarColumn>();
                viewModel.VendasDiariasRowsList = new List<ChartBarVendasRow>();



                // Iniciar datas das vendas
                var year = dateDashboard.Year;
                var semanaDataInicial = dateDashboard.MondayOfWeek();
                var cal = new CultureInfo("pt-PT").Calendar;
                var numeroDaSemana = cal.GetWeekOfYear(semanaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var yearAA = year - 1;
                var semanaDataInicialAA = VendaSemanalController.FirstDateOfWeek(yearAA, numeroDaSemana);



                // Chart: vendas por semana
                // add columns (anos + acumulado + objetivo)
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = _localizer["Dia da Semana"],
                };
                viewModel.VendasDiariasColumnsList.Add(lbl);
                var anoAA_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = yearAA.ToString(),
                };
                viewModel.VendasDiariasColumnsList.Add(anoAA_col);
                var ano_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = year.ToString(),
                };
                viewModel.VendasDiariasColumnsList.Add(ano_col);
                var acu_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Acumulado"],
                };
                viewModel.VendasDiariasColumnsList.Add(acu_col);
                var obj_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Objetivo"],
                };
                viewModel.VendasDiariasColumnsList.Add(obj_col);



                // ler vendas da db
                var responseSemana = await _mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = year, NumeroDaSemana = numeroDaSemana });
                var responseSemanaAA = await _mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = yearAA, NumeroDaSemana = numeroDaSemana });
                if (!responseSemana.Succeeded || !responseSemanaAA.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - LoadChartVendasSemana - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                    return new ObjectResult(new { status = "error" });
                }
                //Todas as vendasSemanais da semana num ano e no anterior
                var vendasWeekList = _mapper.Map<List<VendaSemanalViewModel>>(responseSemana.Data);
                var vendasWeekListAA = _mapper.Map<List<VendaSemanalViewModel>>(responseSemanaAA.Data);



                // filtrar por loja/grupoloja/empresa/mercado se necessário
                vendasWeekList = GetFilteredVendasSemanais(vendasWeekList.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();
                vendasWeekListAA = GetFilteredVendasSemanais(vendasWeekListAA.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();


                // valor acumulado ao longo da semana
                var totalAcumuladoSemana = 0.00;



                // add rows (Dias da Semana)
                for (var diaDaSemana = 1; diaDaSemana <= 7; diaDaSemana++)
                {
                    // diaDaSemana == 1 => 2ªfeira
                    // diaDaSemana == 7 => Domingo
                    var dia = semanaDataInicial.AddDays(diaDaSemana - 1);
                    var diaAA = semanaDataInicialAA.AddDays(diaDaSemana - 1);
                    var dayName = dia.ToString("ddd", new CultureInfo(_localizer["pt-PT"]));
                    var totalVendasDoDiaDaSemana = new List<VendaDiariaViewModel>();
                    var totalVendasDoDiaDaSemanaAA = new List<VendaDiariaViewModel>();
                    var totalObjetivoSemana = vendasWeekList.Sum(vs => vs.ObjetivoDaVendaSemanal);
                    var totalObjetivoSemanaAA = vendasWeekListAA.Sum(vs => vs.ObjetivoDaVendaSemanal);



                    // apurar as vendas diárias deste dia da semana
                    foreach (var vs in vendasWeekList)
                    {
                        var responseVS = await _mediator.Send(new GetVendaDiariaByVendaSemanalIdDiaQuery() { VendaSemanalId = vs.Id, DiaDaSemana = diaDaSemana });
                        if (responseVS.Succeeded)
                        {
                            var vd = _mapper.Map<VendaDiariaViewModel>(responseVS.Data);
                            totalVendasDoDiaDaSemana.Add(vd);
                        }
                    }

                    foreach (var vs in vendasWeekListAA)
                    {
                        var responseVS = await _mediator.Send(new GetVendaDiariaByVendaSemanalIdDiaQuery() { VendaSemanalId = vs.Id, DiaDaSemana = diaDaSemana });
                        if (responseVS.Succeeded)
                        {
                            var vd = _mapper.Map<VendaDiariaViewModel>(responseVS.Data);
                            totalVendasDoDiaDaSemanaAA.Add(vd);
                        }
                    }



                    // remover nulls se existirem
                    totalVendasDoDiaDaSemana.RemoveAll(vd => vd == null);
                    totalVendasDoDiaDaSemanaAA.RemoveAll(vd => vd == null);



                    var chartRow = new ChartBarVendasRow();
                    chartRow.Label = dayName + " " + dia.ToString("dd/MM");
                    chartRow.ValuesList = new List<double>();

                    // calcular valores (anoAA, ano, acumulado, objetivo)
                    chartRow.ValuesList.Add(Math.Round(totalVendasDoDiaDaSemanaAA.Sum(v => v.ValorDaVenda), 2));
                    var totalVd = Math.Round(totalVendasDoDiaDaSemana.Sum(v => v.ValorDaVenda), 2);
                    totalAcumuladoSemana = totalAcumuladoSemana + totalVd;
                    chartRow.ValuesList.Add(totalVd);
                    chartRow.ValuesList.Add(totalAcumuladoSemana);
                    chartRow.ValuesList.Add(Math.Round(totalObjetivoSemana, 2));

                    // adicionar row ao chart
                    viewModel.VendasDiariasRowsList.Add(chartRow);

                }


                return PartialView("_dashboardChartPorDia", viewModel);
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
        /// as duas tables de vendas por dia.
        /// </summary>
        /// <returns>PartialView("_dashboardVendasDiarias", DashboardVendasDiaViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTablesVendasDia()
        {
            // Iniciar viewModel
            var viewModel = new DashboardVendasDiaViewModel();
            return PartialView("_dashboardVendasDiarias", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das duas tabelas de vendas por dia. (DataTables)
        /// devolve a lista de vendas por dia
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendasPorDia()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();

                var dateStr = Request.Form["date"].FirstOrDefault();



                int intMercadoFilter = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int intEmpresaFilter = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int intGrupolojaFilter = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int intLojaFilter = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;

                DateTime dashDate = String.IsNullOrEmpty(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);



                var semanaDataInicial = dashDate.MondayOfWeek();
                var cal = new CultureInfo("pt-PT").Calendar;
                var numeroDaSemana = cal.GetWeekOfYear(semanaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);



                // ler objetivo semanal da db
                var responseSemana = await _mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = semanaDataInicial.Year, NumeroDaSemana = numeroDaSemana });
                if (!responseSemana.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - GetVendasPorSemana - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                    return new ObjectResult(new { status = "error" });
                }

                var vendasSemanaisList = _mapper.Map<List<VendaSemanalViewModel>>(responseSemana.Data);

                // filtrar por loja/grupoloja/empresa/mercado se necessário
                vendasSemanaisList = GetFilteredVendasSemanais(vendasSemanaisList.AsQueryable(), intMercadoFilter, intEmpresaFilter, intGrupolojaFilter, intLojaFilter).ToList();

                var objSemanal = vendasSemanaisList.Sum(vs => vs.ObjetivoDaVendaSemanal);
                var acumuladoSemanal = 0.00;



                // lista de vendas do dia
                var allVendas = new List<VendasDayViewModel>();



                for (var diaDaSemana = 0; diaDaSemana <= 6; diaDaSemana++)
                {
                    // diaDaSemana == 0 => 2ªfeira
                    // diaDaSemana == 1 => 3ªfeira
                    // diaDaSemana == 6 => Domingo
                    var diaVendas = semanaDataInicial.AddDays(diaDaSemana);
                    var dayName = diaVendas.ToString("ddd", new CultureInfo(_localizer["pt-PT"])).ToLower().FirstCharToUpper();



                    var response = await _mediator.Send(new GetVendasDiariasDiaCachedQuery() { Ano = diaVendas.Year, Mes = diaVendas.Month, Dia = diaVendas.Day });
                    if (!response.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - GetVendasPorSemana - vai sair e retornar Error: Leitura Vendas Diárias da db falhou.");
                        return new ObjectResult(new { status = "error" });
                    }
                    var vendasWeekDayList = _mapper.Map<List<VendaDiariaViewModel>>(response.Data);

                    // filtrar por loja/grupoloja/empresa/mercado se necessário
                    vendasWeekDayList = GetFilteredVendasDiarias(vendasWeekDayList.AsQueryable(), intMercadoFilter, intEmpresaFilter, intGrupolojaFilter, intLojaFilter).ToList();



                    // remover nulls
                    vendasWeekDayList.RemoveAll(vd => vd == null);



                    var Valor = vendasWeekDayList.Sum(v => v.ValorDaVenda);
                    acumuladoSemanal = acumuladoSemanal + Valor;
                    var Variacao = 0.00;
                    if (objSemanal > 0)
                    {
                        Variacao = ((acumuladoSemanal - objSemanal) / objSemanal) * 100;
                    }



                    var vWeekDay = new VendasDayViewModel
                    {
                        DiaDaSemanaName = dayName.ToLower() + " " + diaVendas.ToString("dd/MM"),
                        Quantidade = vendasWeekDayList.Sum(v => v.TotalArtigos),
                        Valor = Math.Round(Valor, 2),
                        Acumulado = Math.Round(acumuladoSemanal, 2),
                        Objetivo = Math.Round(objSemanal, 2),
                        Variacao = Math.Round(Variacao, 2)
                    };

                    if (vWeekDay.Valor > 0)
                    {
                        allVendas.Add(vWeekDay);
                    }

                }

                //adicionar ultima fila com o total de vendas
                var totalVendas = Math.Round(allVendas.Sum(v => v.Valor), 2);

                var totalVendasWeekDay = new VendasDayViewModel
                {
                    DiaDaSemanaName = _localizer["Total"],
                    Quantidade = allVendas.Sum(v => v.Quantidade),
                    Valor = Math.Round(allVendas.Sum(v => v.Valor), 2),
                    Objetivo = Math.Round(objSemanal, 2),
                    Acumulado = Math.Round(allVendas.Sum(v => v.Valor), 2),
                    Variacao = objSemanal == 0 ? 0 : Math.Round(((totalVendas - objSemanal) / objSemanal) * 100, 2)
                };

                allVendas.Add(totalVendasWeekDay);

                var jsonData = new { draw = draw, recordsFiltered = allVendas.Count, recordsTotal = allVendas.Count, data = allVendas };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - GetVendasPorMes - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart de vendas por semana.
        /// </summary>
        /// <returns>PartialView("_dashboardChartPorSemana", ChartSemanalViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChartVendasSemana()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();
                var title = Request.Form["title"].FirstOrDefault();
                var subtitle = Request.Form["subtitle"].FirstOrDefault();



                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                DateTime dateDashboard = string.IsNullOrEmpty(diafilter) ? DateTime.Now : DateTime.Parse(diafilter);



                // Iniciar viewModel
                var viewModel = new ChartSemanalViewModel();
                viewModel.Title = string.IsNullOrEmpty(title) ? _localizer["Vendas por dia"] : title;
                viewModel.Subtitle = string.IsNullOrEmpty(subtitle) ? _localizer["Vendas por dia"] : subtitle;
                viewModel.VendasSemanaisColumnsList = new List<ChartBarColumn>();
                viewModel.VendasSemanaisRowsList = new List<ChartBarVendasRow>();



                // Iniciar datas das vendas
                var year = dateDashboard.Year;
                var semanaDataInicial = dateDashboard.MondayOfWeek();
                var cal = new CultureInfo("pt-PT").Calendar;
                var numeroDaSemana = cal.GetWeekOfYear(semanaDataInicial, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                var yearAA = year - 1;
                var semanaDataInicialAA = VendaSemanalController.FirstDateOfWeek(yearAA, numeroDaSemana);



                // Chart: vendas por semana
                // add columns (anos + acumulado + objetivo)
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = _localizer["Dia da Semana"],
                };
                viewModel.VendasSemanaisColumnsList.Add(lbl);
                var anoAA_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = yearAA.ToString(),
                };
                viewModel.VendasSemanaisColumnsList.Add(anoAA_col);
                var ano_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = year.ToString(),
                };
                viewModel.VendasSemanaisColumnsList.Add(ano_col);
                var obj_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Objetivo"],
                };
                viewModel.VendasSemanaisColumnsList.Add(obj_col);



                // ler vendas da db
                var responseSemana = await _mediator.Send(new GetAllVendasSemanaisCachedQuery() );
                if (!responseSemana.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - LoadChartVendasSemana - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                    return new ObjectResult(new { status = "error" });
                }
                //Todas as vendasSemanais num ano e no anterior
                var responseList = _mapper.Map<List<VendaSemanalViewModel>>(responseSemana.Data);
                
                
                
                var vendasWeekList = responseList.Where(vs => vs.Ano == year).ToList();
                var vendasWeekListAA = responseList.Where(vs => vs.Ano == yearAA).ToList();



                // filtrar por loja/grupoloja/empresa/mercado se necessário
                vendasWeekList = GetFilteredVendasSemanais(vendasWeekList.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();
                vendasWeekListAA = GetFilteredVendasSemanais(vendasWeekListAA.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();



                // remover nulls se existirem
                vendasWeekList.RemoveAll(vs => vs == null);
                vendasWeekListAA.RemoveAll(vs => vs == null);



                // add rows (Semanas do ano)
                for (var nWeek = 1; nWeek <= 53; nWeek++)
                {
                    // todas as vendas semanais de uma semana 
                    var totalVendasSemanaisDaSemana = vendasWeekList.Where(vs => vs.NumeroDaSemana == nWeek).ToList();
                    var totalVendasSemanaisDaSemanaAA = vendasWeekListAA.Where(vs => vs.NumeroDaSemana == nWeek).ToList();



                    var chartRow = new ChartBarVendasRow();
                    chartRow.Label = nWeek.ToString();
                    chartRow.ValuesList = new List<double>();


                    // calcular valores (anoAA, ano, objetivo)
                    var valAnoAA = Math.Round(totalVendasSemanaisDaSemanaAA.Sum(vs => vs.ValorTotalDaVenda), 2);
                    var valAno = Math.Round(totalVendasSemanaisDaSemana.Sum(vs => vs.ValorTotalDaVenda), 2);
                    var valObjetivo = Math.Round(totalVendasSemanaisDaSemana.Sum(vs => vs.ObjetivoDaVendaSemanal), 2);


                    chartRow.ValuesList.Add(valAnoAA);
                    chartRow.ValuesList.Add(valAno);
                    chartRow.ValuesList.Add(valObjetivo);

                    // adicionar row ao chart
                    viewModel.VendasSemanaisRowsList.Add(chartRow);    
                }


                return PartialView("_dashboardChartPorSemana", viewModel);
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
        /// as duas tables de vendas por semana.
        /// </summary>
        /// <returns>PartialView("_dashboardVendasSemanais", DashboardVendasSemanaViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTablesVendasSemana()
        {
            // Iniciar viewModel
            //var viewModel = new DashboardVendasSemanaViewModel();
            //viewModel.DataDashboardYear = DashBoardYear;
            //return PartialView("_dashboardVendasSemanais", viewModel);
            return PartialView("_dashboardVendasSemanais");
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das duas tabelas de vendas por semana. (DataTables)
        /// devolve a lista de vendas por semana
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendasPorSemana()
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

                DateTime dashDate = String.IsNullOrEmpty(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);



                // ler vendas da db
                var responseSemana = await _mediator.Send(new GetAllVendasSemanaisCachedQuery());
                var responseDia = await _mediator.Send(new GetAllVendasDiariasCachedQuery());
                if (!responseSemana.Succeeded || !responseDia.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - LoadChartVendasSemana - vai sair e retornar Error: Leitura Vendas Semanais da db falhou.");
                    return new ObjectResult(new { status = "error" });
                }
                //Todas as vendasSemanais num ano e no anterior
                var responseSemanaList = _mapper.Map<List<VendaSemanalViewModel>>(responseSemana.Data);
                var responseDiaList = _mapper.Map<List<VendaDiariaViewModel>>(responseDia.Data);



                var vendasWeekList = responseSemanaList.GroupBy(vs => vs.Ano).FirstOrDefault(vs => vs.Key == dashDate.Year).ToList();
                var vendasWeekListAA = responseSemanaList.GroupBy(vs => vs.Ano).FirstOrDefault(vs => vs.Key == dashDate.Year -1).ToList();
                var vendasDayList = responseDiaList.GroupBy(vd => vd.Ano).FirstOrDefault(vd => vd.Key == dashDate.Year).ToList();
                var vendasDayListAA = responseDiaList.GroupBy(vd => vd.Ano).FirstOrDefault(vd => vd.Key == dashDate.Year - 1).ToList();



                // filtrar por loja/grupoloja/empresa/mercado se necessário
                vendasWeekList = GetFilteredVendasSemanais(vendasWeekList.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();
                vendasWeekListAA = GetFilteredVendasSemanais(vendasWeekListAA.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();
                vendasDayList = GetFilteredVendasDiarias(vendasDayList.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();
                vendasDayListAA = GetFilteredVendasDiarias(vendasDayListAA.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();



                // remover nulls se existirem
                vendasWeekList.RemoveAll(vs => vs == null);
                vendasWeekListAA.RemoveAll(vs => vs == null);
                vendasDayList.RemoveAll(vd => vd == null);
                vendasDayListAA.RemoveAll(vd => vd == null);



                // lista de vendas semanais
                var allVendas = new List<VendasWeekViewModel>();
                var cal = new CultureInfo("pt-PT").Calendar;
                var numeroDaSemana = cal.GetWeekOfYear(dashDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);


                for (var nWeek = 1; nWeek <= 53; nWeek++)
                {

                    var totalVendasSemanaisDaSemana = vendasWeekList.Where(vs => vs.NumeroDaSemana == nWeek);
                    var totalVendasSemanaisDaSemanaAA = vendasWeekListAA.Where(vs => vs.NumeroDaSemana == nWeek);


                    var vendaSemanalIds = totalVendasSemanaisDaSemana.Select(vs => vs.Id).ToHashSet();
                    var vendaSemanalIdsAA = totalVendasSemanaisDaSemanaAA.Select(vs => vs.Id).ToHashSet();

                    var totalVendasDiariasDaSemana = vendasDayList.Where(vd => vendaSemanalIds.Contains(vd.VendaSemanalId)).ToList();
                    var totalVendasDiariasDaSemanaAA = vendasDayListAA.Where(vd => vendaSemanalIdsAA.Contains(vd.VendaSemanalId)).ToList();



                    var Valor = Math.Round(totalVendasDiariasDaSemana.Sum(vd => vd.ValorDaVenda), 2);
                    var Objetivo = Math.Round(totalVendasSemanaisDaSemana.Sum(vs => vs.ObjetivoDaVendaSemanal), 2);
                    var Variacao = 0.00;
                    if (Objetivo > 0)
                    {
                        Variacao = Math.Round(((Valor - Objetivo) / Objetivo) * 100, 2);
                    }



                    var vWeek = new VendasWeekViewModel
                    {
                        NumeroDaSemana = nWeek.ToString(),
                        Quantidade = totalVendasDiariasDaSemana.Sum(vd => vd.TotalArtigos),
                        Valor = Valor,
                        Objetivo = Objetivo,
                        Variacao = Variacao
                    };

                    if (vWeek.Valor > 0)
                    {
                        allVendas.Add(vWeek);
                    }
                    
                }

                //adicionar ultima fila com o total de vendas
                var totalVendas = Math.Round(allVendas.Sum(v => v.Valor), 2);
                var objetivo = Math.Round(allVendas.Sum(v => v.Objetivo), 2);

                var totalVendasAno = new VendasWeekViewModel
                {
                    NumeroDaSemana = _localizer["Total"],
                    Quantidade = allVendas.Sum(v => v.Quantidade),
                    Valor = totalVendas,
                    Objetivo = objetivo,
                    Variacao = objetivo == 0 ? 0 : Math.Round(((totalVendas - objetivo) / objetivo) * 100, 2)
                };

                allVendas.Add(totalVendasAno);

                var jsonData = new { draw = draw, recordsFiltered = allVendas.Count, recordsTotal = allVendas.Count, data = allVendas };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - GetVendasPorMes - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart de vendas por mês.
        /// </summary>
        /// <returns>PartialView("_dashboardChartPorMes", ChartMensalViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChartVendasMes()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();
                var title = Request.Form["title"].FirstOrDefault();
                var subtitle = Request.Form["subtitle"].FirstOrDefault();



                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                DateTime dateDashboard = string.IsNullOrEmpty(diafilter) ? DateTime.Now : DateTime.Parse(diafilter);



                var year = dateDashboard.Year;
                var yearAA = year - 1;



                // Iniciar viewModel
                var viewModel = new ChartMensalViewModel();
                viewModel.Title = string.IsNullOrEmpty(title) ? _localizer["Vendas por mês"] : title;
                viewModel.Subtitle = string.IsNullOrEmpty(subtitle) ? _localizer["Vendas por mês"] : subtitle;
                viewModel.VendasMensaisColumnsList = new List<ChartBarColumn>();
                viewModel.VendasMensaisRowsList = new List<ChartBarVendasRow>();



                // Chart: vendas por mes
                // add columns (anos)
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = _localizer["Mês"],
                };
                viewModel.VendasMensaisColumnsList.Add(lbl);
                var anoAA_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = yearAA.ToString(),
                };
                viewModel.VendasMensaisColumnsList.Add(anoAA_col);
                var ano_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = year.ToString(),
                };
                viewModel.VendasMensaisColumnsList.Add(ano_col);
                var obj_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Objetivo"],
                };
                viewModel.VendasMensaisColumnsList.Add(obj_col);




                // add rows (Meses)
                for (var month = 1; month <= 12; month++)
                {
                    var response = await _mediator.Send(new GetVendasDiariasMesCachedQuery() { Ano = year, Mes = month });
                    var responseAA = await _mediator.Send(new GetVendasDiariasMesCachedQuery() { Ano = year - 1, Mes = month });
                    if (response.Succeeded && responseAA.Succeeded)
                    {
                        var vendasMonthList = _mapper.Map<List<VendaDiariaViewModel>>(response.Data);
                        var vendasMonthListAA = _mapper.Map<List<VendaDiariaViewModel>>(responseAA.Data);

                        // filtrar por loja/grupoloja/empresa/mercado se necessário
                        vendasMonthList = GetFilteredVendasDiarias(vendasMonthList.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();
                        vendasMonthListAA = GetFilteredVendasDiarias(vendasMonthListAA.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();


                        // remover nulls
                        vendasMonthList.RemoveAll(vd => vd == null);
                        vendasMonthListAA.RemoveAll(vd => vd == null);


                        var chartRow = new ChartBarVendasRow();
                        chartRow.Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month).FirstCharToUpper();
                        chartRow.ValuesList = new List<double>();

                        // calcular valores
                        chartRow.ValuesList.Add(Math.Round(vendasMonthListAA.Sum(v => v.ValorDaVenda), 2));
                        chartRow.ValuesList.Add(Math.Round(vendasMonthList.Sum(v => v.ValorDaVenda), 2));
                        chartRow.ValuesList.Add(Math.Round(vendasMonthListAA.Sum(v => v.ValorDaVenda) * 1.10, 2));

                        // adicionar row ao chart
                        viewModel.VendasMensaisRowsList.Add(chartRow);
                    }
                }


                return PartialView("_dashboardChartPorMes", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - LoadChartMes - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// as duas tables de vendas por mes.
        /// </summary>
        /// <returns>PartialView("_dashboardVendasMensais", DashboardVendasMensaisViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTablesVendasMes()
        {
            // Iniciar viewModel
            var viewModel = new DashboardVendasMensaisViewModel();
            return PartialView("_dashboardVendasMensais", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das duas tabelas de vendas por mês. (DataTables)
        /// devolve a lista de vendas por mes
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendasPorMes()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();

                var dateStr = Request.Form["date"].FirstOrDefault();




                int intMercadoFilter = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int intEmpresaFilter = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int intGrupolojaFilter = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int intLojaFilter = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;

                DateTime dashDate = String.IsNullOrEmpty(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);




                // lista de vendas por mes
                var allVendas = new List<VendasMonthViewModel>();
                var year = dashDate.Year;
                var day = dashDate.Day;

                for (var month = 1; month <= 12; month++)
                {
                    var response = await _mediator.Send(new GetVendasDiariasMesCachedQuery() { Ano = year, Mes = month });
                    var responseAA = await _mediator.Send(new GetVendasDiariasMesCachedQuery() { Ano = year - 1, Mes = month });
                    if (response.Succeeded && responseAA.Succeeded)
                    {
                        var vendasMonthList = _mapper.Map<List<VendaDiariaViewModel>>(response.Data);
                        var vendasMonthListAA = _mapper.Map<List<VendaDiariaViewModel>>(responseAA.Data);

                        // filtrar por loja/grupoloja/empresa/mercado se necessário
                        vendasMonthList = GetFilteredVendasDiarias(vendasMonthList.AsQueryable(), intMercadoFilter, intEmpresaFilter, intGrupolojaFilter, intLojaFilter).ToList();
                        vendasMonthListAA = GetFilteredVendasDiarias(vendasMonthListAA.AsQueryable(), intMercadoFilter, intEmpresaFilter, intGrupolojaFilter, intLojaFilter).ToList();



                        // remover nulls
                        vendasMonthList.RemoveAll(vd => vd == null);
                        vendasMonthListAA.RemoveAll(vd => vd == null);



                        var Valor = vendasMonthList.Sum(v => v.ValorDaVenda);
                        var Objetivo = vendasMonthListAA.Sum(v => v.ValorDaVenda) * 1.10;
                        var Variacao = 0.00;
                        if (Objetivo > 0)
                        {
                            Variacao = ((Valor - Objetivo) / Objetivo) * 100;
                        }



                        var vmonth = new VendasMonthViewModel
                        {
                            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month).FirstCharToUpper(),
                            Quantidade = vendasMonthList.Sum(v => v.TotalArtigos),
                            Valor = Math.Round(Valor, 2),
                            Objetivo = Math.Round(Objetivo, 2),
                            Variacao = Math.Round(Variacao, 2)
                        };

                        if (vmonth.Valor > 0)
                        {
                            allVendas.Add(vmonth);
                        }
                    }
                }

                //adicionar ultima fila com o total de vendas
                var totalVendas = Math.Round(allVendas.Sum(v => v.Valor), 2);
                var totalObjetivo = Math.Round(allVendas.Sum(v => v.Objetivo), 2);

                var totalVendasMonth = new VendasMonthViewModel
                {
                    MonthName = _localizer["Total"],
                    Quantidade = allVendas.Sum(v => v.Quantidade),
                    Valor = totalVendas,
                    Objetivo = totalObjetivo,
                    Variacao = totalObjetivo == 0 ? 0 : Math.Round(((totalVendas - totalObjetivo) / totalObjetivo) * 100, 2)
                };

                allVendas.Add(totalVendasMonth);

                var jsonData = new { draw = draw, recordsFiltered = allVendas.Count, recordsTotal = allVendas.Count, data = allVendas };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - GetVendasPorMes - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart de vendas por trimestre.
        /// </summary>
        /// <returns>PartialView("_dashboardChartPorTrimestre", ChartTrimestreViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChartVendasTrimestre()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();
                var title = Request.Form["title"].FirstOrDefault();
                var subtitle = Request.Form["subtitle"].FirstOrDefault();



                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                DateTime dateDashboard = string.IsNullOrEmpty(diafilter) ? DateTime.Now : DateTime.Parse(diafilter);



                var year = dateDashboard.Year;
                var yearAA = year - 1;



                // Iniciar viewModel
                var viewModel = new ChartTrimestralViewModel();
                viewModel.Title = string.IsNullOrEmpty(title) ? _localizer["Vendas por trimestre"] : title;
                viewModel.Subtitle = string.IsNullOrEmpty(subtitle) ? _localizer["Vendas por trimestre"] : subtitle;
                viewModel.VendasTrimestraisColumnsList = new List<ChartBarColumn>();
                viewModel.VendasTrimestraisRowsList = new List<ChartBarVendasRow>();



                // Chart: vendas por trimestre
                // add columns (anos)
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = _localizer["Trimestre"],
                };
                viewModel.VendasTrimestraisColumnsList.Add(lbl);
                var anoAA_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = yearAA.ToString(),
                };
                viewModel.VendasTrimestraisColumnsList.Add(anoAA_col);
                var ano_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = year.ToString(),
                };
                viewModel.VendasTrimestraisColumnsList.Add(ano_col);
                var obj_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Objetivo"],
                };
                viewModel.VendasTrimestraisColumnsList.Add(obj_col);




                // add rows (Quarters)
                for (var quarter = 1; quarter <= 4; quarter++)
                {
                    var response = await _mediator.Send(new GetVendasDiariasTrimestreCachedQuery() { Ano = year, Trimestre = quarter });
                    var responseAA = await _mediator.Send(new GetVendasDiariasTrimestreCachedQuery() { Ano = year - 1, Trimestre = quarter });
                    if (response.Succeeded && responseAA.Succeeded)
                    {
                        var vendasQuarterList = _mapper.Map<List<VendaDiariaViewModel>>(response.Data);
                        var vendasQuarterListAA = _mapper.Map<List<VendaDiariaViewModel>>(responseAA.Data);

                        // filtrar por loja/grupoloja/empresa/mercado se necessário
                        vendasQuarterList = GetFilteredVendasDiarias(vendasQuarterList.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();
                        vendasQuarterListAA = GetFilteredVendasDiarias(vendasQuarterListAA.AsQueryable(), mercadoId, empresaId, grupolojaId, lojaId).ToList();



                        // remover nulls
                        vendasQuarterList.RemoveAll(vd => vd == null);
                        vendasQuarterListAA.RemoveAll(vd => vd == null);



                        var chartRow = new ChartBarVendasRow();
                        chartRow.Label = "Q" + quarter.ToString();
                        chartRow.ValuesList = new List<double>();

                        // calcular valores
                        chartRow.ValuesList.Add(Math.Round(vendasQuarterListAA.Sum(v => v.ValorDaVenda), 2));
                        chartRow.ValuesList.Add(Math.Round(vendasQuarterList.Sum(v => v.ValorDaVenda), 2));
                        chartRow.ValuesList.Add(Math.Round(vendasQuarterListAA.Sum(v => v.ValorDaVenda) * 1.10, 2));

                        // adicionar row ao chart
                        viewModel.VendasTrimestraisRowsList.Add(chartRow);
                    }
                }


                return PartialView("_dashboardChartPorTrimestre", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - LoadChartVendasTrimestre - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// as duas tables de vendas por trimestre.
        /// </summary>
        /// <returns>PartialView("_dashboardVendasTrimestre", DashboardVendasTrimestreViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public  IActionResult LoadTablesVendasTrimestre()
        {
            // Iniciar viewModel
            var viewModel = new DashboardVendasTrimestreViewModel();
            return PartialView("_dashboardVendasTrimestrais", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das duas tabelas de vendas por trimestre. (DataTables)
        /// devolve a lista de vendas por trimestre
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendasPorTrimestre()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();

                var dateStr = Request.Form["date"].FirstOrDefault();




                int intMercadoFilter = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int intEmpresaFilter = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int intGrupolojaFilter = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int intLojaFilter = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;

                DateTime dashDate = String.IsNullOrEmpty(dateStr) ? DateTime.MinValue : DateTime.Parse(dateStr);




                // lista de vendas por trimestre
                var allVendas = new List<VendasQuarterViewModel>();
                var year = dashDate.Year;
                var day = dashDate.Day;

                for (var quarter = 1; quarter <= 4; quarter++)
                {
                    var response = await _mediator.Send(new GetVendasDiariasTrimestreCachedQuery() { Ano = year, Trimestre = quarter });
                    var responseAA = await _mediator.Send(new GetVendasDiariasTrimestreCachedQuery() { Ano = year - 1, Trimestre = quarter });
                    if (response.Succeeded  && responseAA.Succeeded)
                    {
                        var vendasTrimestreList = _mapper.Map<List<VendaDiariaViewModel>>(response.Data);
                        var vendasTrimestreListAA = _mapper.Map<List<VendaDiariaViewModel>>(responseAA.Data);

                        // filtrar por loja/grupoloja/empresa/mercado se necessário
                        vendasTrimestreList = GetFilteredVendasDiarias(vendasTrimestreList.AsQueryable(), intMercadoFilter, intEmpresaFilter, intGrupolojaFilter, intLojaFilter).ToList();
                        vendasTrimestreListAA = GetFilteredVendasDiarias(vendasTrimestreListAA.AsQueryable(), intMercadoFilter, intEmpresaFilter, intGrupolojaFilter, intLojaFilter).ToList();



                        // remover nulls
                        vendasTrimestreList.RemoveAll(vd =>  vd == null);
                        vendasTrimestreListAA.RemoveAll(vd => vd == null);



                        var Valor = vendasTrimestreList.Sum(v => v.ValorDaVenda);
                        var Objetivo = vendasTrimestreListAA.Sum(v => v.ValorDaVenda) * 1.10;
                        var Variacao = 0.00;
                        if (Objetivo > 0)
                        {
                            Variacao = ((Valor - Objetivo) / Objetivo) * 100;
                        }



                        var vquarter = new VendasQuarterViewModel
                        {
                            QuarterName = "Q" + quarter.ToString(),
                            Quantidade = vendasTrimestreList.Sum(v => v.TotalArtigos),
                            Valor = Math.Round(Valor, 2),
                            Objetivo = Math.Round(Objetivo, 2),
                            Variacao = Math.Round(Variacao, 2)
                        };

                        if (vquarter.Valor > 0)
                        {
                            allVendas.Add(vquarter);
                        }
                    }
                }

                //adicionar ultima fila com o total de vendas
                var totalVendas = Math.Round(allVendas.Sum(v => v.Valor), 2);
                var totalObjetivo = Math.Round(allVendas.Sum(v => v.Objetivo), 2);

                var totalVendasQuarter = new VendasQuarterViewModel
                {
                    QuarterName = _localizer["Total"],
                    Quantidade = allVendas.Sum(v => v.Quantidade),
                    Valor = totalVendas,
                    Objetivo = totalObjetivo,
                    Variacao = totalObjetivo == 0 ? 0 : Math.Round(((totalVendas - totalObjetivo) / totalObjetivo) * 100, 2)
                };

                allVendas.Add(totalVendasQuarter);

                var jsonData = new { draw = draw, recordsFiltered = allVendas.Count, recordsTotal = allVendas.Count, data = allVendas };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Vendas Dia Controller - GetVendasPorTrimestre - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
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
        public async Task<JsonResult> LoadDataDia()
        {
            try
            {
                var mercadofilter = Request.Form["mercadofilter"].FirstOrDefault();
                var empresafilter = Request.Form["empresafilter"].FirstOrDefault();
                var grupolojafilter = Request.Form["grupolojafilter"].FirstOrDefault();
                var lojafilter = Request.Form["lojafilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();



                int mercadoId = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int empresaId = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int grupolojaId = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int lojaId = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                DateTime dateDashboard = string.IsNullOrEmpty(diafilter) ? DateTime.Now : DateTime.Parse(diafilter);



                // Iniciar viewModel
                var viewModel = new DashboardVendasGeralViewModel();
                viewModel.CurrentRole = await GetCurrentRoleAsync();



                var cInfo = new CultureInfo("pt-PT");

                viewModel.DataDashboard = dateDashboard;
                viewModel.Ano = viewModel.DataDashboard.Year;
                viewModel.Mes = viewModel.DataDashboard.Month;
                viewModel.MesLiteral = cInfo.DateTimeFormat.GetMonthName(viewModel.Mes);
                viewModel.MesLiteral = viewModel.MesLiteral.FirstCharToUpper();
                viewModel.Dia = viewModel.DataDashboard.Day;
                viewModel.NumeroDaSemana = cInfo.Calendar.GetWeekOfYear(viewModel.DataDashboard, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
                // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
                if ((viewModel.NumeroDaSemana == 1) && (viewModel.Mes == 12))
                {
                    // VendaSemanal pertence à semana 1 do ano seguinte
                    viewModel.Ano = viewModel.Ano + 1;
                }
                viewModel.AA_DataDashboard = new DateTime(viewModel.Ano - 1, viewModel.Mes, viewModel.Dia);





                // ler vendas diárias do dia da db
                var vendasDoDia = await GetVendaDiariaDiaListAsync(viewModel.CurrentRole, viewModel.Ano, viewModel.Mes, viewModel.Dia);
                vendasDoDia = GetFilteredVendasDiarias(vendasDoDia, mercadoId, empresaId, grupolojaId, lojaId);



                // ler vendas diárias do dia no ano anterior da db
                var AA_vendasDoDia = await GetVendaDiariaDiaListAsync(viewModel.CurrentRole, viewModel.Ano - 1, viewModel.Mes, viewModel.Dia);
                AA_vendasDoDia = GetFilteredVendasDiarias(AA_vendasDoDia, mercadoId, empresaId, grupolojaId, lojaId);




                // total de unidades dia/semana/mês
                viewModel.TotalUnidadesDia = vendasDoDia.Sum(vd => vd.TotalArtigos);




                // total de unidades dia/semana/mês do ano anterior
                viewModel.AA_TotalUnidadesDia = AA_vendasDoDia.Sum(vd => vd.TotalArtigos);




                // total de vendas
                viewModel.TotalVendasDia = Math.Round(vendasDoDia.Sum(vd => vd.ValorDaVenda), 2);




                // total de vendas do ano anterior
                viewModel.AA_TotalVendasDia = Math.Round(AA_vendasDoDia.Sum(vd => vd.ValorDaVenda), 2);





                // objetivos = vendas ano anterior + 10%
                viewModel.ObjetivoVendasDia = Math.Round(viewModel.AA_TotalVendasDia * 1.10, 2);




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
        /// prepara a lista de vendas diárias de um dia existentes tendo em conta 
        /// o role do user corrente.
        /// </summary>
        /// <returns></returns>

        internal async Task<IQueryable<VendaDiariaViewModel>> GetVendaDiariaDiaListAsync(CurrentRole currentRole, int ano, int mes, int dia)
        {

            var viewModel = new List<VendaDiariaViewModel>().AsQueryable();

            if (currentRole.IsSuperAdmin || currentRole.IsAdmin || currentRole.IsRevisor) // todas as vendas
            {
                var response = await _mediator.Send(new GetAllVendasDiariasCachedQuery());
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaDiariaViewModel>>(response.Data).AsQueryable();
                }
            }

            if (currentRole.IsSupervisor) // vendas de grupoloja
            {
                var response = await _mediator.Send(new GetVendasDiariasByGrupolojaIdQuery() { GrupolojaId = currentRole.GrupolojaId });
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaDiariaViewModel>>(response.Data).AsQueryable();
                }
            }

            if (currentRole.IsGerenteLoja || currentRole.IsColaborador || currentRole.IsBasic) // vendas de loja
            {
                var response = await _mediator.Send(new GetVendasDiariasByLojaIdQuery() { LojaId = currentRole.LojaId });
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaDiariaViewModel>>(response.Data).AsQueryable();
                }
            }

            viewModel = viewModel.Where(vs => vs.Ano == ano && vs.Mês == mes && vs.DiaDoMês == dia);

            return viewModel;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara a lista de vendas diárias de um mês existentes tendo em conta 
        /// o role do user corrente.
        /// </summary>
        /// <returns></returns>

        internal async Task<IQueryable<VendaDiariaViewModel>> GetVendaDiariaMesListAsync(CurrentRole currentRole, int ano, int mes)
        {

            var viewModel = new List<VendaDiariaViewModel>().AsQueryable();

            if (currentRole.IsSuperAdmin || currentRole.IsAdmin || currentRole.IsRevisor) // todas as vendas
            {
                var response = await _mediator.Send(new GetAllVendasDiariasCachedQuery());
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaDiariaViewModel>>(response.Data).AsQueryable();
                }
            }

            if (currentRole.IsSupervisor) // vendas de grupoloja
            {
                var response = await _mediator.Send(new GetVendasDiariasByGrupolojaIdQuery() { GrupolojaId = currentRole.GrupolojaId });
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaDiariaViewModel>>(response.Data).AsQueryable();
                }
            }

            if (currentRole.IsGerenteLoja || currentRole.IsColaborador || currentRole.IsBasic) // vendas de loja
            {
                var response = await _mediator.Send(new GetVendasDiariasByLojaIdQuery() { LojaId = currentRole.LojaId });
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaDiariaViewModel>>(response.Data).AsQueryable();
                }
            }

            viewModel = viewModel.Where(vs => vs.Ano == ano && vs.Mês == mes);

            return viewModel;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara a lista de vendas semanais de uma semana existentes tendo em conta 
        /// o role do user corrente.
        /// </summary>
        /// <returns></returns>

        internal async Task<IQueryable<VendaSemanalViewModel>> GetVendaSemanalDiaListAsync(CurrentRole currentRole, int ano, int numeroDaSemana)
        {

            var viewModel = new List<VendaSemanalViewModel>().AsQueryable();

            if (currentRole.IsSuperAdmin || currentRole.IsAdmin || currentRole.IsRevisor) // todas as vendas
            {
                var response = await _mediator.Send(new GetAllVendasSemanaisCachedQuery());
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaSemanalViewModel>>(response.Data).AsQueryable();
                }
            }

            if (currentRole.IsSupervisor) // vendas de grupoloja
            {
                var response = await _mediator.Send(new GetVendasSemanaisByGrupolojaIdQuery() { GrupolojaId = currentRole.GrupolojaId });
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaSemanalViewModel>>(response.Data).AsQueryable();
                }
            }

            if (currentRole.IsGerenteLoja || currentRole.IsColaborador || currentRole.IsBasic) // vendas de loja
            {
                var response = await _mediator.Send(new GetVendasSemanaisByLojaIdQuery() { LojaId = currentRole.LojaId });
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<VendaSemanalViewModel>>(response.Data).AsQueryable();
                }
            }

            viewModel = viewModel.Where(vs => vs.Ano == ano && vs.NumeroDaSemana == numeroDaSemana);

            return viewModel;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// lê da db as vendasDiarias de uma semana
        /// </summary>
        /// <returns>List<VendaDiariaViewModel></returns>

        internal async Task<List<VendaDiariaViewModel>> GetVendasDiariasAsync(int vendaSemanalId)
        {
            var response = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vendaSemanalId });
            if (!response.Succeeded)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | DashboardVendasDia Contoller - GetVendasDiariasAsync - exception vai sair e retornar Error: " + response.Message);
                return new List<VendaDiariaViewModel>();
            }

            return _mapper.Map<List<VendaDiariaViewModel>>(response.Data);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// filtra uma lista de vendas semanais por
        /// lojaId/grupolojaId/empresaId/mercadoId
        /// </summary>
        /// <returns></returns>

        internal IQueryable<VendaSemanalViewModel> GetFilteredVendasSemanais(IQueryable<VendaSemanalViewModel> vendasSemanaisList, int mercadoId, int empresaId, int grupolojaId, int lojaId)
        {
            if (lojaId > 0) return vendasSemanaisList.Where(vs => vs.LojaId == lojaId);
            if (grupolojaId > 0) return vendasSemanaisList.Where(vs => vs.GrupolojaId == grupolojaId);
            if (empresaId > 0) return vendasSemanaisList.Where(vs => vs.EmpresaId == empresaId);
            if (mercadoId > 0) return vendasSemanaisList.Where(vs => vs.MercadoId == mercadoId);

            return vendasSemanaisList;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// filtra uma lista de vendas diárias por
        /// lojaId/grupolojaId/empresaId/mercadoId
        /// </summary>
        /// <returns></returns>

        internal IQueryable<VendaDiariaViewModel> GetFilteredVendasDiarias(IQueryable<VendaDiariaViewModel> vendasDiariasList, int mercadoId, int empresaId, int grupolojaId, int lojaId)
        {
            if (lojaId > 0) return vendasDiariasList.Where(vs => vs.LojaId == lojaId);
            if (grupolojaId > 0) return vendasDiariasList.Where(vs => vs.GrupolojaId == grupolojaId);
            if (empresaId > 0) return vendasDiariasList.Where(vs => vs.EmpresaId == empresaId);
            if (mercadoId > 0) return vendasDiariasList.Where(vs => vs.MercadoId == mercadoId);

            return vendasDiariasList;
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
