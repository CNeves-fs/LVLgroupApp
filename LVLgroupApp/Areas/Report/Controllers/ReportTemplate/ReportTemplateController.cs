using AutoMapper;
using Core.Constants;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Report.Queries.GetAllCached;
using Core.Features.ReportTemplate.Commands.Create;
using Core.Features.ReportTemplate.Commands.Delete;
using Core.Features.ReportTemplate.Commands.Update;
using Core.Features.ReportTemplate.Queries.GetAllCached;
using Core.Features.ReportTemplate.Queries.GetById;
using Core.Features.ReportTemplateQuestion.Commands.Create;
using Core.Features.ReportTemplateQuestion.Commands.Delete;
using Core.Features.ReportTemplateQuestion.Queries.GetAllCached;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Models.Loja;
using LVLgroupApp.Areas.Report.Controllers.QuestionTemplate;
using LVLgroupApp.Areas.Report.Controllers.ReportType;
using LVLgroupApp.Areas.Report.Models.QuestionTemplate;
using LVLgroupApp.Areas.Report.Models.ReportTemplate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Report.Controllers.ReportTemplate
{
    [Area("Report")]
    [Authorize]
    public class ReportTemplateController : BaseController<ReportTemplateController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<ReportTemplateController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ReportTemplateController(IStringLocalizer<ReportTemplateController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportTemplate.View)]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - Index - start");

            // Culture contains the information of the requested culture
            var culture = _culture.RequestCulture.Culture;

            var model = new ReportTemplateViewModel();
            model.IsActive = false;
            model.Version = 0;
            model.ReportTypes = await ReportTypeController.GetSelectListAllReportTypesAsync(culture.Name, 0, _mapper, _mediator);

            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportTemplate.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - LoadAll - return _ViewAll");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.ReportTemplate.View)]
        public async Task<IActionResult> GetReportTemplates()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var reportType = Request.Form["reportType"].FirstOrDefault();
                var version = Request.Form["version"].FirstOrDefault();
                var isActive = Request.Form["isActive"].FirstOrDefault();


                int intReportTypeFilter = reportType != null ? Convert.ToInt32(reportType) : 0;
                int intVersionFilter = string.IsNullOrEmpty(version) ? 0 : Convert.ToInt32(version);
                bool isFilterActive = isActive != null ? Convert.ToBoolean(isActive) : false;


                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Culture contains the information of the requested culture
                var culture = _culture.RequestCulture.Culture;

                // lista de Report Templates
                var responseAllReportTemplates = await _mediator.Send(new GetAllReportTemplateCachedQuery());
                if (!responseAllReportTemplates.Succeeded) return null;
                var allReportTemplatesData = _mapper.Map<List<ReportTemplateViewModel>>(responseAllReportTemplates.Data).AsQueryable();

                // lista de Report Templates a devolver
                var allReportTemplatesList = new List<ReportTemplateListViewModel>();


                // ViewModel
                foreach (var rTemplate in allReportTemplatesData) 
                {
                    // criar novo ReportTemplateListViewModel
                    var reportTemplate = new ReportTemplateListViewModel();

                    reportTemplate.Id = rTemplate.Id;
                    reportTemplate.Name = rTemplate.Name;                    
                    reportTemplate.CreatedAt = rTemplate.CreatedAt;
                    reportTemplate.IsActive = rTemplate.IsActive;
                    reportTemplate.Version = rTemplate.Version;
                    reportTemplate.ReportTypeName = await ReportTypeController.GetReportTypeNomeAsync(rTemplate.ReportTypeId, culture.Name, _mediator, _mapper);
                    reportTemplate.NumberOfQuestions = await GetTotalQuestionsInReportTemplateAsync(rTemplate.Id);
                    reportTemplate.UsedInReports = 0;


                    // adicionar ao allReportTemplatesList
                    allReportTemplatesList.Add(reportTemplate);
                }

                var allReportTemplatesViewModel = allReportTemplatesList.AsQueryable();

                // filtrar por type se necessário
                if (intReportTypeFilter > 0)
                {
                    allReportTemplatesViewModel = allReportTemplatesViewModel.Where(rt => rt.ReportTypeId == intReportTypeFilter);
                }

                // filtrar por active se necessário
                if (isFilterActive)
                {
                    allReportTemplatesViewModel = allReportTemplatesViewModel.Where(rt => rt.IsActive == true);
                }

                // filtrar por version se necessário
                if (intVersionFilter > 0)
                {
                    allReportTemplatesViewModel = allReportTemplatesViewModel.Where(rt => rt.Version == intVersionFilter);
                }              

                // ordenar
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    allReportTemplatesViewModel = allReportTemplatesViewModel.OrderBy(sortColumn + " " + sortColumnDirection);
                }

                // search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allReportTemplatesViewModel = allReportTemplatesViewModel.Where(rt => rt.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase) || rt.ReportTypeName.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                }


                recordsTotal = allReportTemplatesViewModel.Count();
                var data = allReportTemplatesViewModel.Skip(skip).Take(pageSize).ToList();


                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - GetReportTemplates - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportTemplate.Create)]
        public async Task<IActionResult> OnGetCreate(int id = 0)
        {
            //criar ReportTemplateViewModel para edição ou criação
            var rtvm = new ReportTemplateViewModel();
            rtvm.QuestionTemplateList = new List<QuestionTemplateViewModel>();
            rtvm.QuestionTemplateInReportList = new List<ReportTemplateQuestionViewModel>();
            rtvm.ReportTypes = new SelectList(new List<SelectListItem>());
            var culture = _culture.RequestCulture.Culture;

            try
            {
                if (id > 0)
                {
                    // erro: editar ??? ReportTemplate
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreate - error: invalid reportTemplate Id > 0");
                    return RedirectToAction("Index");
                }

                // criar ReportTemplate vazio
                rtvm = await InitNewReportTemplateAsync();

                // retornar ReportTemplateViewModel
                return View("_CreateOrEdit", rtvm);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreateOrEdit - Exception: " + ex.Message);
                return RedirectToAction("Index");
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportTemplate.Edit)]
        public async Task<IActionResult> OnGetEdit(int id = 0)
        {
            //criar ReportTemplateViewModel para edição ou criação
            var rtvm = new ReportTemplateViewModel();
            rtvm.QuestionTemplateList = new List<QuestionTemplateViewModel>();
            rtvm.QuestionTemplateInReportList = new List<ReportTemplateQuestionViewModel>();
            rtvm.ReportTypes = new SelectList(new List<SelectListItem>());
            var culture = _culture.RequestCulture.Culture;

            try
            {
                if (id == 0)
                {
                    // erro: criar ??? ReportTemplate
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetEdit - error: invalid reportTemplate Id = 0");
                    return RedirectToAction("Index");
                }

                // editar ReportTemplate
                var response = await _mediator.Send(new GetReportTemplateByIdQuery() { Id = id });
                if (!response.Succeeded) return null;
                rtvm = _mapper.Map<ReportTemplateViewModel>(response.Data);

                // carregar as QuestionTemplate do Report
                var responseAllQuestionsInReport = await _mediator.Send(new GetAllReportTemplateQuestionByReportCachedQuery() { reportTemplateId = id });
                if (!responseAllQuestionsInReport.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetEdit - error ao ler as ReportTemplateQuestions do reportTemplate Id = " + id);
                    return RedirectToAction("Index");
                }

                // carregar listas
                rtvm.QuestionTemplateInReportList = _mapper.Map<List<ReportTemplateQuestionViewModel>>(responseAllQuestionsInReport.Data);
                rtvm.ReportTypes = await ReportTypeController.GetSelectListAllReportTypesAsync(culture.Name, rtvm.ReportTypeId, _mapper, _mediator);
                rtvm.QuestionTemplateList = await QuestionTemplateController.GetAllQuestionTemplatesAsync(culture.Name, _mapper, _mediator, _logger, _sessionId, _sessionName, _culture);
                rtvm.NumberOfQuestions = rtvm.QuestionTemplateInReportList.Count();

                // retornar ReportTemplateViewModel
                return View("_CreateOrEdit", rtvm);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetEdit - Exception: " + ex.Message);
                // retornar ReportTemplateViewModel
                return RedirectToAction("Index");
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.ReportTemplate.Create)]
        public async Task<IActionResult> OnPostCreate(int id, ReportTemplateViewModel reportTemplate)
        {
            try
            {
                var culture = _culture.RequestCulture.Culture;

                if (!ModelState.IsValid)
                {
                    // ModelState not valid
                    // devolver ReportTemplateViewModel para continuar edição
                    reportTemplate.ReportTypes = await ReportTypeController.GetSelectListAllReportTypesAsync(culture.Name, reportTemplate.ReportTypeId, _mapper, _mediator);

                    // retornar ReportTemplateViewModel
                    return View("_CreateOrEdit", reportTemplate);
                }

                // Model is valid

                if (id > 0)
                {
                    // erro: criar ??? ReportTemplate
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreate - error: invalid reportTemplate Id = 0");
                    return RedirectToAction("Index");
                }

                // id=0 criar novo ReportTemplate
                reportTemplate.Version = 1;                   // Version 1
                reportTemplate.IsActive = true;               // Active por default
                reportTemplate.CreatedAt = DateTime.Now;      // CreatedAt por default
                //reportTemplate.NumberOfQuestions = reportTemplate.QuestionTemplateInReportList.Count();

                //create new ReportTemplate
                var createReportTemplateCommand = _mapper.Map<CreateReportTemplateCommand>(reportTemplate);
                var result = await _mediator.Send(createReportTemplateCommand);
                if (!result.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreate - Exception: " + result.Message);
                    _notify.Error(result.Message);

                    return RedirectToAction("Index");
                }

                _notify.Success($"{_localizer["Modelo de Relatório com ID"]} {result.Data} {_localizer[" criado."]}");

                // atualizar Id
                reportTemplate.Id = result.Data;

                // salvar as questions do ReportTemplate
                var questionsInDb = await WriteToDbQuestionsFromReportTemplateAsync(reportTemplate);
                if (!questionsInDb)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreate - Erro ao criar as ReportTemplateQuestions");
                    _notify.Error($"{_localizer["Erro ao salvar as perguntas do Modelo de Relatório."]}");
                }

                // return index
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                // devolver ReportTemplateViewModel para continuar edição
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreate - Exception: " + ex.Message);
                _notify.Error($"{_localizer["Erro ao salvar o Modelo de Pergunta. Possível duplicado."]}");

                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreate - Exception vai retornar _CreateOrEdit com o Error: " + ex.Message);
                var html = await _viewRenderer.RenderViewToStringAsync("_Create", reportTemplate);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.ReportTemplate.Edit)]
        public async Task<IActionResult> OnPostEdit(int id, ReportTemplateViewModel reportTemplate)
        {
            try
            {
                var culture = _culture.RequestCulture.Culture;

                if (!ModelState.IsValid)
                {
                    // ModelState not valid
                    // devolver ReportTemplateViewModel para continuar edição
                    reportTemplate.ReportTypes = await ReportTypeController.GetSelectListAllReportTypesAsync(culture.Name, reportTemplate.ReportTypeId, _mapper, _mediator);

                    // retornar ReportTemplateViewModel
                    return View("_CreateOrEdit", reportTemplate);
                }
                else
                {
                    // Model is valid

                    if (id == 0)
                    {
                        // id=0 criar ??? novo ReportTemplate
                        _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - error: invalid reportTemplate Id = 0");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // update ReportTemplate
                        reportTemplate.IsActive = true;               // Active por default
                        reportTemplate.CreatedAt = DateTime.Now;      // CreatedAt por default

                        // verificar se existem reports baseados neste ReportTemplate
                        // se existirem não é possível alterar o ReportTemplate ou as
                        // questions associadas. Será necessário criar um novo template com uma nova versão
                        var responseAllreports = await _mediator.Send(new GetAllReportByReportTemplateIdCachedQuery() { reportTemplateId = reportTemplate.Id });
                        if (!responseAllreports.Succeeded)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - error ao ler os Reports do reportTemplate Id = " + id);
                            return RedirectToAction("Index");
                        }

                        if (responseAllreports.Data.Count() > 0)
                        {
                            // existem reports baseados neste ReportTemplate
                            _notify.Error($"{_localizer["Não é possível alterar o Modelo de Relatório com ID"]} {reportTemplate.Id}. {_localizer["Existem relatórios baseados neste modelo. Este novo modelo será salvo como uma nova versão."]}");

                            // criar novo ReportTemplate com nova versão
                            reportTemplate.Id = 0; // forçar criação
                            reportTemplate.Version += 1; // incrementar versão
                            
                            //create new ReportTemplate
                            var createReportTemplateCommand = _mapper.Map<CreateReportTemplateCommand>(reportTemplate);
                            var resultCreate = await _mediator.Send(createReportTemplateCommand);
                            if (!resultCreate.Succeeded)
                            {
                                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - Exception: " + resultCreate.Message);
                                _notify.Error(resultCreate.Message);
                                return RedirectToAction("Index");
                            }

                            // criar as questions do novo ReportTemplate
                            reportTemplate.Id = resultCreate.Data; // atualizar Id para o novo criado
                            var newQuestionsInDb = await WriteToDbQuestionsFromReportTemplateAsync(reportTemplate);
                            if (!newQuestionsInDb)
                            {
                                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - Erro ao criar as ReportTemplateQuestions");
                                _notify.Error($"{_localizer["Erro ao salvar as perguntas do Modelo de Relatório."]}");
                            }

                            // atualizar ReportTemplate original para inativo
                            var deactivateOriginalReportTemplateCommand = new UpdateReportTemplateCommand
                            {
                                Id = id,
                                Name = reportTemplate.Name,
                                ReportTypeId = reportTemplate.ReportTypeId,
                                IsActive = false,
                                Version = reportTemplate.Version - 1
                            };
                            var resultDeactivate = await _mediator.Send(deactivateOriginalReportTemplateCommand);
                            if (!resultDeactivate.Succeeded)
                            {
                                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - Exception: " + resultDeactivate.Message);
                                _notify.Error(resultDeactivate.Message);
                                return RedirectToAction("Index");
                            }
                            _notify.Success($"{_localizer["Modelo de Relatório com ID"]} {resultCreate.Data} {_localizer[" atualizado."]}");
                            return RedirectToAction("Index");
                        }


                        // não existem reports baseados neste ReportTemplate
                        // update new ReportTemplate
                        var updateReportTemplateCommand = _mapper.Map<UpdateReportTemplateCommand>(reportTemplate);
                        var result = await _mediator.Send(updateReportTemplateCommand);
                        if (!result.Succeeded)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - Exception: " + result.Message);
                            _notify.Error(result.Message);

                            return RedirectToAction("Index");
                        }

                        _notify.Success($"{_localizer["Modelo de Relatório com ID"]} {result.Data} {_localizer[" atualizado."]}");

                        // salvar as questions do ReportTemplate
                        var questionsInDb = await WriteToDbQuestionsFromReportTemplateAsync(reportTemplate);
                        if (!questionsInDb)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - Erro ao criar as ReportTemplateQuestions");
                            _notify.Error($"{_localizer["Erro ao salvar as perguntas do Modelo de Relatório."]}");
                        }

                    }

                    // return index
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // devolver ReportTemplateViewModel para continuar edição
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - Exception: " + ex.Message);
                _notify.Error($"{_localizer["Erro ao salvar o Modelo de Relatório. Possível duplicado."]}");

                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostEdit - Exception vai retornar _Edit com o Error: " + ex.Message);
                var html = await _viewRenderer.RenderViewToStringAsync("_Edit", reportTemplate);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função que atende o cancel do editar/criar Report Template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Claims.Edit)]
        public IActionResult OnPostCancel(int id = 0)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCancelAsync - Entrou para cancelar editar/criar report template");

            _notify.Information($"{_localizer["Editar/Criar Modelo de Relatório foi cancelado."]}");
            //return RedirectToAction("Index");
            return Json(new { redirectToUrl = Url.Action("Index") });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.ReportTemplate.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - Modelo de relatório inválido id: " + id);
                    _notify.Error($"{_localizer["Modelo de Relatório inválido com ID"]} {id}");
                    var html_text= await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                // verificar se o ReportTemplate está a ser usado em algum Report
                var responseAllreports = await _mediator.Send(new GetAllReportByReportTemplateIdCachedQuery() { reportTemplateId = id });
                if (!responseAllreports.Succeeded)
                {
                    _notify.Error(responseAllreports.Message);
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                if (responseAllreports.Data.Count() > 0)
                {
                    // o ReportTemplate está a ser usado em Reports
                    _notify.Error($"{_localizer["Não é possível remover o Modelo de Relatório com ID"]} {id}. {_localizer["Existem relatórios baseados neste modelo."]}");
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                // o ReportTemplate não está a ser usado em Reports
                // remover ReportTemplate
                var deleteCommand = await _mediator.Send(new DeleteReportTemplateCommand { Id = id });
                if (!deleteCommand.Succeeded)
                {
                    _notify.Error(deleteCommand.Message);
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                // remover as ReportTemplateQuestions associadas ao ReportTemplate
                var responseAllQuestionsInReport = await _mediator.Send(new GetAllReportTemplateQuestionByReportCachedQuery() { reportTemplateId = id });
                if (!responseAllQuestionsInReport.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - error ao ler as ReportTemplateQuestions do reportTemplate Id = " + id);
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }
                // remover da db todas as questions do ReportTemplate
                foreach (var rtQuestion in responseAllQuestionsInReport.Data)
                {
                    var deleteQuestionCommand = await _mediator.Send(new DeleteReportTemplateQuestionCommand { Id = rtQuestion.Id });
                    if (!deleteQuestionCommand.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - error ao remover a ReportTemplateQuestion Id = " + rtQuestion.Id);
                        var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                        return new JsonResult(new { isValid = true, html = html_text });
                    }
                }

                // sucesso
                _notify.Information($"{_localizer["Modelo de pergunta com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllReportTemplateCachedQuery());
                if (!response.Succeeded)
                {
                    _notify.Error(response.Message);
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                // reurn _viewall
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                return new JsonResult(new { isValid = true, html = html });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - Exception vai retornar _CreateOrEdit com o Error: " + ex.Message);
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                return new JsonResult(new { isValid = true, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<JsonResult> LoadReports(int reportTypeId)
        {
            if (reportTypeId > 0)
            {
                var reportTemplateResponse = await _mediator.Send(new GetReportTemplateByReportTypeIdCachedQuery() { reportTypeId = reportTypeId });

                if (reportTemplateResponse.Succeeded)
                {
                    var reportTemplateViewModel = _mapper.Map<List<ReportTemplateViewModel>>(reportTemplateResponse.Data);
                    var reportTemplateSelectList = new SelectList(reportTemplateViewModel, nameof(ReportTemplateViewModel.Id), nameof(ReportTemplateViewModel.Name), null, null);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { ReportTemplateList = reportTemplateSelectList });
                    return Json(jsonString);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara uma estrutura ReportTemplateViewModel para ser enviada
        /// ao client. evocada no atendimento GET OnGetCreate (criar Question Template).
        /// </summary>
        /// <returns> type="ReportTemplateViewModel"</returns>

        internal  async Task<ReportTemplateViewModel> InitNewReportTemplateAsync()
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - InitNewReportTemplateAsync - Entrou para criar estrutura ReportTemplateViewModel");

                var culture = _culture.RequestCulture.Culture;

                //criar modelView para retornar
                var rtvm = new ReportTemplateViewModel();

                // criar ReportTemplate vazio
                rtvm.Id = 0;                        // Id a 0 para criar novo
                rtvm.Name = string.Empty;           
                rtvm.Version = 1;                   // Version 1
                rtvm.IsActive = true;               // Active por default
                rtvm.ReportTypes = await ReportTypeController.GetSelectListAllReportTypesAsync(culture.Name, 0, _mapper, _mediator);
                rtvm.ReportTypeId = Int32.Parse(rtvm.ReportTypes.FirstOrDefault().Value);             // ReportTypeId por default
                rtvm.QuestionTemplateList = await QuestionTemplateController.GetAllQuestionTemplatesAsync(culture.Name, _mapper, _mediator, _logger, _sessionId, _sessionName, _culture);
                rtvm.CreatedAt = DateTime.Now;      // CreatedAt por default
                rtvm.NumberOfQuestions = 0;         // Não existem questions
                rtvm.UsedInReports = 0;             // Não existe em Reports

                return rtvm;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - InitNewReportTemplateAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica quantas QuestionTemplates estão a ser usadas no ReportTemplate
        /// </summary>
        /// <param name="reportTemplateId"></param>
        /// <returns> type="int"</returns>

        internal async Task<int> GetTotalQuestionsInReportTemplateAsync(int reportTemplateId)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - GetTotalQuestionsInReportTemplateAsync - Entrou para contar questions do ReportTemplate = " + reportTemplateId);

                // verificar se a QuestionTemplate está a ser usada em algum Report
                var responseAllQuestionsInReport = await _mediator.Send(new GetAllReportTemplateQuestionByReportCachedQuery() { reportTemplateId = reportTemplateId });
                if (!responseAllQuestionsInReport.Succeeded)
                {
                    return 0;
                }

                return responseAllQuestionsInReport.Data.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - GetTotalQuestionsInReportTemplateAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return 0;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// escreve na db as ReportTemplateQuestion associadas a um ReportTemplate
        /// </summary>
        /// <param name="rtvm"></param>
        /// <returns> type="bool>"</returns>

        internal async Task<bool> WriteToDbQuestionsFromReportTemplateAsync(ReportTemplateViewModel rtvm)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - WriteToDbQuestionsFromReportTemplateAsync - Entrou para colocar na db as questions do ReportTemplate = " + rtvm.Id);

                // remover questions que possam existir para este ReportTemplate
                // ler da db todas as questions do ReportTemplate
                var responseAllQuestionsInReport = await _mediator.Send(new GetAllReportTemplateQuestionByReportCachedQuery() { reportTemplateId = rtvm.Id });
                if (!responseAllQuestionsInReport.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - WriteToDbQuestionsFromReportTemplateAsync - error ao ler as ReportTemplateQuestions do reportTemplate Id = " + rtvm.Id);
                    return false;
                }

                // remover da db todas as questions do ReportTemplate
                foreach (var question in responseAllQuestionsInReport.Data)
                {
                    var deleteCommand = await _mediator.Send(new DeleteReportTemplateQuestionCommand { Id = question.Id });
                    if (!deleteCommand.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - WriteToDbQuestionsFromReportTemplateAsync - Exception ao eliminar ReportTemplateQuestion: " + deleteCommand.Message);
                        _notify.Error(deleteCommand.Message);
                    }
                }


                //status
                var statusOk = true;

                // salvar as questions do ReportTemplate
                foreach (var question in rtvm.QuestionTemplateInReportList)
                {
                    var createReportTemplateQuestionCommand = new CreateReportTemplateQuestionCommand
                    {
                        ReportTemplateId = rtvm.Id,
                        QuestionTemplateId = question.QuestionTemplateId,
                        QuestionTypeId = question.QuestionTypeId,
                        Order = question.Order
                    };
                    var resultCreateRTQuestion = await _mediator.Send(createReportTemplateQuestionCommand);

                    if (statusOk) statusOk = resultCreateRTQuestion.Succeeded;
                }
                return statusOk;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - WriteToDbOptionsFromQuestionTemplateAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<int> GetReportTypeIdAsync(int reportTemplateId, IMapper mapper, IMediator mediator)
        {
            var response = await mediator.Send(new GetReportTemplateByIdQuery() { Id = reportTemplateId });
            if (!response.Succeeded) return 0;
            var rtvm = mapper.Map<ReportTemplateViewModel>(response.Data);
            return rtvm.ReportTypeId;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllReportTemplatesAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            // lista de Report Templates
            var responseAllReportTemplates = await mediator.Send(new GetAllReportTemplateCachedQuery());
            if (!responseAllReportTemplates.Succeeded) return null;
            var allReportTemplatesData = mapper.Map<List<ReportTemplateViewModel>>(responseAllReportTemplates.Data).AsQueryable();

            return new SelectList(allReportTemplatesData, nameof(ReportTemplateViewModel.Id), nameof(ReportTemplateViewModel.Name), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<JsonResult> LoadTemoplatesFromType(int reportTypeId)
        {
            if (reportTypeId > 0)
            {
                var templatesResponse = await _mediator.Send(new GetReportTemplateByReportTypeIdCachedQuery() { reportTypeId = reportTypeId });

                if (templatesResponse.Succeeded)
                {
                    var reportTemplateViewModel = _mapper.Map<List<ReportTemplateViewModel>>(templatesResponse.Data);
                    var templates = new SelectList(reportTemplateViewModel, nameof(ReportTemplateViewModel.Id), nameof(ReportTemplateViewModel.Name), null, null);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { templatesList = templates });
                    return Json(jsonString);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------

    }
}