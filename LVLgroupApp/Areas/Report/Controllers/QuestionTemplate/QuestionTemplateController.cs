using AspNetCoreHero.Results;
using Core.Constants;
using Core.Entities.Reports;
using Core.Features.QuestionOption.Commands.Create;
using Core.Features.QuestionOption.Commands.Delete;
using Core.Features.QuestionOption.Commands.Update;
using Core.Features.QuestionOption.Queries.GetAllCached;
using Core.Features.QuestionTemplate.Commands.Create;
using Core.Features.QuestionTemplate.Commands.Delete;
using Core.Features.QuestionTemplate.Commands.Update;
using Core.Features.QuestionTemplate.Queries.GetAllCached;
using Core.Features.QuestionTemplate.Queries.GetById;
using Core.Features.QuestionTemplateLocalized.Commands.Create;
using Core.Features.QuestionTemplateLocalized.Queries.GetAllCached;
using Core.Features.ReportTemplateQuestion.Queries.GetAllCached;
using Core.Features.ReportTypes.Commands.Create;
using Core.Features.ReportTypesLocalized.Commands.Create;
using Core.Features.TiposOcorrenciasLocalized.Commands.Delete;
using DocumentFormat.OpenXml.Office2010.Excel;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Report.Models.QuestionOption;
using LVLgroupApp.Areas.Report.Models.QuestionTemplate;
using LVLgroupApp.Areas.Vendas.Models.VendaSemanal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

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

                // lista de Question Templates (localized)
                var responseAllQuestionTemplateLocalized = await _mediator.Send(new GetAllQuestionTemplateLocalizedCachedQuery());
                if (!responseAllQuestionTemplateLocalized.Succeeded) return null;
                var allQuestionTemplateLocalizedData = _mapper.Map<List<QuestionTemplateLocalizedViewModel>>(responseAllQuestionTemplateLocalized.Data).AsQueryable();

                // lista de ReportTemplateQuestions para verificar se a
                // QuestionTemplate está a ser usada em algum Report
                var responseAllReportTemplateQuestions = await _mediator.Send(new GetAllReportTemplateQuestionCachedQuery());
                if (!responseAllReportTemplateQuestions.Succeeded) return null;
                var allReportTemplateQuestionsData = _mapper.Map<List<Core.Entities.Reports.ReportTemplateQuestion>>(responseAllReportTemplateQuestions.Data).AsQueryable();

                // lista de Question Options
                var responseAllQuestionOptions = await _mediator.Send(new GetAllQuestionOptionCachedQuery());
                if (!responseAllQuestionOptions.Succeeded) return null;
                var allQuestionOptionsData = _mapper.Map<List<QuestionOptionViewModel>>(responseAllQuestionOptions.Data).AsQueryable();

                // lista de Question Templates
                var allQuestionTemplatesList = new List<QuestionTemplateListViewModel>();


                // adicionar Translations ao ViewModel
                foreach (var qTemplate in allQuestionTemplatesData) 
                {
                    // criar novo QuestionTemplateViewModel
                    var questionTemplate = new QuestionTemplateListViewModel();

                    questionTemplate.Id = qTemplate.Id;
                    questionTemplate.QuestionText = allQuestionTemplateLocalizedData.Where(qtl => qtl.QuestionTemplateId == qTemplate.Id && qtl.Language == culture.Name).Select(qtl => qtl.QuestionText).FirstOrDefault() ?? "";                    
                    questionTemplate.CreatedAt = qTemplate.CreatedAt;
                    questionTemplate.IsActive = qTemplate.IsActive;
                    questionTemplate.Version = qTemplate.Version;
                    questionTemplate.QuestionTypeId = qTemplate.QuestionTypeId;
                    questionTemplate.QuestionTypeName = QuestionTypeList.GetQuestionTypeName(culture.Name, qTemplate.QuestionTypeId);
                    questionTemplate.UsedInReports = allReportTemplateQuestionsData.Where(rtq => rtq.QuestionTemplateId == qTemplate.Id).Count();

                    // Questions Options não são necessárias para a lista em _ViewAll
                    qTemplate.Options = new List<QuestionOptionViewModel>();

                    // adicionar ao allQuestionTemplatesList
                    allQuestionTemplatesList.Add(questionTemplate);
                }

                var allQuestionTemplateViewModel = allQuestionTemplatesList.AsQueryable();

                // filtrar por type se necessário
                if (intTypeFilter > 0)
                {
                    allQuestionTemplateViewModel = allQuestionTemplateViewModel.Where(qt => qt.QuestionTypeId == intTypeFilter);
                }

                // filtrar por active se necessário
                if (isFilterActive)
                {
                    allQuestionTemplateViewModel = allQuestionTemplateViewModel.Where(q => q.IsActive == true);
                }

                // filtrar por version se necessário
                if (intVersionFilter > 0)
                {
                    allQuestionTemplateViewModel = allQuestionTemplateViewModel.Where(q => q.Version == intVersionFilter);
                }              

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    allQuestionTemplateViewModel = allQuestionTemplateViewModel.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allQuestionTemplateViewModel = allQuestionTemplateViewModel.Where(q => q.QuestionText.Contains(searchValue, StringComparison.OrdinalIgnoreCase));

                }


                recordsTotal = allQuestionTemplateViewModel.Count();
                var data = allQuestionTemplateViewModel.Skip(skip).Take(pageSize).ToList();

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


        [Authorize(Policy = Permissions.QuestionTemplate.Edit)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            //criar QuestionTemplateViewModel para edição ou criação
            var qtvm = new QuestionTemplateViewModel();
            var culture = _culture.RequestCulture.Culture;

            try
            {
                if (id > 0)
                {
                    // editar QuestionTemplate
                    var response = await _mediator.Send(new GetQuestionTemplateByIdQuery() { Id = id });
                    if (!response.Succeeded) return null;
                    qtvm = _mapper.Map<QuestionTemplateViewModel>(response.Data);

                    //construir All Question tipos para Select dropbox
                    qtvm.QuestionTypes = QuestionTypeList.GetSelectListQuestionType(culture.Name, qtvm.QuestionTypeId);

                    // verificar se a QuestionTemplate está a ser usada em algum Report
                    qtvm.UsedInReports = await GetTotalReportsUsingQuestionTemplateAsync(id);

                    //construir lista de options da QuestionTemplate se necessário
                    qtvm.Options = await GetOptionsFromQuestionTemplateAsync(id, qtvm.QuestionTypeId);

                    // lista de Question Templates (localized)
                    qtvm = await GetFromDbQuestionTemplateLocalizedAsync(qtvm);

                    qtvm.EditMode = true;
                }
                else
                {
                    // criar QuestionTemplate vazio
                    qtvm = InitNewQuestionTemplate();
                }

                // retornar QuestionTemplateViewModel
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", qtvm) });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnGetCreateOrEdit - Exception: " + ex.Message);
                // retornar QuestionTemplateViewModel
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", qtvm) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.QuestionTemplate.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, QuestionTemplateViewModel questionTemplate)
        {
            try
            {
                var culture = _culture.RequestCulture.Culture;

                if (!ModelState.IsValid)
                {
                    // ModelState not valid
                    // devolver QuestionTemplateViewModel para continuar edição
                    
                    //construir All Question tipos para Select dropbox
                    questionTemplate.QuestionTypes = QuestionTypeList.GetSelectListQuestionType(culture.Name, questionTemplate.QuestionTypeId);

                    var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", questionTemplate);
                    return new JsonResult(new { isValid = false, html = html });
                }
                else
                {
                    // Model is valid

                    if (id > 0)
                    {
                        // update QuestionTemplate

                        // verificar se a QuestionTemplate está a ser usada em algum Report
                        questionTemplate.UsedInReports = await GetTotalReportsUsingQuestionTemplateAsync(id);
                        if (questionTemplate.UsedInReports > 0)
                        {
                            // se estiver a ser usada, não podemos atualizar a QuestionTemplate
                            // temos que criar outra nova com a version incrementada
                            questionTemplate.Id = 0;                                    // Id a 0 para criar novo    
                            questionTemplate.Version = questionTemplate.Version + 1;    // Version incrementada
                            questionTemplate.IsActive = true;                           // Active por default
                            questionTemplate.CreatedAt = DateTime.Now;                  // CreatedAt por default
                            questionTemplate.UsedInReports = 0;                         // Não existe em Reports

                            //create new QuestionTemplate
                            var createQuestionTemplateCommand = _mapper.Map<CreateQuestionTemplateCommand>(questionTemplate);
                            var resultCQTC = await _mediator.Send(createQuestionTemplateCommand);
                            if (!resultCQTC.Succeeded)
                            {
                                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnPostCreateOrEdit - Exception: " + resultCQTC.Message);
                                _notify.Error(resultCQTC.Message);
                                // return _ViewAll
                                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                                return new JsonResult(new { isValid = true, html = html });
                            }

                            // atualizar Id
                            questionTemplate.Id = resultCQTC.Data;

                            // criar translations
                            var translationsInDb = WriteToDbQuestionTemplateLocalizedAsync(questionTemplate);

                            _notify.Success($"{_localizer["Modelo de Pergunta com ID"]} {resultCQTC.Data} {_localizer[" criado."]}");

                            // se for do tipo escolha múltipla, guardar as options
                            var optionsInDb = WriteToDbOptionsFromQuestionTemplateAsync(questionTemplate);

                        }
                        else
                        {
                            // atualizar QuestionTemplate existente

                            var updateQuestionTemplateCommand = _mapper.Map<UpdateQuestionTemplateCommand>(questionTemplate);
                            var result = await _mediator.Send(updateQuestionTemplateCommand);
                            if (!result.Succeeded)
                            {
                                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnPostCreateOrEdit - Exception: " + result.Message);
                                _notify.Error(result.Message);
                                // return _ViewAll
                                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                                return new JsonResult(new { isValid = true, html = html });
                            }

                            // atualizar translations
                            var translationsInDb = await WriteToDbQuestionTemplateLocalizedAsync(questionTemplate);

                            _notify.Success($"{_localizer["Modelo de Pergunta com ID"]} {result.Data} {_localizer[" atualizado."]}");

                            // se for do tipo escolha múltipla, guardar as options
                            var optionsInDb = await WriteToDbOptionsFromQuestionTemplateAsync(questionTemplate);

                        }
                    }
                    else
                    {
                        // id=0 criar novo QuestionTemplate
                        questionTemplate.Version = 1;                   // Version 1
                        questionTemplate.IsActive = true;               // Active por default
                        questionTemplate.CreatedAt = DateTime.Now;      // CreatedAt por default

                        //create new QuestionTemplate
                        var createQuestionTemplateCommand = _mapper.Map<CreateQuestionTemplateCommand>(questionTemplate);
                        var result = await _mediator.Send(createQuestionTemplateCommand);
                        if (!result.Succeeded)
                        {
                            _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnPostCreateOrEdit - Exception: " + result.Message);
                            _notify.Error(result.Message);

                            // return _ViewAll
                            var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                            return new JsonResult(new { isValid = true, html = html });
                        }

                        // atualizar Id
                        questionTemplate.Id = result.Data;

                        // criar translations
                        var translationsInDb = await WriteToDbQuestionTemplateLocalizedAsync(questionTemplate);

                        _notify.Success($"{_localizer["Modelo de Pergunta com ID"]} {result.Data} {_localizer[" criado."]}");

                        // se for do tipo escolha múltipla, guardar as options
                        var optionsInDb = await WriteToDbOptionsFromQuestionTemplateAsync(questionTemplate);

                    }
                    // return _ViewAll
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }
            }
            catch (Exception ex)
            {
                // devolver QuestionTemplateViewModel para continuar edição
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnPostCreateOrEdit - Exception: " + ex.Message);
                _notify.Error($"{_localizer["Erro ao salvar o Modelo de Pergunta. Possível duplicado."]}");

                //construir All Question tipos para Select dropbox
                questionTemplate.QuestionTypes = QuestionTypeList.GetSelectListQuestionType(_culture.RequestCulture.Culture.Name, questionTemplate.QuestionTypeId);

                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnPostCreateOrEdit - Exception vai retornar _CreateOrEdit com o Error: " + ex.Message);
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", questionTemplate);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.QuestionTemplate.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnPostDelete - Modelo de pergunta inválido id: " + id);
                    _notify.Error($"{_localizer["Não é possível eliminar o Modelo de Pergunta com ID"]} {id}");
                    var html_text= await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                // verificar se a QuestionTemplate está a ser usada em algum Report
                var totalReportsUsing = await GetTotalReportsUsingQuestionTemplateAsync(id);
                if (totalReportsUsing > 0)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnPostDelete - Modelo de pergunta está a ser usado em relatórios.");
                    _notify.Error($"{_localizer["Não é possível eliminar o Modelo de Pergunta com ID"]} {id} {_localizer[" porque está a ser usado em relatórios."]}");
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                // remover QuestionTemplate
                var deleteCommand = await _mediator.Send(new DeleteQuestionTemplateCommand { Id = id });
                if (!deleteCommand.Succeeded)
                {
                    _notify.Error(deleteCommand.Message);
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                // sucesso
                _notify.Information($"{_localizer["Modelo de pergunta com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllQuestionTemplateCachedQuery());
                if (!response.Succeeded)
                {
                    _notify.Error(response.Message);
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                // se existirem, remover translations desta QuestionTemplate
                var responseAllQuestionTemplateLocalized = await _mediator.Send(new GetQuestionTemplateLocalizedByQuestionTemplateIdQuery() { questionTemplateId = id });
                if (!responseAllQuestionTemplateLocalized.Succeeded)
                {
                    var html_text = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                    return new JsonResult(new { isValid = true, html = html_text });
                }

                var allQuestionTemplateLocalizedData = _mapper.Map<List<QuestionTemplateLocalizedViewModel>>(responseAllQuestionTemplateLocalized.Data).AsQueryable();

                foreach (var qtlvm in allQuestionTemplateLocalizedData)
                {
                    // remover translation
                    var deleteQTLResult = await _mediator.Send(new DeleteQuestionTemplateLocalizedCommand() { Id = qtlvm.Id });
                }

                // remover options desta QuestionTemplate
                var responseAllQuestionOptions = await _mediator.Send(new GetQuestionOptionByQuestionTemplateIdCachedQuery() { questionTemplateId = id });
                if (responseAllQuestionOptions.Succeeded)
                {
                    var allQuestionOptionData = _mapper.Map<List<QuestionOptionViewModel>>(responseAllQuestionOptions.Data).AsQueryable();
                    foreach (var qovm in allQuestionOptionData)
                    {
                        // remover option
                        var deleteQOResult = await _mediator.Send(new DeleteQuestionOptionCommand() { Id = qovm.Id });
                    }
                }

                // reurn _viewall
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                return new JsonResult(new { isValid = true, html = html });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - OnPostDelete - Exception vai retornar _CreateOrEdit com o Error: " + ex.Message);
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", new QuestionTemplateViewModel());
                return new JsonResult(new { isValid = true, html = html });
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


        /// <summary>
        /// prepara uma estrutura QuestionTemplateViewModel para ser enviada
        /// ao client. evocada no atendimento GET OnGetCreateOrEdit (criar Question Template).
        /// </summary>
        /// <returns> type="QuestionTemplateViewModel"</returns>

        internal  QuestionTemplateViewModel InitNewQuestionTemplate()
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - InitNewQuestionTemplateAsync - Entrou para criar estrutura QuestionTemplateViewModel");

                var culture = _culture.RequestCulture.Culture;

                //criar modelView para retornar
                var qtvm = new QuestionTemplateViewModel();

                // criar QuestionTemplate vazio
                qtvm.EditMode = false;
                qtvm.Language = culture.Name;       // Language
                qtvm.Version = 1;                   // Version 1
                qtvm.IsActive = true;               // Active por default
                qtvm.CreatedAt = DateTime.Now;      // CreatedAt por default
                qtvm.UsedInReports = 0;             // Não existe em Reports

                //construir All Question tipos para Select dropbox
                qtvm.QuestionTypes = QuestionTypeList.GetSelectListQuestionType(culture.Name, 0);

                // QuestionTypeId por default
                qtvm.QuestionTypeId = Int32.Parse(qtvm.QuestionTypes.FirstOrDefault().Value);

                // criar lista vazia de options
                qtvm.Options = new List<QuestionOptionViewModel>();

                return qtvm;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - InitNewQuestionTemplateAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica em quantos Reports é que a QuestionTemplate está a ser usada
        /// </summary>
        /// <param name="questionTemplateId"></param>
        /// <returns> type="int"</returns>

        internal async Task<int> GetTotalReportsUsingQuestionTemplateAsync(int questionTemplateId)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - GetTotalReportsUsingQuestionTemplateAsync - Entrou para contar reports que usam QuestionTemplate = " + questionTemplateId);

                // verificar se a QuestionTemplate está a ser usada em algum Report
                var responseAllResponseTemplateQuestions = await _mediator.Send(new GetAllReportTemplateQuestionByQuestionCachedQuery() { questionTemplateId = questionTemplateId });
                if (!responseAllResponseTemplateQuestions.Succeeded) return 0;

                return responseAllResponseTemplateQuestions.Data.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - GetTotalReportsUsingQuestionTemplateAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return 0;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// lê da db as options associadas a uma QuestionTemplate
        /// </summary>
        /// <param name="questionTemplateId"></param>
        /// <param name="questionTypeId"></param>
        /// <returns> type="List<QuestionOptionViewModel>"</returns>

        internal async Task<List<QuestionOptionViewModel>> GetOptionsFromQuestionTemplateAsync(int questionTemplateId, int questionTypeId)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - GetOptionsFromQuestionTemplateAsync - Entrou para retornar as options da QuestionTemplate = " + questionTemplateId);

                var qovmList = new List<QuestionOptionViewModel>();

                //construir lista de options da QuestionTemplate se necessário
                if (questionTypeId == QuestionTypeList.QTYPE_ESCOLHA_MULTIPLA)
                {
                    var responseQuestionOption = await _mediator.Send(new GetQuestionOptionByQuestionTemplateIdCachedQuery() { questionTemplateId = questionTemplateId });
                    if (!responseQuestionOption.Succeeded) return null;
                    qovmList = _mapper.Map<List<QuestionOptionViewModel>>(responseQuestionOption.Data);

                    return qovmList.OrderBy(o => o.Order).ToList();
                }
                
                return new List<QuestionOptionViewModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - GetOptionsFromQuestionTemplateAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return new List<QuestionOptionViewModel>();
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// escreve na db as options associadas a uma QuestionTemplate
        /// </summary>
        /// <param name="qtvm"></param>
        /// <param name="questionTypeId"></param>
        /// <returns> type="List<QuestionOptionViewModel>"</returns>

        internal async Task<bool> WriteToDbOptionsFromQuestionTemplateAsync(QuestionTemplateViewModel qtvm)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - WriteToDbOptionsFromQuestionTemplateAsync - Entrou para colocar na db as options da QuestionTemplate = " + qtvm.Id);

                // remover options que possam existir para esta QuestionTemplate
                var responseAllQuestionOptions = await _mediator.Send(new GetQuestionOptionByQuestionTemplateIdCachedQuery() { questionTemplateId = qtvm.Id });
                if (!responseAllQuestionOptions.Succeeded) return false;
                var allQuestionOptionData = _mapper.Map<List<QuestionOptionViewModel>>(responseAllQuestionOptions.Data).AsQueryable();

                foreach (var qovm in allQuestionOptionData)
                {
                    // remover translation
                    var deleteQOResult = await _mediator.Send(new DeleteQuestionOptionCommand() { Id = qovm.Id });
                }


                //status
                var statusOk = true;

                // se for do tipo escolha múltipla, guardar as options
                if (qtvm.QuestionTypeId == QuestionTypeList.QTYPE_ESCOLHA_MULTIPLA && qtvm.Options != null && qtvm.Options.Count() > 0)
                {
                    int order = 1;
                    foreach (var option in qtvm.Options)
                    {
                        option.QuestionTemplateId = qtvm.Id;
                        option.Order = order;
                        option.IsActive = true;

                        var createOptionCommand = _mapper.Map<CreateQuestionOptionCommand>(option);
                        var resultOption = await _mediator.Send(createOptionCommand);

                        if (statusOk) statusOk = resultOption.Succeeded;

                        order++;
                    }
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


        /// <summary>
        /// escreve na db os textos localizados de uma QuestionTemplate
        /// devolve true se succeeded
        /// </summary>
        /// <param name="qtvm"></param>
        /// <returns> type="bool"</returns>

        internal async Task<bool> WriteToDbQuestionTemplateLocalizedAsync(QuestionTemplateViewModel qtvm)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - WriteToDbQuestionTemplateLocalizedAsync - Entrou para escrever na db os textos localizados da QuestionTemplate = " + qtvm.Id);

                // se existirem, remover translations desta QuestionTemplate
                var responseAllQuestionTemplateLocalized = await _mediator.Send(new GetQuestionTemplateLocalizedByQuestionTemplateIdQuery() { questionTemplateId = qtvm.Id });
                if (!responseAllQuestionTemplateLocalized.Succeeded) return false;
                var allQuestionTemplateLocalizedData = _mapper.Map<List<QuestionTemplateLocalizedViewModel>>(responseAllQuestionTemplateLocalized.Data).AsQueryable();

                foreach (var qtlvm in allQuestionTemplateLocalizedData)
                {
                    // remover translation
                    var deleteQTLResult = await _mediator.Send(new DeleteQuestionTemplateLocalizedCommand() { Id = qtlvm.Id });
                }

                // criar translations

                // pt
                var createPtQTLCommand = new CreateQuestionTemplateLocalizedCommand();
                createPtQTLCommand.QuestionTemplateId = qtvm.Id;
                createPtQTLCommand.Language = "pt";
                createPtQTLCommand.QuestionText = qtvm.QuestionText;
                var resultPT = await _mediator.Send(createPtQTLCommand);

                // es
                var createEsQTLCommand = new CreateQuestionTemplateLocalizedCommand();
                createEsQTLCommand.QuestionTemplateId = qtvm.Id;
                createEsQTLCommand.Language = "es";
                createEsQTLCommand.QuestionText = qtvm.EsQuestionText;
                var resultEs = await _mediator.Send(createEsQTLCommand);

                // en
                var createEnQTLCommand = new CreateQuestionTemplateLocalizedCommand();
                createEnQTLCommand.QuestionTemplateId = qtvm.Id;
                createEnQTLCommand.Language = "en";
                createEnQTLCommand.QuestionText = qtvm.EnQuestionText;
                var resultEn = await _mediator.Send(createEnQTLCommand);

                return (resultPT.Succeeded && resultEs.Succeeded && resultEn.Succeeded);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - WriteToDbQuestionTemplateLocalizedAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// lê da db os textos localizados de uma QuestionTemplate
        /// </summary>
        /// <param name="qtvm"></param>
        /// <returns> type="QuestionTemplateViewModel"</returns>

        internal async Task<QuestionTemplateViewModel> GetFromDbQuestionTemplateLocalizedAsync(QuestionTemplateViewModel qtvm)
        {
            try
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - GetFromDbQuestionTemplateLocalizedAsync - Entrou para ler da db os textos localizados da QuestionTemplate = " + qtvm.Id);

                // lista de Question Templates (localized)
                var responseQuestionTemplateLocalizedList = await _mediator.Send(new GetQuestionTemplateLocalizedByQuestionTemplateIdQuery() { questionTemplateId = qtvm.Id });
                if (!responseQuestionTemplateLocalizedList.Succeeded) return qtvm;
                var questionTemplateLocalizedList = _mapper.Map<List<QuestionTemplateLocalizedViewModel>>(responseQuestionTemplateLocalizedList.Data).AsQueryable();

                // atualizar Translations ao ViewModel
                qtvm.QuestionText = questionTemplateLocalizedList.Where(qtl => qtl.Language == "pt").Select(qtl => qtl.QuestionText).FirstOrDefault() ?? "";
                qtvm.EsQuestionText = questionTemplateLocalizedList.Where(qtl => qtl.Language == "es").Select(qtl => qtl.QuestionText).FirstOrDefault() ?? "";
                qtvm.EnQuestionText = questionTemplateLocalizedList.Where(qtl => qtl.Language == "en").Select(qtl => qtl.QuestionText).FirstOrDefault() ?? "";

                return qtvm;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | QuestionTemplate Contoller - GetFromDbQuestionTemplateLocalizedAsync - IO exception vai sair e retornar Error: " + ex.Message);
                return qtvm;
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}