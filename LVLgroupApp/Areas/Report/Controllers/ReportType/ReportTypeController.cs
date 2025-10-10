using AutoMapper;
using Core.Constants;
using Core.Features.ReportTypes.Commands.Create;
using Core.Features.ReportTypes.Commands.Delete;
using Core.Features.ReportTypes.Queries.GetAllCached;
using Core.Features.ReportTypes.Queries.GetById;
using Core.Features.ReportTypesLocalized.Commands.Create;
using Core.Features.ReportTypesLocalized.Commands.Update;
using Core.Features.ReportTypesLocalized.Queries.GetAllCached;
using Core.Features.TiposOcorrenciasLocalized.Commands.Update;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Report.Models.ReportType;
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

namespace LVLgroupApp.Areas.Report.Controllers.ReportType
{
    [Area("Report")]
    [Authorize]
    public class ReportTypeController : BaseController<ReportTypeController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<ReportTypeController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ReportTypeController(IStringLocalizer<ReportTypeController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportType.View)]
        public IActionResult Index()
        {
            var model = new ReportTypeViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportType.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | ReportType Contoller - LoadAll - return lista vazia de ReportTypeViewModel");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de ReportTypes para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.ReportType.View)]
        public async Task<IActionResult> GetReportTypes()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

   
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // lista de tipos de Relatório a devolver ao client
                var viewModel = new List<ReportTypeListViewModel>();
                
                // Culture contains the information of the requested culture
                var culture = _culture.RequestCulture.Culture;

                // lista de tipos de Relatórios
                var response = await _mediator.Send(new GetAllReportTypesCachedQuery());
                if (response.Succeeded)
                {
                    var reportTypesViewModel = _mapper.Map<List<ReportTypeViewModel>>(response.Data);

                    // adicionar Translations ao ViewModel
                    foreach (var item in reportTypesViewModel)
                    {
                        item.Translations = new List<ReportTypeLocalizedViewModel>();
                        var reportTypesLocalizedResponse = await _mediator.Send(new GetReportTypesLocalizedByReportTypeIdQuery() { ReportTypeId = item.Id });
                        item.Translations = _mapper.Map<List<ReportTypeLocalizedViewModel>>(reportTypesLocalizedResponse.Data);
                        
                        var rt = new ReportTypeListViewModel();
                        rt.Id = item.Id;
                        switch (culture.Name)
                        {
                            case "pt":  rt.DefaultName = item.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault(); 
                                        break;
                            case "es":  rt.DefaultName = item.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                                        break;
                            case "en":  rt.DefaultName = item.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                                        break;  
                        }
                        viewModel.Add(rt);
                    }

                    var AllQueryableviewModel = viewModel.AsQueryable();

                    // filtrar searchValue
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        AllQueryableviewModel = AllQueryableviewModel.Where(x => x.DefaultName.Contains(searchValue, StringComparison.OrdinalIgnoreCase));
                    }

                    // ordenar lista
                    if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                    {
                        AllQueryableviewModel = AllQueryableviewModel.OrderBy(sortColumn + " " + sortColumnDirection);
                    }

                    // retornar lista para a datatable
                    recordsTotal = AllQueryableviewModel.Count();
                    var data = AllQueryableviewModel.Skip(skip).Take(pageSize).ToList();

                    var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                    return Ok(jsonData);
                }

                return new ObjectResult(new { status = "error" });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | ReportType Contoller - GetReportTypes - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.ReportType.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar novo ReportType
            {
                var reportTypeViewModel = new ReportTypeViewModel();
                reportTypeViewModel.EditMode = false;
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", reportTypeViewModel) });
            }
            else // Editar ReportType
            {
                var response = await _mediator.Send(new GetReportTypeByIdQuery() { Id = id });
                var reportTypeViewModel = _mapper.Map<ReportTypeViewModel>(response.Data);
                reportTypeViewModel.EditMode = true;

                // adicionar Translations ao ViewModel
                reportTypeViewModel.Translations = new List<ReportTypeLocalizedViewModel>();
                var reportTypesLocalizedResponse = await _mediator.Send(new GetReportTypesLocalizedByReportTypeIdQuery() { ReportTypeId = reportTypeViewModel.Id });
                reportTypeViewModel.Translations = _mapper.Map<List<ReportTypeLocalizedViewModel>>(reportTypesLocalizedResponse.Data);
                reportTypeViewModel.EsName = reportTypeViewModel.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                reportTypeViewModel.EnName = reportTypeViewModel.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                reportTypeViewModel.DefaultName = reportTypeViewModel.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();


                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", reportTypeViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.ReportType.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, ReportTypeViewModel reportType)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new ReportType
                    var createReportTypeCommand = _mapper.Map<CreateReportTypeCommand>(reportType);
                    var result = await _mediator.Send(createReportTypeCommand);
                    if (result.Succeeded)
                    {
                        // criar translations
                        var reportTypeId = result.Data;
                        // pt
                        var createPtRTLCommand = new CreateReportTypeLocalizedCommand();
                        createPtRTLCommand.Name = reportType.DefaultName;
                        createPtRTLCommand.Language = "pt";
                        createPtRTLCommand.ReportTypeId = reportTypeId;
                        var resultPT = await _mediator.Send(createPtRTLCommand);
                        // es
                        var createEsRTLCommand = new CreateReportTypeLocalizedCommand();
                        createEsRTLCommand.Name = reportType.EsName;
                        createEsRTLCommand.Language = "es";
                        createEsRTLCommand.ReportTypeId = reportTypeId;
                        var resultEs = await _mediator.Send(createEsRTLCommand);
                        // en
                        var createEnRTLCommand = new CreateReportTypeLocalizedCommand();
                        createEnRTLCommand.Name = reportType.EnName;
                        createEnRTLCommand.Language = "en";
                        createEnRTLCommand.ReportTypeId = reportTypeId;
                        var resultEn = await _mediator.Send(createEnRTLCommand);

                        id = result.Data;
                        _notify.Success($"{_localizer["Tipo de Relatório com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    //update ReportType
                    await UpdateReportTypeLocalized(id, reportType);

                    _notify.Information($"{_localizer["Tipo de Relatório com ID"]} {id} {_localizer[" atualizado."]}");
                }

                // return _ViewAll

                var response = await _mediator.Send(new GetAllReportTypesCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<ReportTypeViewModel>>(response.Data);
                    // adicionar Translations ao ViewModel
                    foreach (var item in viewModel)
                    {
                        item.Translations = new List<ReportTypeLocalizedViewModel>();
                        var reportTypesLocalizedResponse = await _mediator.Send(new GetReportTypesLocalizedByReportTypeIdQuery() { ReportTypeId = item.Id });
                        item.Translations = _mapper.Map<List<ReportTypeLocalizedViewModel>>(reportTypesLocalizedResponse.Data);
                        item.EsName = item.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                        item.EnName = item.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                        item.DefaultName = item.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();
                    }

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
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", reportType);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.ReportType.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteReportTypeCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Tipo de relatório com ID"]} {id} {_localizer[" removido."]}");

                // return _ViewAll
                var viewModel = new List<ReportTypeViewModel>();
                var response = await _mediator.Send(new GetAllReportTypesCachedQuery());
                if (response.Succeeded)
                {
                    viewModel = _mapper.Map<List<ReportTypeViewModel>>(response.Data);
                    // adicionar Translations ao ViewModel
                    foreach (var item in viewModel)
                    {
                        item.Translations = new List<ReportTypeLocalizedViewModel>();
                        var reportTypesLocalizedResponse = await _mediator.Send(new GetReportTypesLocalizedByReportTypeIdQuery() { ReportTypeId = item.Id });
                        item.Translations = _mapper.Map<List<ReportTypeLocalizedViewModel>>(reportTypesLocalizedResponse.Data);
                        item.EsName = item.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                        item.EnName = item.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                        item.DefaultName = item.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();
                    }

                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
            }
            else
            {
                _notify.Error(deleteCommand.Message);
                return new JsonResult(new { isValid = true, html = "" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria uma lista de Tipos de relatórios em Json
        /// </summary>
        /// <returns>List<ReportTypeViewModel></returns>

        public async Task<JsonResult> LoadReportTypes()
        {
            var response = await _mediator.Send(new GetAllReportTypesCachedQuery());
            if (response.Succeeded)
            {
                var allReportTypes = _mapper.Map<List<ReportTypeViewModel>>(response.Data);
                // adicionar Translations ao allReportTypes
                foreach (var item in allReportTypes)
                {
                    item.Translations = new List<ReportTypeLocalizedViewModel>();
                    var reportTypesLocalizedResponse = await _mediator.Send(new GetReportTypesLocalizedByReportTypeIdQuery() { ReportTypeId = item.Id });
                    item.Translations = _mapper.Map<List<ReportTypeLocalizedViewModel>>(reportTypesLocalizedResponse);
                    item.EsName = item.Translations.Where(t => t.Language == "es").Select(t => t.Name).FirstOrDefault();
                    item.EnName = item.Translations.Where(t => t.Language == "en").Select(t => t.Name).FirstOrDefault();
                    item.DefaultName = item.Translations.Where(t => t.Language == "pt").Select(t => t.Name).FirstOrDefault();
                }

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { tipoocorrenciasList = allReportTypes });
                return Json(jsonString);

            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza as strings de tradução de um ReportType
        /// </summary>
        /// <returns>bool</returns>

        internal async Task<bool> UpdateReportTypeLocalized(int reportTypeId, ReportTypeViewModel model)
        {
            // ler as traduções existentes
            var reportTypeLocalizedListQuery = new GetReportTypesLocalizedByReportTypeIdQuery() { ReportTypeId = reportTypeId };
            var reportTypeLocalizedListResult = await _mediator.Send(reportTypeLocalizedListQuery);
            if (reportTypeLocalizedListResult.Succeeded)
            {
                var reportTypeLocalizedList = _mapper.Map<List<ReportTypeLocalizedViewModel>>(reportTypeLocalizedListResult.Data);

                //atualizar cada uma das traduções
                foreach (var trad in reportTypeLocalizedList)
                {
                    switch (trad.Language)
                    {
                        case "pt":
                            trad.Name = model.DefaultName;
                            break;
                        case "es":
                            trad.Name = model.EsName;
                            break;
                        case "en":
                            trad.Name = model.EnName;
                            break;
                    }

                    //atualizar a tradução
                    var updateRTLCommand = _mapper.Map<UpdateReportTypeLocalizedCommand>(trad);
                    var result = await _mediator.Send(updateRTLCommand);
                }
                return true;
            }
            return false;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllReportTypesAsync(string lang, int selectedId, IMapper mapper, IMediator mediator)
        {
            var typesLocalizedResponse = await mediator.Send(new GetAllReportTypesLocalizedCachedQuery());
            var typesLocalizedViewModel = mapper.Map<List<ReportTypeLocalizedViewModel>>(typesLocalizedResponse.Data).AsQueryable();
            typesLocalizedViewModel = typesLocalizedViewModel.Where(rtl => rtl.Language == lang);   
            return new SelectList(typesLocalizedViewModel, nameof(ReportTypeLocalizedViewModel.Id), nameof(ReportTypeLocalizedViewModel.Name), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve o nome localized do reportTypeId
        /// </summary>
        /// <returns>string=nome</returns>

        public static async Task<string> GetReportTypeNomeAsync(int reportTypeId, string lang, IMediator mediator, IMapper mapper)
        {
            var reportTypesLocalizedResponse = await mediator.Send(new GetReportTypesLocalizedByReportTypeIdQuery() { ReportTypeId = reportTypeId });
            var reportTypeLocalized = reportTypesLocalizedResponse.Data.Where(t => t.Language == lang).FirstOrDefault();
            var reportTypeLocalizedViewModel = mapper.Map<ReportTypeLocalizedViewModel>(reportTypeLocalized);

            return reportTypeLocalizedViewModel.Name;
        }


        //---------------------------------------------------------------------------------------------------

    }
}