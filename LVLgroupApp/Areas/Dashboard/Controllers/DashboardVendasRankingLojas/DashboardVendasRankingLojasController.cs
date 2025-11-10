using Azure;
using Core.Constants;
using Core.Entities.Charts;
using Core.Entities.Identity;
using Core.Entities.Vendas;
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
using LVLgroupApp.Areas.Dashboard.Models.DashboardVendasGeral;
using LVLgroupApp.Areas.Dashboard.Models.DashboardVendasRankingLojas;
using LVLgroupApp.Areas.Dashboard.Models.DashboardVendasTops;
using LVLgroupApp.Areas.Vendas.Controllers;
using LVLgroupApp.Areas.Vendas.Models.VendaDiaria;
using LVLgroupApp.Areas.Vendas.Models.VendaSemanal;
using Microsoft.AspNet.SignalR.Hosting;
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


namespace LVLgroupApp.Areas.Dashboard.Controllers.DashboardVendasRankingLojas
{

    [Area("Dashboard")]
    [Authorize]
    public class DashboardVendasRankingLojasController : BaseController<DashboardVendasRankingLojasController>
    {

        //---------------------------------------------------------------------------------------------------


        private IWebHostEnvironment _environment;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<DashboardVendasRankingLojasController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public DashboardVendasRankingLojasController(IWebHostEnvironment environment, 
                                        IStringLocalizer<DashboardVendasRankingLojasController> localizer,
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
            // Iniciar viewModel
            var viewModel = new DashboardVendasRankingLojasViewModel();
            var cInfo = new CultureInfo("pt-PT");

            viewModel.CurrentRole = await GetCurrentRoleAsync();
            viewModel.DataDashboard = DateTime.Now;
            viewModel.Ano = viewModel.DataDashboard.Year;
            viewModel.Mes = viewModel.DataDashboard.Month;
            viewModel.Dia = viewModel.DataDashboard.Day;
            viewModel.NumeroDaSemana = cInfo.Calendar.GetWeekOfYear(viewModel.DataDashboard, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            // verificar se o Ano da venda e o NumeroDaSemana são válidos (final do ano)
            if ((viewModel.NumeroDaSemana == 1) && (viewModel.Mes == 12))
            {
                // VendaSemanal pertence à semana 1 do ano seguinte
                viewModel.Ano = viewModel.Ano + 1;
            }


            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - Index - return view");
            return View("Index", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart LoadChartTopVendasDia.
        /// Lista de lojas ordenada por vendas de um dia
        /// </summary>
        /// <returns>PartialView("_dashboardChartTopVendasDia", ChartTopVendasDiaViewModel)</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChartTopVendasDia()
        {
            try
            {
                var anofilter = Request.Form["ano"].FirstOrDefault();
                var mesfilter = Request.Form["mes"].FirstOrDefault();
                var diafilter = Request.Form["dia"].FirstOrDefault();
                var divfilter = Request.Form["divfilter"].FirstOrDefault();



                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;
                int mesDashboard = anofilter != null ? Convert.ToInt32(mesfilter) : 0;
                int diaDashboard = anofilter != null ? Convert.ToInt32(diafilter) : 0;                




                // Iniciar viewModel
                var viewModel = new ChartTopVendaDiaViewModel();
                viewModel.Dia = diaDashboard;
                viewModel.Mes = mesDashboard;
                viewModel.Ano = anoDashboard;
                viewModel.DateDashboard = new DateTime(anoDashboard, mesDashboard, diaDashboard);
                viewModel.Title = _localizer["Vendas do dia"] + " " + viewModel.DateDashboard.ToString("d", CultureInfo.CurrentCulture);
                viewModel.Subtitle = _localizer["Total de Vendas"];
                viewModel.DivId = string.IsNullOrEmpty(divfilter) ? string.Empty : divfilter;
                viewModel.VendasDiariasColumnsList = new List<ChartBarColumn>();
                viewModel.VendasDiariasRowsList = new List<ChartBarVendasRow>();




                // todas as lojas
                var listLojas = new List<LojaViewModel>();
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasDia - vai sair e retornar Error: na leitura de lojas da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listLojas = _mapper.Map<List<LojaViewModel>>(responseAllLojas.Data);




                // todas as vendas diarias do dia
                var listVendasDiarias = new List<VendaDiariaViewModel>();
                var responseAllVendasDiarias = await _mediator.Send(new GetVendasDiariasDiaCachedQuery() { Ano = anoDashboard, Mes = mesDashboard, Dia = diaDashboard });
                if (!responseAllVendasDiarias.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasDia - vai sair e retornar Error: na leitura de vendas diarias de um dia da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasDiarias = _mapper.Map<List<VendaDiariaViewModel>>(responseAllVendasDiarias.Data);




                // remover nulls se existirem
                listLojas.RemoveAll(vs => vs == null);
                listVendasDiarias.RemoveAll(vd => vd == null);




                // Chart: vendas por loja
                // add columns (loja + objetivo)
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = _localizer["Loja"],
                };
                viewModel.VendasDiariasColumnsList.Add(lbl);
                var venda_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Total Vendas"],
                    //ColumnName = viewModel.DateDashboard.ToString("d", CultureInfo.CurrentCulture),
                };
                viewModel.VendasDiariasColumnsList.Add(venda_col);




                // add rows (vendas por loja)
                foreach (var lj in listLojas)
                {
                    // a venda diaria da loja 
                    var vd = listVendasDiarias.Where(v => v.LojaId == lj.Id).FirstOrDefault();
                    var valVenda = 0.00;
                    if (vd != null)
                    {
                        // existe venda diaria para a loja
                        valVenda = Math.Round(vd.ValorDaVenda, 2);
                    }
                    

                    var chartRow = new ChartBarVendasRow();
                    //chartRow.Label = string.Join(" ", lj.Nome.Split().Skip(1));
                    chartRow.Label = lj.Nome;
                    chartRow.ValuesList = new List<double>();
                    chartRow.ValuesList.Add(valVenda);


                    // adicionar row ao chart
                    viewModel.VendasDiariasRowsList.Add(chartRow);
                }

                // ordenar lista
                viewModel.VendasDiariasRowsList = viewModel.VendasDiariasRowsList.OrderByDescending(v => v.ValuesList[0]).ToList();

                return PartialView("_dashboardChartTopVendasDia", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasDia - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// a table de apoio ao chart TopVendasDia.
        /// </summary>
        /// <returns>PartialView("_dashboardTabelaTopVendasDia", TableTopVendasDiaViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTabelaTopVendasDia(int dia, int mes, int ano)
        {
            // Iniciar viewModel
            var viewModel = new TableTopVendasDiaViewModel();
            viewModel.Dia = dia;
            viewModel.Mes = mes;
            viewModel.Ano = ano;
            return PartialView("_dashboardTabelaTopVendasDia", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das tabelas de vendas do dia. (DataTables)
        /// devolve a lista de vendas do dia de todas as lojas
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendasTabelaTopVendasDia()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var anofilter = Request.Form["anofilter"].FirstOrDefault();
                var mesfilter = Request.Form["mesfilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();




                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;
                int mesDashboard = anofilter != null ? Convert.ToInt32(mesfilter) : 0;
                int diaDashboard = anofilter != null ? Convert.ToInt32(diafilter) : 0;




                // Iniciar viewModel
                var viewModel = new List<ChartTopVendaDiaLojaViewModel>();




                // todas as lojas
                var listLojas = new List<LojaViewModel>();
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasDia - vai sair e retornar Error: na leitura de lojas da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listLojas = _mapper.Map<List<LojaViewModel>>(responseAllLojas.Data);




                // todas as vendas diarias do dia
                var listVendasDiarias = new List<VendaDiariaViewModel>();
                var responseAllVendasDiarias = await _mediator.Send(new GetVendasDiariasDiaCachedQuery() { Ano = anoDashboard, Mes = mesDashboard, Dia = diaDashboard });
                if (!responseAllVendasDiarias.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasDia - vai sair e retornar Error: na leitura de vendas diarias de um dia da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasDiarias = _mapper.Map<List<VendaDiariaViewModel>>(responseAllVendasDiarias.Data);




                // remover nulls se existirem
                listLojas.RemoveAll(vs => vs == null);
                listVendasDiarias.RemoveAll(vd => vd == null);




                // add rows (vendas por loja do ano)
                foreach (var lj in listLojas)
                {
                    // criar registo para a loja desta venda
                    var vendaLoja = new ChartTopVendaDiaLojaViewModel();
                    vendaLoja.LojaNome = lj.Nome;
                    vendaLoja.Quantidade = listVendasDiarias.Where(v => v.LojaId == lj.Id).Sum(v => v.TotalArtigos);
                    vendaLoja.Valor = listVendasDiarias.Where(v => v.LojaId == lj.Id).Sum(v => v.ValorDaVenda);

                    viewModel.Add(vendaLoja);
                }



                // ordenar lista
                viewModel = viewModel.OrderByDescending(v => v.Valor).ToList();



                // atualizer index
                for (var i = 1; i <= viewModel.Count; i++)
                {
                    viewModel[i - 1].Index = i.ToString();
                }



                //adicionar ultima fila com o total de vendas
                var totalVendasDia = new ChartTopVendaDiaLojaViewModel
                {
                    Index = _localizer["Total"],
                    LojaNome = _localizer["LVL group"],
                    Quantidade = viewModel.Sum(v => v.Quantidade),
                    Valor = Math.Round(viewModel.Sum(v => v.Valor), 2)
                };

                viewModel.Add(totalVendasDia);

                var jsonData = new { draw = draw, recordsFiltered = viewModel.Count, recordsTotal = viewModel.Count, data = viewModel };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopVendasDia - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart LoadChartTopVendasSemana.
        /// Lista de lojas ordenada por vendas de uma semana
        /// </summary>
        /// <returns>PartialView("_dashboardChartTopVendasSemana", ChartTopVendasSemanaViewModel)</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChartTopVendasSemana()
        {
            try
            {
                var anofilter = Request.Form["ano"].FirstOrDefault();
                var mesfilter = Request.Form["mes"].FirstOrDefault();
                var diafilter = Request.Form["dia"].FirstOrDefault();
                var divfilter = Request.Form["divfilter"].FirstOrDefault();



                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;
                int mesDashboard = anofilter != null ? Convert.ToInt32(mesfilter) : 0;
                int diaDashboard = anofilter != null ? Convert.ToInt32(diafilter) : 0;




                // Iniciar viewModel
                var viewModel = new ChartTopVendasSemanaViewModel();
                viewModel.Dia = diaDashboard;
                viewModel.Mes = mesDashboard;
                viewModel.Ano = anoDashboard;
                viewModel.Semana = VendaSemanalController.GetNumeroDaSemana(anoDashboard, mesDashboard, diaDashboard);
                viewModel.DateDashboard = new DateTime(anoDashboard, mesDashboard, diaDashboard);
                viewModel.Title = _localizer["Vendas da semana"] + "    " + viewModel.Semana.ToString() + " / " + viewModel.Ano.ToString();
                viewModel.Subtitle = _localizer["Total de Vendas"];
                viewModel.DivId = string.IsNullOrEmpty(divfilter) ? string.Empty : divfilter;
                viewModel.VendasSemanaisColumnsList = new List<ChartBarColumn>();
                viewModel.VendasSemanaisRowsList = new List<ChartBarVendasRow>();




                // todas as lojas
                var listLojas = new List<LojaViewModel>();
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasSemana - vai sair e retornar Error: na leitura de lojas da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listLojas = _mapper.Map<List<LojaViewModel>>(responseAllLojas.Data);




                // todas as vendas diarias do dia
                var listVendasDiarias = new List<VendaDiariaViewModel>();
                var responseAllVendasDiarias = await _mediator.Send(new GetVendasDiariasDiaCachedQuery() { Ano = anoDashboard, Mes = mesDashboard, Dia = diaDashboard });
                if (!responseAllVendasDiarias.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasSemana - vai sair e retornar Error: na leitura de vendas diarias de um dia da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasDiarias = _mapper.Map<List<VendaDiariaViewModel>>(responseAllVendasDiarias.Data);




                // todas as vendas semanais da semana
                var listVendasSemanais = new List<VendaSemanalViewModel>();
                var responseAllVendasSemanais = await _mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = anoDashboard, NumeroDaSemana = viewModel.Semana });
                if (!responseAllVendasSemanais.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasSemana - vai sair e retornar Error: na leitura de vendas semanais de uma semana da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseAllVendasSemanais.Data);




                // remover nulls se existirem
                listLojas.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vd => vd == null);




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
                    ColumnName = _localizer["Total Vendas"],
                    //ColumnName = viewModel.DateDashboard.ToString("d", CultureInfo.CurrentCulture),
                };
                viewModel.VendasSemanaisColumnsList.Add(venda_col);




                // add rows (vendas por loja)
                foreach (var lj in listLojas)
                {
                    // a venda diaria da loja 
                    var vd = listVendasSemanais.Where(v => v.LojaId == lj.Id).FirstOrDefault();
                    var valVenda = 0.00;
                    if (vd != null)
                    {
                        // existe venda diaria para a loja
                        valVenda = Math.Round(vd.ValorTotalDaVenda, 2);
                    }


                    var chartRow = new ChartBarVendasRow();
                    //chartRow.Label = string.Join(" ", lj.Nome.Split().Skip(1));
                    chartRow.Label = lj.Nome;
                    chartRow.ValuesList = new List<double>();
                    chartRow.ValuesList.Add(valVenda);


                    // adicionar row ao chart
                    viewModel.VendasSemanaisRowsList.Add(chartRow);
                }

                // ordenar lista
                viewModel.VendasSemanaisRowsList = viewModel.VendasSemanaisRowsList.OrderByDescending(v => v.ValuesList[0]).ToList();

                return PartialView("_dashboardChartTopVendasSemana", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasSemana - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// a table de apoio ao chart TopVendasSemana.
        /// </summary>
        /// <returns>PartialView("_dashboardTabelaTopVendasSemana", TableTopVendasSemanaViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTabelaTopVendasSemana(int dia, int mes, int ano)
        {
            // Iniciar viewModel
            var viewModel = new TableTopVendasSemanaViewModel();
            viewModel.Dia = dia;
            viewModel.Mes = mes;
            viewModel.Ano = ano;
            return PartialView("_dashboardTabelaTopVendasSemana", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das tabelas de vendas do dia. (DataTables)
        /// devolve a lista de vendas do dia de todas as lojas
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendasTabelaTopVendasSemana()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var anofilter = Request.Form["anofilter"].FirstOrDefault();
                var mesfilter = Request.Form["mesfilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();




                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;
                int mesDashboard = anofilter != null ? Convert.ToInt32(mesfilter) : 0;
                int diaDashboard = anofilter != null ? Convert.ToInt32(diafilter) : 0;
                int semanaDashboard = VendaSemanalController.GetNumeroDaSemana(anoDashboard, mesDashboard, diaDashboard);




                // Iniciar viewModel
                var viewModel = new List<ChartTopVendaSemanalLojaViewModel>();




                // todas as lojas
                var listLojas = new List<LojaViewModel>();
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopVendasSemana - vai sair e retornar Error: na leitura de lojas da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listLojas = _mapper.Map<List<LojaViewModel>>(responseAllLojas.Data);




                // todas as vendas semanais da semana
                var listVendasSemanais = new List<VendaSemanalViewModel>();
                var responseAllVendasSemanais = await _mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = anoDashboard, NumeroDaSemana = semanaDashboard});
                if (!responseAllVendasSemanais.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopVendasSemana - vai sair e retornar Error: na leitura de vendas semanais de uma semana da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseAllVendasSemanais.Data);




                // remover nulls se existirem
                listLojas.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vd => vd == null);




                // add rows (vendas por loja do ano)
                foreach (var lj in listLojas)
                {
                    // criar registo para a loja desta venda
                    var vendaLoja = new ChartTopVendaSemanalLojaViewModel();
                    vendaLoja.LojaNome = lj.Nome;

                    // criar lista de vendas diarias da loja
                    var listVendasDiarias = new List<VendaDiariaViewModel>();
                    var vs = listVendasSemanais.Where(v => v.LojaId == lj.Id).FirstOrDefault();
                    if (vs != null)
                    {
                        // existe venda semanal para a loja
                        var responseVendasDiariasLoja = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vs.Id });
                        if (responseVendasDiariasLoja.Succeeded)
                        {
                            listVendasDiarias = _mapper.Map<List<VendaDiariaViewModel>>(responseVendasDiariasLoja.Data);
                            listVendasDiarias.RemoveAll(vd => vd == null);
                        }

                        vendaLoja.Quantidade = listVendasDiarias.Sum(v => v.TotalArtigos);
                        vendaLoja.Valor = Math.Round(vs.ValorTotalDaVenda, 2);
                    }
                    viewModel.Add(vendaLoja);
                }



                // ordenar lista
                viewModel = viewModel.OrderByDescending(v => v.Valor).ToList();




                // atualizer index
                for (var i = 1; i <= viewModel.Count; i++)
                {
                    viewModel[i - 1].Index = i.ToString();
                }




                //adicionar ultima fila com o total de vendas
                var totalVendasDia = new ChartTopVendaSemanalLojaViewModel
                {
                    Index = _localizer["Total"],
                    LojaNome = _localizer["LVL group"],
                    Quantidade = viewModel.Sum(v => v.Quantidade),
                    Valor = Math.Round(viewModel.Sum(v => v.Valor), 2)
                };

                viewModel.Add(totalVendasDia);




                var jsonData = new { draw = draw, recordsFiltered = viewModel.Count, recordsTotal = viewModel.Count, data = viewModel };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopVendasDia - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart TopObjetivoSemanal.
        /// Lista de lojas ordenada por vendas de uma semana
        /// </summary>
        /// <returns>PartialView("_dashboardChartTopObjetivoSemanal", ChartTopObjetivoSemanalViewModel)</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChartTopObjetivoSemanal()
        {
            try
            {
                var anofilter = Request.Form["ano"].FirstOrDefault();
                var mesfilter = Request.Form["mes"].FirstOrDefault();
                var diafilter = Request.Form["dia"].FirstOrDefault();
                var divfilter = Request.Form["divfilter"].FirstOrDefault();



                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;
                int mesDashboard = anofilter != null ? Convert.ToInt32(mesfilter) : 0;
                int diaDashboard = anofilter != null ? Convert.ToInt32(diafilter) : 0;




                // Iniciar viewModel
                var viewModel = new ChartTopObjetivoSemanalViewModel();
                viewModel.Dia = diaDashboard;
                viewModel.Mes = mesDashboard;
                viewModel.Ano = anoDashboard;
                viewModel.Semana = VendaSemanalController.GetNumeroDaSemana(anoDashboard, mesDashboard, diaDashboard);
                viewModel.DateDashboard = new DateTime(anoDashboard, mesDashboard, diaDashboard);
                viewModel.Title = _localizer["Vendas da semana"] + "    " + viewModel.Semana.ToString() + " / " + viewModel.Ano.ToString();
                viewModel.Subtitle = _localizer["Total de Vendas"];
                viewModel.DivId = string.IsNullOrEmpty(divfilter) ? string.Empty : divfilter;
                viewModel.VendasSemanaisColumnsList = new List<ChartBarColumn>();
                viewModel.VendasSemanaisRowsList = new List<ChartBarVendasRow>();




                // todas as lojas
                var listLojas = new List<LojaViewModel>();
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopObjetivoSemanal - vai sair e retornar Error: na leitura de lojas da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listLojas = _mapper.Map<List<LojaViewModel>>(responseAllLojas.Data);




                // todas as vendas diarias do dia
                var listVendasDiarias = new List<VendaDiariaViewModel>();
                var responseAllVendasDiarias = await _mediator.Send(new GetVendasDiariasDiaCachedQuery() { Ano = anoDashboard, Mes = mesDashboard, Dia = diaDashboard });
                if (!responseAllVendasDiarias.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopObjetivoSemanal - vai sair e retornar Error: na leitura de vendas diarias de um dia da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasDiarias = _mapper.Map<List<VendaDiariaViewModel>>(responseAllVendasDiarias.Data);




                // todas as vendas semanais da semana
                var listVendasSemanais = new List<VendaSemanalViewModel>();
                var responseAllVendasSemanais = await _mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = anoDashboard, NumeroDaSemana = viewModel.Semana });
                if (!responseAllVendasSemanais.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopObjetivoSemanal - vai sair e retornar Error: na leitura de vendas semanais de uma semana da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseAllVendasSemanais.Data);




                // remover nulls se existirem
                listLojas.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vd => vd == null);




                // Chart: vendas por loja
                // add columns (loja + objetivo)
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = _localizer["Loja"],
                };
                viewModel.VendasSemanaisColumnsList.Add(lbl);
                var obj_col = new ChartBarColumn()
                {
                    ColumnType = "number",
                    ColumnName = _localizer["Objetivo semanal"],
                    //ColumnName = viewModel.DateDashboard.ToString("d", CultureInfo.CurrentCulture),
                };
                viewModel.VendasSemanaisColumnsList.Add(obj_col);




                // add rows (vendas por loja)
                foreach (var lj in listLojas)
                {
                    var chartRow = new ChartBarVendasRow();
                    //chartRow.Label = string.Join(" ", lj.Nome.Split().Skip(1));
                    chartRow.Label = lj.Nome;
                    chartRow.ValuesList = new List<double>();


                    // a venda semanal da loja 
                    var vs = listVendasSemanais.Where(v => v.LojaId == lj.Id).FirstOrDefault();
                    var valVariacao = 0.00;
                    if (vs != null )
                    {
                        // existe venda semanal para a loja
                        var valVenda = Math.Round(vs.ValorTotalDaVenda, 2);

                        if (valVenda > 0)
                        {
                            var valObjetivo = Math.Round(vs.ObjetivoDaVendaSemanal, 2);
                            valVariacao = valObjetivo == 0 ? 0 : Math.Round(((valVenda - valObjetivo) / valObjetivo) * 100, 2);

                            chartRow.ValuesList.Add(valVariacao);

                            // adicionar row ao chart
                            viewModel.VendasSemanaisRowsList.Add(chartRow);
                        } 
                    }

                }

                // ordenar lista
                viewModel.VendasSemanaisRowsList = viewModel.VendasSemanaisRowsList.OrderByDescending(v => v.ValuesList[0]).ToList();

                return PartialView("_dashboardChartTopObjetivoSemanal", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopObjetivoSemanal - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// a table de apoio ao chart TopObjetivoSemanal.
        /// </summary>
        /// <returns>PartialView("_dashboardTabelaTopObjetivoSemanal", TableTopObjetivoSemanalViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTabelaTopObjetivoSemanal(int dia, int mes, int ano)
        {
            // Iniciar viewModel
            var viewModel = new TableTopObjetivoSemanalViewModel();
            viewModel.Dia = dia;
            viewModel.Mes = mes;
            viewModel.Ano = ano;
            return PartialView("_dashboardTabelaTopObjetivoSemanal", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das tabelas de vendas do dia. (DataTables)
        /// devolve a lista de vendas do dia de todas as lojas
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendasTabelaTopObjetivoSemanal()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var anofilter = Request.Form["anofilter"].FirstOrDefault();
                var mesfilter = Request.Form["mesfilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();




                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;
                int mesDashboard = anofilter != null ? Convert.ToInt32(mesfilter) : 0;
                int diaDashboard = anofilter != null ? Convert.ToInt32(diafilter) : 0;
                int semanaDashboard = VendaSemanalController.GetNumeroDaSemana(anoDashboard, mesDashboard, diaDashboard);




                // Iniciar viewModel
                var viewModel = new List<ChartTopObjetivoSemanalLojaViewModel>();




                // todas as lojas
                var listLojas = new List<LojaViewModel>();
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopVendasSemana - vai sair e retornar Error: na leitura de lojas da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listLojas = _mapper.Map<List<LojaViewModel>>(responseAllLojas.Data);




                // todas as vendas semanais da semana
                var listVendasSemanais = new List<VendaSemanalViewModel>();
                var responseAllVendasSemanais = await _mediator.Send(new GetVendasSemanaisSemanaCachedQuery() { Ano = anoDashboard, NumeroDaSemana = semanaDashboard });
                if (!responseAllVendasSemanais.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopVendasSemana - vai sair e retornar Error: na leitura de vendas semanais de uma semana da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseAllVendasSemanais.Data);




                // remover nulls se existirem
                listLojas.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vd => vd == null);




                // add rows (vendas por loja do ano)
                foreach (var lj in listLojas)
                {
                    // criar registo para a loja desta venda
                    var vendaLoja = new ChartTopObjetivoSemanalLojaViewModel();
                    vendaLoja.LojaNome = lj.Nome;

                    // criar lista de vendas diarias da loja
                    var listVendasDiarias = new List<VendaDiariaViewModel>();
                    var vs = listVendasSemanais.Where(v => v.LojaId == lj.Id).FirstOrDefault();
                    if (vs != null)
                    {
                        // existe venda semanal para a loja
                        var responseVendasDiariasLoja = await _mediator.Send(new GetVendasDiariasByVendaSemanalIdQuery() { VendaSemanalId = vs.Id });
                        if (responseVendasDiariasLoja.Succeeded)
                        {
                            listVendasDiarias = _mapper.Map<List<VendaDiariaViewModel>>(responseVendasDiariasLoja.Data);
                            listVendasDiarias.RemoveAll(vd => vd == null);
                        }

                        vendaLoja.Quantidade = listVendasDiarias.Sum(v => v.TotalArtigos);
                        vendaLoja.Valor = Math.Round(vs.ValorTotalDaVenda, 2);
                        vendaLoja.Objetivo = Math.Round(vs.ObjetivoDaVendaSemanal, 2);
                        vendaLoja.Variacao = vendaLoja.Objetivo == 0 ? 0 : Math.Round(((vendaLoja.Valor - vendaLoja.Objetivo) / vendaLoja.Objetivo) * 100, 2);
                    }
                    viewModel.Add(vendaLoja);
                }



                // ordenar lista
                viewModel = viewModel.OrderByDescending(v => v.Variacao).ToList();




                // atualizer index
                for (var i = 1; i <= viewModel.Count; i++)
                {
                    viewModel[i - 1].Index = i.ToString();
                }




                //adicionar ultima fila com o total de vendas
                var vValor = Math.Round(viewModel.Sum(v => v.Valor));
                var vObjetivo = Math.Round(viewModel.Sum(v => v.Objetivo));

                var totalVendasSemana = new ChartTopObjetivoSemanalLojaViewModel
                {
                    Index = _localizer["Total"],
                    LojaNome = _localizer["LVL group"],
                    Quantidade = viewModel.Sum(v => v.Quantidade),
                    Valor = vValor,
                    Objetivo = vObjetivo,
                    Variacao = vObjetivo == 0 ? 0 : Math.Round(((vValor - vObjetivo) / vObjetivo) * 100, 2)
                };

                viewModel.Add(totalVendasSemana);




                var jsonData = new { draw = draw, recordsFiltered = viewModel.Count, recordsTotal = viewModel.Count, data = viewModel };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopVendasDia - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }
















        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// o chart LoadChartTopObjetivos.
        /// Lista de lojas ordenada por numero de semanas com objetivo atingido
        /// </summary>
        /// <returns>PartialView("_dashboardChartTopObjetivos", ChartTopObjetivosViewModel)</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadChartTopObjetivos()
        {
            try
            {
                var anofilter = Request.Form["ano"].FirstOrDefault();
                var mesfilter = Request.Form["mes"].FirstOrDefault();
                var diafilter = Request.Form["dia"].FirstOrDefault();
                var divfilter = Request.Form["divfilter"].FirstOrDefault();



                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;
                int mesDashboard = anofilter != null ? Convert.ToInt32(mesfilter) : 0;
                int diaDashboard = anofilter != null ? Convert.ToInt32(diafilter) : 0;




                // Iniciar viewModel
                var viewModel = new ChartTopObjetivosViewModel();
                viewModel.Dia = diaDashboard;
                viewModel.Mes = mesDashboard;
                viewModel.Ano = anoDashboard;
                viewModel.Semana = VendaSemanalController.GetNumeroDaSemana(anoDashboard, mesDashboard, diaDashboard);
                viewModel.DateDashboard = new DateTime(anoDashboard, mesDashboard, diaDashboard);
                viewModel.Title = _localizer["Vendas da semana"] + "    " + viewModel.Semana.ToString() + " / " + viewModel.Ano.ToString();
                viewModel.Subtitle = _localizer["Total de Vendas"];
                viewModel.DivId = string.IsNullOrEmpty(divfilter) ? string.Empty : divfilter;
                viewModel.VendasSemanaisColumnsList = new List<ChartBarColumn>();
                viewModel.VendasSemanaisRowsList = new List<ChartBarVendasRow>();




                // todas as lojas
                var listLojas = new List<LojaViewModel>();
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasSemana - vai sair e retornar Error: na leitura de lojas da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listLojas = _mapper.Map<List<LojaViewModel>>(responseAllLojas.Data);




                // todas as vendas semanais do Ano
                var listVendasSemanais = new List<VendaSemanalViewModel>();
                var responseAllVendasSemanais = await _mediator.Send(new GetVendasSemanaisAnoCachedQuery() { Ano = anoDashboard });
                if (!responseAllVendasSemanais.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasSemana - vai sair e retornar Error: na leitura de vendas semanais de uma semana da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseAllVendasSemanais.Data);




                // remover nulls se existirem
                listLojas.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vd => vd == null);




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
                    ColumnName = _localizer["Semanas com Objetivo Atingido"],
                    //ColumnName = viewModel.DateDashboard.ToString("d", CultureInfo.CurrentCulture),
                };
                viewModel.VendasSemanaisColumnsList.Add(venda_col);




                // add rows (vendas por loja)
                foreach (var lj in listLojas)
                {
                    // as venda semanais da loja 
                    var vsLojaList = listVendasSemanais.Where(v => v.LojaId == lj.Id).ToList();
                    var totalSemanas = vsLojaList.Where(vs => vs.ObjetivoDaVendaSemanal < vs.ValorTotalDaVenda).Count();



                    var chartRow = new ChartBarVendasRow();
                    chartRow.Label = lj.Nome;
                    chartRow.ValuesList = new List<double>();
                    chartRow.ValuesList.Add(totalSemanas);


                    // adicionar row ao chart
                    viewModel.VendasSemanaisRowsList.Add(chartRow);
                }

                // ordenar lista
                viewModel.VendasSemanaisRowsList = viewModel.VendasSemanaisRowsList.OrderByDescending(v => v.ValuesList[0]).ToList();

                return PartialView("_dashboardChartTopObjetivos", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - LoadChartTopVendasSemana - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o request do client para carregar
        /// a table de apoio ao chart TopObjetivos.
        /// </summary>
        /// <returns>PartialView("_dashboardTabelaTopObjetivos", TableTopObjetivosViewModel)</returns>

        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult LoadTabelaTopObjetivos(int dia, int mes, int ano)
        {
            // Iniciar viewModel
            var viewModel = new TableTopObjetivosViewModel();
            viewModel.Dia = dia;
            viewModel.Mes = mes;
            viewModel.Ano = ano;
            return PartialView("_dashboardTabelaTopObjetivos", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para atender os carregamentos
        /// das tabelas de vendas do dia. (DataTables)
        /// devolve a lista de vendas do dia de todas as lojas
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> GetVendasTabelaTopObjetivos()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();

                var anofilter = Request.Form["anofilter"].FirstOrDefault();
                var mesfilter = Request.Form["mesfilter"].FirstOrDefault();
                var diafilter = Request.Form["diafilter"].FirstOrDefault();




                int anoDashboard = anofilter != null ? Convert.ToInt32(anofilter) : 0;
                int mesDashboard = anofilter != null ? Convert.ToInt32(mesfilter) : 0;
                int diaDashboard = anofilter != null ? Convert.ToInt32(diafilter) : 0;
                int semanaDashboard = VendaSemanalController.GetNumeroDaSemana(anoDashboard, mesDashboard, diaDashboard);




                // Iniciar viewModel
                var viewModel = new List<ChartTopObjetivosLojaViewModel>();




                // todas as lojas
                var listLojas = new List<LojaViewModel>();
                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopObjetivos - vai sair e retornar Error: na leitura de lojas da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listLojas = _mapper.Map<List<LojaViewModel>>(responseAllLojas.Data);




                // todas as vendas semanais do ano
                var listVendasSemanais = new List<VendaSemanalViewModel>();
                var responseAllVendasSemanais = await _mediator.Send(new GetVendasSemanaisAnoCachedQuery() { Ano = anoDashboard });
                if (!responseAllVendasSemanais.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopObjetivos - vai sair e retornar Error: na leitura de vendas semanais do ano da db.");
                    return new ObjectResult(new { status = "error" });
                }
                listVendasSemanais = _mapper.Map<List<VendaSemanalViewModel>>(responseAllVendasSemanais.Data);




                // remover nulls se existirem
                listLojas.RemoveAll(vs => vs == null);
                listVendasSemanais.RemoveAll(vd => vd == null);




                // add rows (vendas por loja do ano)
                foreach (var lj in listLojas)
                {
                    // as venda semanais da loja 
                    var vsLojaList = listVendasSemanais.Where(v => v.LojaId == lj.Id).ToList();
                    var totalSemanas = vsLojaList.Where(vs => vs.ObjetivoDaVendaSemanal < vs.ValorTotalDaVenda).Count();
                    var totalVendas = vsLojaList.Sum(vs => vs.ValorTotalDaVenda);
                    var totalObjetivo = vsLojaList.Sum(vs => vs.ObjetivoDaVendaSemanal);


                    // criar registo para a loja desta venda
                    var vendaLoja = new ChartTopObjetivosLojaViewModel();
                    vendaLoja.LojaNome = lj.Nome;
                    vendaLoja.Semanas = totalSemanas;
                    vendaLoja.Variacao = totalObjetivo == 0 ? 0 : Math.Round(((totalVendas - totalObjetivo) / totalObjetivo) * 100, 2);

                    viewModel.Add(vendaLoja);
                }



                // ordenar lista
                viewModel = viewModel.OrderByDescending(v => v.Semanas).ToList();




                // atualizer index
                for (var i = 1; i <= viewModel.Count; i++)
                {
                    viewModel[i - 1].Index = i.ToString();
                }




                //adicionar ultima fila com o total de vendas
                var valorTotal = Math.Round(listVendasSemanais.Sum(v => v.ValorTotalDaVenda), 2);
                var objetivoTotal = Math.Round(listVendasSemanais.Sum(v => v.ObjetivoDaVendaSemanal), 2);
                var totalVendasDia = new ChartTopObjetivosLojaViewModel
                {
                    Index = _localizer["Total"],
                    LojaNome = _localizer["LVL group"],
                    Semanas = viewModel.Sum(v => v.Semanas),
                    Variacao = objetivoTotal == 0 ? 0 : Math.Round(((valorTotal - objetivoTotal) / objetivoTotal) * 100, 2),
                };

                viewModel.Add(totalVendasDia);




                var jsonData = new { draw = draw, recordsFiltered = viewModel.Count, recordsTotal = viewModel.Count, data = viewModel };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard VendasRankingLojas Controller - GetVendasTabelaTopVendasDia - Exception vai sair e retornar Error: " + ex.Message);
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
                appUserId = _signInManager.UserManager.GetUserId(User);
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
