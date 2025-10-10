using Core.Constants;
using Core.Entities.Charts;
using Core.Features.Charts.CountQueries.CountAllClaimsCached;
using Core.Features.Charts.CountQueries.CountClaimsStatusCached;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Empresas.Response;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Dashboard.Models.DashboardGeral;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Dashboard.Controllers.DashboardGeral
{

    [Area("Dashboard")]
    [Authorize]
    public class DashboardGeralController : BaseController<DashboardGeralController>
    {

        //---------------------------------------------------------------------------------------------------


        private IWebHostEnvironment _environment;

        private readonly IStringLocalizer<DashboardGeralController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public DashboardGeralController(IWebHostEnvironment Environment, 
                                        IStringLocalizer<DashboardGeralController> localizer)
        {
            _localizer = localizer;
            _environment = Environment;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Dashboards.View)]
        public IActionResult Index()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Geral Controller - Index - return view");
            return View("Index");
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Dashboards.View)]
        public async Task<IActionResult> LoadAll()
        {
            // Iniciar viewModel
            var viewModel = new DashboardGeralViewModel();
            viewModel.TotalClaimsList = new List<ChartPoint>();
            viewModel.StatusClaims = new ChartStatusViewModel();
            viewModel.StatusClaims.StatusColumnsList = new List<ChartBarColumn>();
            viewModel.StatusClaims.StatusRowsList = new List<ChartBarRow>();

            // Counter: total de claims
            var response_all = await _mediator.Send( new CountAllClaimsCachedQuery() );
            viewModel.TotalClaims = response_all.Data.EntityCount;

            // Counter: total de claims por fechar
            var response_porfechar = await _mediator.Send(new CountClaimsPorFecharCachedQuery());
            viewModel.ClaimsPorFechar = response_porfechar.Data.EntityCount;

            // Counter: total de claims por decidir
            var response_pordecidir = await _mediator.Send(new CountClaimsPorDecidirCachedQuery());
            viewModel.ClaimsPorDecidir = response_pordecidir.Data.EntityCount;

            // Counter: espaço livre em disco
            viewModel.FreeDiskSpace = GetFreeDiskSpace();

            // Chart: total de claims
            var response_emp = await _mediator.Send(new GetAllEmpresasCachedQuery());
            if (response_emp.Succeeded)
            {
                foreach (var emp in response_emp.Data)
                {
                    var response_cp = await _mediator.Send(
                        new CountAllClaimsByEmpresaIdCachedQuery( ) { empresaId = emp.Id } );
                    viewModel.TotalClaimsList.Add(response_cp.Data);
                }
            }

            // Chart: status das claims
            if (response_emp.Succeeded)
            {
                // add columns
                var lbl = new ChartBarColumn()
                {
                    ColumnType = "string",
                    ColumnName = "Status",
                };             
                viewModel.StatusClaims.StatusColumnsList.Add(lbl);
                foreach (var emp in response_emp.Data)
                {
                    var emp_col = new ChartBarColumn()
                    {
                        ColumnType = "number",
                        ColumnName = emp.Nome,
                    };
                    viewModel.StatusClaims.StatusColumnsList.Add(emp_col);
                }

                // add rows
                viewModel.StatusClaims.StatusRowsList.Add(await GetChartRowPendentesAsync(response_emp.Data));
                viewModel.StatusClaims.StatusRowsList.Add(await GetChartRowAguardaValidacaoAsync(response_emp.Data));
                viewModel.StatusClaims.StatusRowsList.Add(await GetChartRowAguardaOpiniaoAsync(response_emp.Data));
                viewModel.StatusClaims.StatusRowsList.Add(await GetChartRowAguardaDecisaoAsync(response_emp.Data));
                viewModel.StatusClaims.StatusRowsList.Add(await GetChartRowAceitesAsync(response_emp.Data));
                viewModel.StatusClaims.StatusRowsList.Add(await GetChartRowNaoAceitesAsync(response_emp.Data));
                //viewModel.StatusClaims.StatusRowsList.Add(await GetChartRowFechadasAsync(response_emp.Data));
            }
            return PartialView("_dashboardGeral", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        internal int GetFreeDiskSpace()
        {
            try
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                var contentPath = Path.Combine(_environment.WebRootPath, "Claims");
                if (Directory.Exists(contentPath))
                {
                    DirectoryInfo diSource = new DirectoryInfo(contentPath);
                    var letter = diSource.Root.Root.FullName;
                    foreach (DriveInfo d in allDrives)
                    {
                        if (d.IsReady && d.Name == letter)
                        {
                            decimal totalSize = d.TotalSize / (1024 * 1024);     //Mb
                            decimal freeSize = d.TotalFreeSpace / (1024 * 1024); //Mb
                            decimal percent = (freeSize / totalSize) * 100;
                            return ((int)percent);
                        }
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetFreeDiskSpace - IO Error: " + ex.Message);
                return -1;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<ChartBarRow> GetChartRowPendentesAsync(List<EmpresaCachedResponse> emps)
        {
            try
            {
                var chartRow = new ChartBarRow();
                chartRow.Label = _localizer["Pendentes em Loja"];
                chartRow.ValuesList = new List<int>();

                foreach (var emp in emps)
                {
                    var responsePendentes = await _mediator.Send(new CountClaimsPendentesCachedQuery() { empresaId = emp.Id});
                    chartRow.ValuesList.Add(responsePendentes.Data.EntityCount);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowPendentesAsync - Emp=" + emp.Nome + " Pendentes=" + responsePendentes.Data.EntityCount);
                }
                return chartRow;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowPendentesAsync - IO Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<ChartBarRow> GetChartRowAguardaValidacaoAsync(List<EmpresaCachedResponse> emps)
        {
            try
            {
                var chartRow = new ChartBarRow();
                chartRow.Label = _localizer["Aguardam Validação"];
                chartRow.ValuesList = new List<int>();

                foreach (var emp in emps)
                {
                    var responseAguardaValidacao = await _mediator.Send(new CountClaimsAguardaValidacaoCachedQuery() { empresaId = emp.Id });
                    chartRow.ValuesList.Add(responseAguardaValidacao.Data.EntityCount);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowAguardaValidacaoAsync - Emp=" + emp.Nome + " Aguarda Validação=" + responseAguardaValidacao.Data.EntityCount);
                }
                return chartRow;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowAguardaValidacaoAsync - IO Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<ChartBarRow> GetChartRowAguardaDecisaoAsync(List<EmpresaCachedResponse> emps)
        {
            try
            {
                var chartRow = new ChartBarRow();
                chartRow.Label = _localizer["Aguardam Decisão"];
                chartRow.ValuesList = new List<int>();

                foreach (var emp in emps)
                {
                    var responseAguardaDecisao = await _mediator.Send(new CountClaimsAguardaDecisaoCachedQuery() { empresaId = emp.Id });
                    chartRow.ValuesList.Add(responseAguardaDecisao.Data.EntityCount);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowAguardaDecisaoAsync - Emp=" + emp.Nome + " Aguarda Decisão=" + responseAguardaDecisao.Data.EntityCount);
                }
                return chartRow;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowAguardaDecisaoAsync - IO Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<ChartBarRow> GetChartRowAguardaOpiniaoAsync(List<EmpresaCachedResponse> emps)
        {
            try
            {
                var chartRow = new ChartBarRow();
                chartRow.Label = _localizer["Aguardam Opinião"];
                chartRow.ValuesList = new List<int>();

                foreach (var emp in emps)
                {
                    var responseAguardaOpiniao = await _mediator.Send(new CountClaimsAguardaOpiniaoCachedQuery() { empresaId = emp.Id });
                    chartRow.ValuesList.Add(responseAguardaOpiniao.Data.EntityCount);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowAguardaOpiniaoAsync - Emp=" + emp.Nome + " Aguarda Opinião=" + responseAguardaOpiniao.Data.EntityCount);
                }
                return chartRow;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowAguardaOpiniaoAsync - IO Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<ChartBarRow> GetChartRowFechadasAsync(List<EmpresaCachedResponse> emps)
        {
            try
            {
                var chartRow = new ChartBarRow();
                chartRow.Label = _localizer["Fechadas"];
                chartRow.ValuesList = new List<int>();

                foreach (var emp in emps)
                {
                    var responseFechadas = await _mediator.Send(new CountClaimsFechadasCachedQuery() { empresaId = emp.Id });
                    chartRow.ValuesList.Add(responseFechadas.Data.EntityCount);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowFechadasAsync - Emp=" + emp.Nome + " Fechadas=" + responseFechadas.Data.EntityCount);
                }
                return chartRow;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowFechadasAsync - IO Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<ChartBarRow> GetChartRowAceitesAsync(List<EmpresaCachedResponse> emps)
        {
            try
            {
                var chartRow = new ChartBarRow();
                chartRow.Label = _localizer["Aceites"];
                chartRow.ValuesList = new List<int>();

                foreach (var emp in emps)
                {
                    var responseAceites = await _mediator.Send(new CountClaimsAceiteCachedQuery() { empresaId = emp.Id });
                    chartRow.ValuesList.Add(responseAceites.Data.EntityCount);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowAceitesAsync - Emp=" + emp.Nome + " Fechadas=" + responseAceites.Data.EntityCount);
                }
                return chartRow;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowAceitesAsync - IO Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<ChartBarRow> GetChartRowNaoAceitesAsync(List<EmpresaCachedResponse> emps)
        {
            try
            {
                var chartRow = new ChartBarRow();
                chartRow.Label = _localizer["Não Aceites"];
                chartRow.ValuesList = new List<int>();

                foreach (var emp in emps)
                {
                    var responseNaoAceites = await _mediator.Send(new CountClaimsNaoAceiteCachedQuery() { empresaId = emp.Id });
                    chartRow.ValuesList.Add(responseNaoAceites.Data.EntityCount);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowNaoAceitesAsync - Emp=" + emp.Nome + " Fechadas=" + responseNaoAceites.Data.EntityCount);
                }
                return chartRow;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Dashboard Geral Contoller - GetChartRowNaoAceitesAsync - IO Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}
