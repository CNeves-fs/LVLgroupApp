using AutoMapper;
using Core.Constants;
using Core.Features.QuestionOption.Queries.GetAllCached;
using Core.Features.QuestionTemplate.Queries.GetById;
using Core.Features.Report.Queries.GetAllCached;
using Core.Features.ReportTemplate.Commands.Create;
using Core.Features.ReportTemplate.Commands.Delete;
using Core.Features.ReportTemplate.Commands.Update;
using Core.Features.ReportTemplate.Queries.GetAllCached;
using Core.Features.ReportTemplate.Queries.GetById;
using Core.Features.ReportTemplateQuestion.Queries.GetAllCached;
using Core.Features.ReportTypes.Queries.GetAllCached;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Report.Controllers.ReportType;
using LVLgroupApp.Areas.Report.Models.QuestionOption;
using LVLgroupApp.Areas.Report.Models.QuestionTemplate;
using LVLgroupApp.Areas.Report.Models.ReportTemplate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.ReportTemplate.Controllers.ReportTemplate
{
    [Area("ReportTemplate")]
    [Authorize]
    public class ReportTemplateController : BaseController<ReportTemplateController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<ReportTemplateController> _localizer;

        //private IWebHostEnvironment _environment;

        //private readonly SignInManager<ApplicationUser> _signInManager;

        //private readonly UserManager<ApplicationUser> _userManager;


        //---------------------------------------------------------------------------------------------------

        public ReportTemplateController(IStringLocalizer<ReportTemplateController> localizer 
                //,SignInManager<ApplicationUser> signInManager 
                //,UserManager<ApplicationUser> userManager 
                //,IWebHostEnvironment environment
                )
        {
            _localizer = localizer;
            //_signInManager = signInManager;
            //_environment = environment;
            //_userManager = userManager;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportTemplate.View)]
        public IActionResult Index()
        {
            var model = new ReportTemplateViewModel();

            _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - Index - return viewModel");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportTemplate.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - LoadAll - return lista vazia de ReportTemplateViewModel");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de ReportTemplates para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

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



                var reportTypefilter = Request.Form["reportTypefilter"].FirstOrDefault();
                var activefilter = Request.Form["activefilter"].FirstOrDefault();



                int reportTypeId = reportTypefilter != null ? Convert.ToInt32(reportTypefilter) : 0;
                bool isFilterActive = activefilter != null ? Convert.ToBoolean(activefilter) : false;



                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;




                // lista de tipos de relatório
                var allReportTypesResponse = await _mediator.Send(new GetAllReportTypesCachedQuery());
                if (!allReportTypesResponse.Succeeded) return new ObjectResult(new { status = "error" });
                var allReportTypes = _mapper.Map<List<Core.Entities.Reports.ReportType>>(allReportTypesResponse.Data).AsQueryable();




                // lista de perguntas em modelos de relatório
                var allReportTemplateQuestionsResponse = await _mediator.Send(new GetAllReportTemplateQuestionCachedQuery());
                if (!allReportTemplateQuestionsResponse.Succeeded) return new ObjectResult(new { status = "error" });
                var allReportTemplateQuestions = _mapper.Map<List<Core.Entities.Reports.ReportTemplateQuestion>>(allReportTemplateQuestionsResponse.Data).AsQueryable();




                // lista de modelos de relatório
                var allReportTemplatesResponse = await _mediator.Send(new GetAllReportTemplateCachedQuery());
                if (!allReportTemplatesResponse.Succeeded) return new ObjectResult(new { status = "error" });
                var allReportTemplates = _mapper.Map<List<Core.Entities.Reports.ReportTemplate>>(allReportTemplatesResponse.Data).AsQueryable();
                // filtrar por tipo de relatório se necessário
                if (reportTypeId > 0)
                {
                    allReportTemplates = allReportTemplates.Where(rt => rt.ReportTypeId == reportTypeId);
                }



                // lista de relatórios
                var allReportsResponse = await _mediator.Send(new GetAllReportCachedQuery());
                if (!allReportsResponse.Succeeded) return new ObjectResult(new { status = "error" });
                var allReports = _mapper.Map<List<Core.Entities.Reports.Report>>(allReportsResponse.Data).AsQueryable();




                var allReportTemplatesViewModel = _mapper.Map<List<ReportTemplateViewModel>>(allReportTemplates);
                var culture = _culture.RequestCulture.Culture;
                foreach (var rtvm in allReportTemplatesViewModel)
                {
                    rtvm.ReportTypeName = await ReportTypeController.GetReportTypeNomeAsync(rtvm.Id, culture.Name, _mediator, _mapper);
                    rtvm.NumberOfQuestions = allReportTemplateQuestions.Where(rqt => rqt.ReportTemplateId == rtvm.Id).Count();
                    rtvm.UsedInReports = allReports.Where(r => r.ReportTemplateId == rtvm.Id).Count();
                }




                // ordenar lista
                var sortedReportTemplateData = allReportTemplatesViewModel.AsQueryable();
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    sortedReportTemplateData = sortedReportTemplateData.OrderBy(sortColumn + " " + sortColumnDirection);
                }


                // retornar lista para a datatable
                recordsTotal = sortedReportTemplateData.Count();
                var data = sortedReportTemplateData.Skip(skip).Take(pageSize).ToList();


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
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar novo ReportTemplate
            {
                var viewModel = new ReportTemplateViewModel();
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreateOrEdit - return _CreateOrEdit para criar novo ReportTemplate");
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", viewModel) });
            }
            else // Editar ReportTemplate
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreateOrEdit - Entrou para editar report template id=" + id);

                var response = await _mediator.Send(new GetReportTemplateByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var rtViewModel = _mapper.Map<ReportTemplateViewModel>(response.Data);
                    
                    //adicionar as QuestionTemplate ao view model
                    var responseRTQ = await _mediator.Send(new GetAllReportTemplateQuestionByReportCachedQuery() { reportTemplateId = rtViewModel.Id});
                    if (!responseRTQ.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreateOrEdit - Erro ao ler da db as ReportTemplateQuestions do ReportTemplate id=" + id);
                        _notify.Error(response.Message);
                        return new JsonResult(new { isValid = false, html = string.Empty });
                    }
                    var rtqViewModelList = _mapper.Map<List<ReportTemplateQuestionViewModel>>(responseRTQ.Data);

                    // adicionar QuestionTemplate
                    foreach (var rtq in rtqViewModelList)
                    {
                        var responseQT = await _mediator.Send(new GetQuestionTemplateByIdQuery() { Id = rtq.QuestionTemplateId });
                        if (!responseQT.Succeeded && responseQT.Data != null)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreateOrEdit - Erro ao ler da db a QuestionTemplate com id=" + rtq.QuestionTemplateId);
                            continue;
                        }

                        rtq.QuestionTemplate = _mapper.Map<QuestionTemplateViewModel>(responseQT.Data);

                        // adicionar options
                        if (rtq.QuestionTemplate != null && rtq.QuestionTypeId == QuestionTypeList.QTYPE_ESCOLHA_MULTIPLA)
                        {
                            var responseQO = await _mediator.Send(new GetQuestionOptionByQuestionTemplateIdCachedQuery() { questionTemplateId = rtq.QuestionTemplate.Id });
                            if (!responseQO.Succeeded && responseQO.Data != null)
                            {
                                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreateOrEdit - Erro ao ler da db as QuestionOptions da QuestionTemplate com id=" + rtq.QuestionTemplateId);
                                continue;
                            }
                            rtq.QuestionTemplate.Options = _mapper.Map<List<QuestionOptionViewModel>>(responseQO.Data);
                        }

                        rtViewModel.QuestionTemplateList.Add(rtq.QuestionTemplate);
                    }

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreateOrEdit - return _CreateOrEdit para editar ReportTemplate");
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", rtViewModel) });
                }
                else
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnGetCreateOrEdit - Erro ao ler da db o ReportTemplate id=" + id);
                    _notify.Error(response.Message);
                    return new JsonResult(new { isValid = false, html = string.Empty });
                }
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.ReportTemplate.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, ReportTemplateViewModel reportTemplateViewModel)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - Entrou para post da reportTemplate=" + id);

            // validar ModelState
            if (!ModelState.IsValid)
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - ModelState Not Valid");
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - Total erros = " + ModelState.ErrorCount);

                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - Error Key = " + key);
                    }
                }

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - returm _CreateOrEdit");
                var html1 = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", reportTemplateViewModel);
                return new JsonResult(new { isValid = false, html = html1 });
            }

            if (id > 0)
            {
                // editar reportTemplate
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - Entrou para post edit do report template id=" + id);


                //Update ReportTemplate
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - vai fazer update do ReportTemplate editado");

                var updateReportTemplateCommand = _mapper.Map<UpdateReportTemplateCommand>(reportTemplateViewModel);
                var result = await _mediator.Send(updateReportTemplateCommand);
                if (result.Succeeded)
                {
                    _notify.Information($"{_localizer["O Modelo de Relatório"]} {result.Data} {_localizer["foi atualizado com sucesso."]}");
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - ReportTemplate editado, foi atualizado na db");
                }
                else
                {
                    _notify.Error($"{_localizer["O Modelo de Relatório"]} {result.Data} {_localizer["não foi atualizado"]}");
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - Erro:ReportTemplate não foi atualizada");
                }

            }
            else
            {
                // criar reportTemplate
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - Entrou para post create da reportTemplate=" + id);

                // comando Criar ReportTemplate
                var createReportTemplateCommand = _mapper.Map<CreateReportTemplateCommand>(reportTemplateViewModel);
                var result = await _mediator.Send(createReportTemplateCommand);
                if (!result.Succeeded)
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - Erro:ReportTemplate não foi criado: " + result.Message);
                    _notify.Error(_localizer["Erro:Modelo de Relatório não foi criado"]);

                    var html1 = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new ReportTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html1 });
                }

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - ReportTemplate criado");
                _notify.Success($"{_localizer["A Modelo de Relatório com o Id"]} {result.Data} {_localizer["foi criado com sucesso."]}");

            }

            // return Index View
            var model = new ReportTemplateViewModel();
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostCreateOrEdit - return viewModel");

            var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", model);
            return new JsonResult(new { isValid = true, html = html });
            
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Chamada por ajax call a partir da view _CreateOrEdit.
        /// É passado o Id do Report Template a ser removido
        /// </summary>
        /// <param name="id"></param>
        /// <returns>"JsonResult = _ViewAll"</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.ReportTemplate.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            try
            {
                // verificar se ReportTemplate existe
                var reportTemplateCommand = await _mediator.Send(new GetReportTemplateByIdQuery() { Id = id });
                if (!reportTemplateCommand.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - Erro ao ler reportTemplate: " + reportTemplateCommand.Message);
                    _notify.Error($"{_localizer["O Modelo de Relatório com o Id"]} {id} {_localizer[" não foi removido:"]} {reportTemplateCommand.Message}");
                    return new JsonResult(new { isValid = false, html = "" });
                }

                var rt = _mapper.Map<Core.Entities.Reports.ReportTemplate>(reportTemplateCommand.Data);

                // ler da db os relatórios criados por este reportTemplate
                var allReportsCommand = await _mediator.Send(new GetAllReportByReportTemplateIdCachedQuery() { reportTemplateId = id });
                if (!allReportsCommand.Succeeded)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete -  Error: " + allReportsCommand.Message);
                    return new JsonResult(new { isValid = false, html = "" });
                }

                var allReports = _mapper.Map<List<ReportTemplateViewModel>>(allReportsCommand.Data);


                // só podemos remover o Modelo se não existirem relatórios criados por ele
                if (allReports.Count() == 0)
                {
                    // remover reportTemplate
                    var deleteCommand = await _mediator.Send(new DeleteReportTemplateCommand() { Id = id });
                    if (!deleteCommand.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - Erro ao remover ReportTemplate da db: " + deleteCommand.Message);
                        _notify.Error(deleteCommand.Message);
                        return new JsonResult(new { isValid = true, html = "" });
                    }

                    _notify.Information($"{_localizer["O Modelo de Relatório com o Id"]} {id} {_localizer[" foi removido."]}");

                }
                else
                {
                    // marcar o Modelo de Relatório como inativo e atualizar db
                    rt.IsActive = false;

                    // update db
                    var updateReportTemplateCommand = _mapper.Map<UpdateReportTemplateCommand>(rt);
                    var updateCommandResult = await _mediator.Send(updateReportTemplateCommand);
                    if (!updateCommandResult.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - Erro ao atualizar ReportTemplate na db: " + updateCommandResult.Message);
                        _notify.Error(updateCommandResult.Message);
                        return new JsonResult(new { isValid = true, html = "" });
                    }

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - ReportTemplate foi atualizado na db com isActive = false ");
                    _notify.Information($"{_localizer["O Modelo de Relatório com o Id"]} {id} {_localizer[" foi desativado. Um Modelo só poderá ser removido, se não existirem Relatórios nele baseado."]}");
                }



                // return _ViewAll
                var viewModel = new List<ReportTemplateViewModel>();

                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                return new JsonResult(new { isValid = true, html = html });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportTemplate Contoller - OnPostDelete - IO exception vai sair e retornar Error: " + ex.Message);
                return new JsonResult(new { isValid = false, html = "" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve o reporttypeid de um ReportTemplate
        /// </summary>
        /// <returns>int=id</returns>

        public static async Task<int> GetReportTypeIdAsync(int reportTemplateId, IMediator mediator, IMapper mapper)
        {
            // verificar se ReportTemplate existe
            var reportTemplateCommand = await mediator.Send(new GetReportTemplateByIdQuery() { Id = reportTemplateId });
            if (!reportTemplateCommand.Succeeded || reportTemplateCommand.Data == null)
            {
                return 0;
            }
            return reportTemplateCommand.Data.ReportTypeId;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve o nome localized do type de um ReportTemplate
        /// </summary>
        /// <returns>string=nome</returns>

        public static async Task<string> GetReportTypeNomeAsync(int reportTemplateId, string lang, IMediator mediator, IMapper mapper)
        {
            // verificar se ReportTemplate existe
            var reportTemplateCommand = await mediator.Send(new GetReportTemplateByIdQuery() { Id = reportTemplateId });
            if (!reportTemplateCommand.Succeeded || reportTemplateCommand.Data == null)
            {
                return string.Empty;
            }
            return await ReportTypeController.GetReportTypeNomeAsync(reportTemplateCommand.Data.ReportTypeId, lang, mediator, mapper);
        }


        //---------------------------------------------------------------------------------------------------

    }
}