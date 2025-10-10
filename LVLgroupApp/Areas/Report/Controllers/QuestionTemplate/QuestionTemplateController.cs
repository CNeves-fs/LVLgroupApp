using Core.Constants;
using Core.Features.QuestionOption.Commands.Create;
using Core.Features.QuestionOption.Commands.Update;
using Core.Features.QuestionOption.Queries.GetAllCached;
using Core.Features.QuestionTemplate.Commands.Create;
using Core.Features.QuestionTemplate.Commands.Delete;
using Core.Features.QuestionTemplate.Commands.Update;
using Core.Features.QuestionTemplate.Queries.GetAllCached;
using Core.Features.QuestionTemplate.Queries.GetById;
using Core.Features.QuestionTemplateLocalized.Commands.Create;
using Core.Features.ReportTemplateQuestion.Queries.GetAllCached;
using Core.Features.ReportTypes.Commands.Create;
using Core.Features.ReportTypesLocalized.Commands.Create;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Report.Models.QuestionOption;
using LVLgroupApp.Areas.Report.Models.QuestionTemplate;
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

namespace LVLgroupApp.Areas.Report.Controllers.QuestionTemplate
{
    [Area("Report")]
    [Authorize]
    public class QuestionTemplateController : BaseController<QuestionTemplateController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<QuestionTemplateController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateController(IStringLocalizer<QuestionTemplateController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.QuestionTemplate.View)]
        public IActionResult Index()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - Index - start");

            // Culture contains the information of the requested culture
            var culture = _culture.RequestCulture.Culture;

            var model = new QuestionTemplateViewModel();
            model.QuestionTypes = QuestionTypeList.GetSelectListQuestionType(culture.Name, 0);




            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.QuestionTemplate.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - LoadAll - return _ViewAll");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.QuestionTemplate.View)]
        public async Task<IActionResult> GetQuestionTemplates()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var questionType = Request.Form["questionType"].FirstOrDefault();
                var version = Request.Form["version"].FirstOrDefault();
                var isActive = Request.Form["isActive"].FirstOrDefault();



                int intTypeFilter = questionType != null ? Convert.ToInt32(questionType) : 0;
                int intVersionFilter = string.IsNullOrEmpty(version) ? 0 : Convert.ToInt32(version);
                bool isFilterActive = isActive != null ? Convert.ToBoolean(isActive) : false;




                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Culture contains the information of the requested culture
                var culture = _culture.RequestCulture.Culture;

                // lista de Question Templates
                var responseAllQuestionTemplates = await _mediator.Send(new GetAllQuestionTemplateCachedQuery());
                if (!responseAllQuestionTemplates.Succeeded) return null;
                var allQuestionTemplatesData = _mapper.Map<List<QuestionTemplateViewModel>>(responseAllQuestionTemplates.Data).AsQueryable();

                // lista de ReportTemplateQuestions para verificar se a
                // QuestionTemplate está a ser usada em algum Report
                var responseAllReportTemplateQuestions = await _mediator.Send(new GetAllReportTemplateQuestionCachedQuery());
                if (!responseAllReportTemplateQuestions.Succeeded) return null;
                var allReportTemplateQuestionsData = _mapper.Map<List<Core.Entities.Reports.ReportTemplateQuestion>>(responseAllReportTemplateQuestions.Data).AsQueryable();

                // lista de Question Options
                var responseAllQuestionOptions = await _mediator.Send(new GetAllQuestionOptionCachedQuery());
                if (!responseAllQuestionOptions.Succeeded) return null;
                var allQuestionOptionsData = _mapper.Map<List<QuestionOptionViewModel>>(responseAllQuestionOptions.Data).AsQueryable();

                // adicionar Translations ao ViewModel
                foreach (var qTemplate in allQuestionTemplatesData) 
                {
                    qTemplate.Language = culture.Name;
                    qTemplate.QuestionTypeName = QuestionTypeList.GetQuestionTypeName(culture.Name, qTemplate.QuestionTypeId);
                    qTemplate.UsedInReports = allReportTemplateQuestionsData.Where(rtq => rtq.QuestionTemplateId == qTemplate.Id).Count();

                    // adicionar Questions Options se necessário
                    if (qTemplate.QuestionTypeId == QuestionTypeList.QTYPE_ESCOLHA_MULTIPLA)
                    {
                        qTemplate.Options = allQuestionOptionsData.Where(qo => qo.QuestionTemplateId == qTemplate.Id).ToList();
                    }
                    else
                    {
                        qTemplate.Options = new List<QuestionOptionViewModel>();
                    }
                }

                var allQuestionTemplateViewModel = allQuestionTemplatesData.AsQueryable();



                // filtrar por type se necessário
                if (intTypeFilter > 0)
                {
                    allQuestionTemplateViewModel = allQuestionTemplateViewModel.Where(qt => qt.QuestionTypeId == intTypeFilter);
                }


                // filtrar por active se necessário
                if (isFilterActive)
                {
                    allQuestionTemplatesData = allQuestionTemplatesData.Where(q => q.IsActive == true);
                }


                // filtrar por version se necessário
                if (intVersionFilter > 0)
                {
                    allQuestionTemplatesData = allQuestionTemplatesData.Where(q => q.Version == intVersionFilter);
                }
                

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    allQuestionTemplatesData = allQuestionTemplatesData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allQuestionTemplatesData = allQuestionTemplatesData.Where(q => q.QuestionText.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                }


                recordsTotal = allQuestionTemplatesData.Count();
                var data = allQuestionTemplatesData.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);

            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - GetQuestionTemplates - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        //[HttpPost]
        //[Authorize(Policy = Permissions.QuestionTemplate.View)]
        //public async Task<IActionResult> GetQuestionOtions()
        //{
        //    try
        //    {
        //        var draw = Request.Form["draw"].FirstOrDefault();
        //        var start = Request.Form["start"].FirstOrDefault();
        //        var length = Request.Form["length"].FirstOrDefault();
        //        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        var searchValue = Request.Form["search[value]"].FirstOrDefault();



        //        var questionTemplateIdFilter = Request.Form["questionTemplateIdFilter"].FirstOrDefault();
        //        int questionTemplateId = questionTemplateIdFilter != null ? Convert.ToInt32(questionTemplateIdFilter) : 0;



        //        int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //        if (pageSize < 0) pageSize = Int32.MaxValue;
        //        int skip = start != null ? Convert.ToInt32(start) : 0;
        //        int recordsTotal = 0;

        //        // lista de Question Options
        //        var responseQuestionOptions = await _mediator.Send(new GetQuestionOptionByQuestionTemplateIdCachedQuery() { questionTemplateId = questionTemplateId });
        //        if (!responseQuestionOptions.Succeeded) return null;
        //        var questionOptionsList = _mapper.Map<List<Core.Entities.Reports.QuestionOption>>(responseQuestionOptions.Data).ToList();


        //        // ordenar lista por Order descending
        //        questionOptionsList = questionOptionsList.OrderByDescending(qo => qo.Order).ToList();



        //        recordsTotal = questionOptionsList.Count();
        //        var data = questionOptionsList.Skip(skip).Take(pageSize).ToList();

        //        var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
        //        return Ok(jsonData);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - GetQuestionOtions - Exception vai sair e retornar Error: " + ex.Message);
        //        return new ObjectResult(new { status = "error" });
        //    }
        //}


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.QuestionTemplate.Edit)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            //criar modelView para retornar
            var questionTemplateViewModel =  new QuestionTemplateViewModel();

            var culture = _culture.RequestCulture.Culture;

            if (id > 0) // editar QuestionTemplate
            {
                questionTemplateViewModel.EditMode = true;

                var response = await _mediator.Send(new GetQuestionTemplateByIdQuery() { Id = id });
                questionTemplateViewModel = _mapper.Map<QuestionTemplateViewModel>(response.Data);

                //construir All Question tipos para Select dropbox
                questionTemplateViewModel.QuestionTypes = QuestionTypeList.GetSelectListQuestionType(culture.Name, questionTemplateViewModel.QuestionTypeId);

                //construir lista de options da QuestionTemplate se necessário
                if (questionTemplateViewModel.QuestionTypeId == QuestionTypeList.QTYPE_ESCOLHA_MULTIPLA)
                {
                    var responseQuestionOption = await _mediator.Send(new GetQuestionOptionByQuestionTemplateIdCachedQuery() { questionTemplateId = questionTemplateViewModel.Id });
                    if (!responseQuestionOption.Succeeded) return null;
                    var allQuestionOptionsData = _mapper.Map<List<QuestionOptionViewModel>>(responseQuestionOption.Data).AsQueryable();
                    questionTemplateViewModel.Options = allQuestionOptionsData.OrderBy(o => o.Order).ToList();
                }
                else
                {
                    questionTemplateViewModel.Options = new List<QuestionOptionViewModel>();
                }

                // verificar se a QuestionTemplate está a ser usada em algum Report
                var responseAllResponseTemplateQuestions = await _mediator.Send(new GetAllReportTemplateQuestionByQuestionCachedQuery() { questionTemplateId = id});
                if (!responseAllResponseTemplateQuestions.Succeeded) return null;
                var allReportTemplateQuestionsData = _mapper.Map<List<Core.Entities.Reports.ReportTemplateQuestion>>(responseAllResponseTemplateQuestions.Data).AsQueryable();
                questionTemplateViewModel.UsedInReports = allReportTemplateQuestionsData.Count();

            }
            else // criar QuestionTemplate
            {
                questionTemplateViewModel.EditMode = false;

                // Version 1
                questionTemplateViewModel.Version = 1;

                // Active por default
                questionTemplateViewModel.IsActive = true;

                // CreatedAt por default
                questionTemplateViewModel.CreatedAt = DateTime.Now;

                // Não existe em Reports
                questionTemplateViewModel.UsedInReports = 0;

                // QuestionTypeId por default
                questionTemplateViewModel.QuestionTypeId = 0;

                //construir All Question tipos para Select dropbox
                questionTemplateViewModel.QuestionTypes = QuestionTypeList.GetSelectListQuestionType(culture.Name, 0);

                //construir lista de options da QuestionTemplate
                questionTemplateViewModel.Options = new List<QuestionOptionViewModel>();
            }
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", questionTemplateViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.QuestionTemplate.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, QuestionTemplateViewModel questionTemplate)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {

                    //create new QuestionTemplate
                    var createQuestionTemplateCommand = _mapper.Map<CreateQuestionTemplateCommand>(questionTemplate);
                    var result = await _mediator.Send(createQuestionTemplateCommand);
                    if (result.Succeeded)
                    {
                        // criar translations
                        var questionTemplateId = result.Data;
                        // pt
                        var createPtQTLCommand = new CreateQuestionTemplateLocalizedCommand();
                        createPtQTLCommand.QuestionTemplateId = questionTemplateId;
                        createPtQTLCommand.Language = "pt";                        
                        createPtQTLCommand.QuestionText = questionTemplate.QuestionText;
                        var resultPT = await _mediator.Send(createPtQTLCommand);
                        // es
                        var createEsQTLCommand = new CreateQuestionTemplateLocalizedCommand();
                        createEsQTLCommand.QuestionTemplateId = questionTemplateId;
                        createEsQTLCommand.Language = "es";
                        createEsQTLCommand.QuestionText = questionTemplate.EsQuestionText;
                        var resultEs = await _mediator.Send(createEsQTLCommand);
                        // en
                        var createEnQTLCommand = new CreateQuestionTemplateLocalizedCommand();
                        createEnQTLCommand.QuestionTemplateId = questionTemplateId;
                        createEnQTLCommand.Language = "en";
                        createEnQTLCommand.QuestionText = questionTemplate.EnQuestionText;
                        var resultEn = await _mediator.Send(createEnQTLCommand);

                        id = result.Data;
                        _notify.Success($"{_localizer["Modelo de Pergunta com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);

                    // se for do tipo escolha múltipla, guardar as options
                    if (questionTemplate.QuestionTypeId == QuestionTypeList.QTYPE_ESCOLHA_MULTIPLA && questionTemplate.Options != null && questionTemplate.Options.Count() > 0)
                    {
                        int order = 1;
                        foreach (var option in questionTemplate.Options)
                        {
                            option.QuestionTemplateId = result.Data;
                            option.Order = order;

                            var createOptionCommand = _mapper.Map<CreateQuestionOptionCommand>(option);
                            var resultOption = await _mediator.Send(createOptionCommand);
                            if (resultOption.Succeeded)
                            {
                                _notify.Success($"{_localizer["Opções associadas ao modelo de pergunta com ID"]} {id} {_localizer[" criadas com sucesso."]}");
                            }
                            else
                            {
                                _notify.Error(resultOption.Message);
                            }

                            order++;
                        }
                    }
                }
                else
                {
                    //update QuestionTemplate
                    var updateQuestionTemplateCommand = _mapper.Map<UpdateQuestionTemplateCommand>(questionTemplate);
                    var result = await _mediator.Send(updateQuestionTemplateCommand);
                    if (result.Succeeded)
                    {
                        _notify.Information($"{_localizer["Loja com ID"]} {result.Data} {_localizer[" atualizada."]}");

                        // se for do tipo escolha múltipla, guardar as options
                        if (questionTemplate.QuestionTypeId == QuestionTypeList.QTYPE_ESCOLHA_MULTIPLA && questionTemplate.Options != null && questionTemplate.Options.Count() > 0)
                        {
                            int order = 1;
                            foreach (var option in questionTemplate.Options)
                            {
                                option.QuestionTemplateId = id;
                                option.Order = order;
                                
                                // se a opção já existir, atualizar
                                if (option.Id > 0)
                                {
                                    var updateOptionCommand = _mapper.Map<UpdateQuestionOptionCommand>(option);
                                    var resultOption = await _mediator.Send(updateOptionCommand);
                                    if (resultOption.Succeeded)
                                    {
                                        _notify.Information($"{_localizer["Opção associada ao modelo de pergunta com ID"]} {id} {_localizer[" atualizada."]}");
                                    }
                                    else
                                    {
                                        _notify.Error(resultOption.Message);
                                    }
                                }
                                else // se a opção não existir, criar
                                {
                                    var createOptionCommand = _mapper.Map<CreateQuestionOptionCommand>(option);
                                    var resultOption = await _mediator.Send(createOptionCommand);
                                    if (resultOption.Succeeded)
                                    {
                                        _notify.Success($"{_localizer["Opção associada ao modelo de pergunta com ID"]} {id} {_localizer[" criada."]}");
                                    }
                                    else
                                    {
                                        _notify.Error(resultOption.Message);
                                    }
                                }
                                order++;
                            }
                        }
                    }
                    else _notify.Error(result.Message);
                }

                // return _ViewAll
                var response = await _mediator.Send(new GetAllQuestionTemplateCachedQuery());
                var viewModel = _mapper.Map<List<QuestionTemplateViewModel>>(response.Data);
 
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                // Model State is not valid
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", questionTemplate);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.QuestionTemplate.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteQuestionTemplateCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Modelo de pergunta com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllQuestionTemplateCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<QuestionTemplateViewModel>>(response.Data);
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


        public async Task<JsonResult> LoadQuestions(int questionTypeId)
        {
            if (questionTypeId > 0)
            {
                var questionTemplateResponse = await _mediator.Send(new GetQuestionTemplateByQuestionTypeIdCachedQuery() { questionTypeId = questionTypeId });

                if (questionTemplateResponse.Succeeded)
                {
                    var questionTemplateViewModel = _mapper.Map<List<QuestionTemplateViewModel>>(questionTemplateResponse.Data);
                    var questionTemplateSelectList = new SelectList(questionTemplateViewModel, nameof(QuestionTemplateViewModel.Id), nameof(QuestionTemplateViewModel.QuestionText), null, null);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { QuestionTemplateList = questionTemplateSelectList });
                    return Json(jsonString);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------

    }
}