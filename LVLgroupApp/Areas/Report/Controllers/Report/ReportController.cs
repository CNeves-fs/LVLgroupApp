using Core.Constants;
using Core.Entities.Identity;
using Core.Enums;
using Core.Features.Report.Commands.Create;
using Core.Features.Report.Commands.Delete;
using Core.Features.Report.Commands.Update;
using Core.Features.Report.Queries.GetAllCached;
using Core.Features.Report.Queries.GetById;
using Core.Features.ReportTemplate.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Report.Controllers.ReportType;
using LVLgroupApp.Areas.Report.Models.Report;
using LVLgroupApp.Areas.Report.Models.ReportTemplate;
using LVLgroupApp.Areas.ReportTemplate.Controllers.ReportTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Report.Controllers.Report
{
    [Area("Report")]
    [Authorize]
    public class ReportController : BaseController<ReportController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<ReportController> _localizer;

        private readonly SignInManager<ApplicationUser> _signInManager;


        //---------------------------------------------------------------------------------------------------


        public ReportController(IStringLocalizer<ReportController> localizer, SignInManager<ApplicationUser> signInManager)
        {
            _localizer = localizer;
            _signInManager = signInManager;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Report.View)]
        public IActionResult Index()
        {
            var model = new ReportViewModel();
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Report Contoller - Index - return viewModel");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Report.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Report Contoller - LoadAll - return _ViewAll");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Report.View)]
        public async Task<IActionResult> GetReports()
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
                var activefilter = Request.Form["activeFilter"].FirstOrDefault();
                var versionFilter = Request.Form["versionFilter"].FirstOrDefault();
                var typeFilter = Request.Form["typeFilter"].FirstOrDefault();




                int intFilterMercado = mercadofilter != null ? Convert.ToInt32(mercadofilter) : 0;
                int intFilterEmpresa = empresafilter != null ? Convert.ToInt32(empresafilter) : 0;
                int intFilterGrupoloja = grupolojafilter != null ? Convert.ToInt32(grupolojafilter) : 0;
                int intFilterLoja = lojafilter != null ? Convert.ToInt32(lojafilter) : 0;
                bool isFilterActive = activefilter != null ? Convert.ToBoolean(activefilter) : false;                
                int intVersionFilter = versionFilter != null ? Convert.ToInt32(versionFilter) : 0;               
                int intTypeFilter = typeFilter != null ? Convert.ToInt32(typeFilter) : 0;




                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;





                // lista de Reports permitidas ao current user
                var allReports = await GetReportListAsync();

                // filtrar por mercado se necessário
                if (intFilterMercado > 0)
                {
                    allReports = allReports.Where(v => v.MercadoId == intFilterMercado);
                }

                // filtrar por empresa se necessário
                if (intFilterEmpresa > 0)
                {
                    allReports = allReports.Where(v => v.EmpresaId == intFilterEmpresa);
                }

                // filtrar por grupoloja se necessário
                if (intFilterGrupoloja > 0)
                {
                    allReports = allReports.Where(v => v.GrupolojaId == intFilterGrupoloja);
                }

                // filtrar por loja se necessário
                if (intFilterLoja > 0)
                {
                    allReports = allReports.Where(v => v.LojaId == intFilterLoja);
                }

                // filtrar por type se necessário
                if (intTypeFilter > 0)
                {
                    allReports = allReports.Where(q => q.ReportTypeId == intTypeFilter);
                }

                // filtrar por active se necessário
                if (isFilterActive)
                {
                    allReports = allReports.Where(q => q.ReportTemplateIsActive == true);
                }

                // filtrar por version se necessário
                if (intVersionFilter > 0)
                {
                    allReports = allReports.Where(q => q.ReportTemplateVersion == intVersionFilter);
                }
                

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    allReports = allReports.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allReports = allReports.Where(q => q.ReportTemplateName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                       q.ReportTypeName.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                       q.EmailAutor.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                       q.NomeLoja.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                }


                recordsTotal = allReports.Count();
                var data = allReports.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Report Contoller - GetReports - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Report.Edit)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            //criar modelView para retornar
            var reportViewModel =  new ReportViewModel();
            reportViewModel.CurrentRole = await GetCurrentRoleAsync();
            var culture = _culture.RequestCulture.Culture;

            if (id > 0) // editar Report
            {

                reportViewModel.EditMode = true;

                var response = await _mediator.Send(new GetReportByIdQuery() { Id = id });
                reportViewModel = _mapper.Map<ReportViewModel>(response.Data);

                //construir All Template types para Select dropbox
                var reportTypeId = await ReportTemplateController.GetReportTypeIdAsync(reportViewModel.ReportTemplateId, _mediator, _mapper);
                reportViewModel.ReportTemplateTypes = await ReportTypeController.GetSelectListAllReportTypesAsync(reportViewModel.Language, reportTypeId, _mapper, _mediator);


            }
            else // criar Report
            {
                reportViewModel.EditMode = false;

                // ReportDate
                reportViewModel.ReportDate = DateTime.Now;

                // User email
                reportViewModel.EmailAutor = reportViewModel.CurrentRole.Email;

                // IncluirWeather por default
                reportViewModel.IncluirWeather = true;

                //Language
                reportViewModel.Language = culture.Name;

                //Loja
                reportViewModel.MercadoId = reportViewModel.CurrentRole.MercadoId;
                reportViewModel.EmpresaId = reportViewModel.CurrentRole.EmpresaId;
                reportViewModel.GrupolojaId = reportViewModel.CurrentRole.GrupolojaId;
                reportViewModel.LojaId = reportViewModel.CurrentRole.LojaId;
            }
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", reportViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Report.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, ReportViewModel reportTemplate)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new Report
                    var createReportCommand = _mapper.Map<CreateReportCommand>(reportTemplate);
                    var result = await _mediator.Send(createReportCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Relatório com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);

                    // guardar as respostas do relatório








                }
                else
                {
                    //update Report
                    var updateReportCommand = _mapper.Map<UpdateReportCommand>(reportTemplate);
                    var result = await _mediator.Send(updateReportCommand);
                    if (result.Succeeded)
                    {
                        _notify.Information($"{_localizer["Loja com ID"]} {result.Data} {_localizer[" atualizada."]}");

                        // atualizar as respostas do relatório








                    }
                    else _notify.Error(result.Message);
                }

                // return _ViewAll
                var response = await _mediator.Send(new GetAllReportCachedQuery());
                var viewModel = _mapper.Map<List<ReportViewModel>>(response.Data);
 
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", reportTemplate);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Report.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteReportCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Modelo de pergunta com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllReportCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<ReportViewModel>>(response.Data);
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


        /// <summary>
        /// prepara a lista de reports existentes tendo em conta 
        /// o role do user corrente.
        /// a tabela de reports é carregada com esta lista em _ViewAll
        /// </summary>
        /// <returns></returns>

        internal async Task<IQueryable<ReportListViewModel>> GetReportListAsync()
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

            var viewModelList = new List<ReportListViewModel>().AsQueryable();

            var response = await _mediator.Send(new GetAllReportCachedQuery());
            if (!response.Succeeded || response.Data == null)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Report Contoller - GetReportListAsync - Erro ao ler All Reports da db Error: " + response.Message);
                return viewModelList;
            }


            if (isSuperAdmin || isAdmin || isRevisor) // todas os reports
            {
                viewModelList = _mapper.Map<List<ReportListViewModel>>(response.Data).AsQueryable();
            }

            if (isSupervisor) // reports de grupoloja
            {
                viewModelList = _mapper.Map<List<ReportListViewModel>>(response.Data.Where(r => r.GrupolojaId != grupolojaId)).AsQueryable();
            }

            if (isGerenteLoja || isColaborador || isBasic) // reports de loja
            {
                viewModelList = _mapper.Map<List<ReportListViewModel>>(response.Data.Where(r => r.LojaId != lojaId)).AsQueryable();
            }

            foreach (var rlvm in viewModelList)
            {
                // atualizar informação do ReportTemplate
                var responseRT = await _mediator.Send(new GetReportTemplateByIdQuery());
                if (!responseRT.Succeeded || responseRT.Data == null)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Report Contoller - GetReportListAsync - Erro ao ler Report Template da db Error: " + response.Message);
                    continue;
                }
                var rt = _mapper.Map<ReportTemplateViewModel>(responseRT.Data);

                rlvm.ReportTemplateName = rt.Name;
                rlvm.ReportTemplateVersion = rt.Version;
                rlvm.ReportTemplateIsActive = rt.IsActive;
            }

            return viewModelList;
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
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}