using AutoMapper;
using Core.Constants;
using Core.Features.Fototags.Commands.Create;
using Core.Features.Fototags.Commands.Delete;
using Core.Features.Fototags.Commands.Update;
using Core.Features.Fototags.Queries.GetAllCached;
using Core.Features.Fototags.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Claim.Controllers.Claim;
using LVLgroupApp.Areas.Claim.Models.Fototag;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Claim.Controllers.Fototag
{
    [Area("Claim")]
    [Authorize]
    public class FototagController : BaseController<FototagController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<FototagController> _localizer;

        //private readonly ILogger<ClaimController> _logger;


        //---------------------------------------------------------------------------------------------------


        public FototagController(IStringLocalizer<FototagController> localizer)
        {
            _localizer = localizer;
            //_logger = logger;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Fototags.View)]
        public IActionResult Index()
        {
            var model = new FototagViewModel();
            _logger.LogInformation("Fototag Contoller - Index - return viewModel");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Fototags.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllFototagsCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<FototagViewModel>>(response.Data);
                _logger.LogInformation("Fototag Contoller - LoadAll - retorna lista de tags");

                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Fototags.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar novo Fototag
            {
                var fototagViewModel = new FototagViewModel();
                _logger.LogInformation("Fototag Contoller - OnGetCreateOrEdit - retorna _CreateOrEdit para criar tag");
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", fototagViewModel) });
            }
            else // Editar Fototag
            {
                var response = await _mediator.Send(new GetFototagByIdQuery() { Id = id });
                var fototagViewModel = _mapper.Map<FototagViewModel>(response.Data);
                _logger.LogInformation("Fototag Contoller - OnGetCreateOrEdit - retorna _CreateOrEdit para editar tag");
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", fototagViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Fototags.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, FototagViewModel fototag)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - ModelState Is Valid");
                if (id == 0)
                {
                    _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - nova tag vai ser criada");
                    //create new Fototag
                    var createFototagCommand = _mapper.Map<CreateFototagCommand>(fototag);
                    var result = await _mediator.Send(createFototagCommand);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - nova tag criada com sucesso");
                        id = result.Data;
                        _notify.Success($"{_localizer["Etiqueta de foto com ID"]} {result.Data} {_localizer[" criada."]}");
                    }
                    else
                    {
                        _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - nova tag criada com erro");
                        _notify.Error(result.Message);
                    }
                }
                else
                {
                    //update Fototag
                    _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - nova tag vai ser atualizada");
                    var updateFototagCommand = _mapper.Map<UpdateFototagCommand>(fototag);
                    var result = await _mediator.Send(updateFototagCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Etiqueta de foto com ID"]} {result.Data} {_localizer[" atualizada."]}");
                    _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - tag atualizada");
                }

                // return _ViewAll
                _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - preparar para retornar lista de tags");
                var response = await _mediator.Send(new GetAllFototagsCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<FototagViewModel>>(response.Data);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - retorna _ViewAll com lista de tags");
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - erro ao ler lista de tags");
                    return null;
                }
            }
            else
            {
                _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - ModelState Not Valid");
                _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - Total erros = " + ModelState.ErrorCount);

                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - Error Key = " + key);
                    }
                }

                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", fototag);
                _logger.LogInformation("Fototag Contoller - OnPostCreateOrEdit - return _CreateOrEdit para criar/editar");
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Fototags.Create)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteFototagCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Etiqueta de foto com ID"]} {id} {_localizer[" removida."]}");
                var response = await _mediator.Send(new GetAllFototagsCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<FototagViewModel>>(response.Data);
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


        public async Task<JsonResult> LoadFototags()
        {

            var fototagsResponse = await _mediator.Send(new GetAllFototagsCachedQuery());

            if (fototagsResponse.Succeeded)
            {
                var fototagViewModel = _mapper.Map<List<FototagViewModel>>(fototagsResponse.Data);
                var fototags = new SelectList(fototagViewModel, nameof(FototagViewModel.Id), nameof(FototagViewModel.Tag), null, null);

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { tagsList = fototags });
                return Json(jsonString);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllFototagsAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var fototagResponse = await mediator.Send(new GetAllFototagsCachedQuery());
            var fototagsViewModel = mapper.Map<List<FototagViewModel>>(fototagResponse.Data);
            return new SelectList(fototagsViewModel, nameof(FototagViewModel.Id), nameof(FototagViewModel.Tag), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<string> GetFototagAsync(int tagId, IMapper mapper, IMediator mediator)
        {
            var fototagResponse = await mediator.Send(new GetFototagByIdQuery() { Id = tagId });
            if (fototagResponse.Succeeded) return fototagResponse.Data.Tag;
            return string.Empty;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<FototagListModel>> GetListAllFototagsAsync(int selectedId)
        {
            List<FototagListModel> fTags = new List<FototagListModel>();
            var fototagResponse = await _mediator.Send(new GetAllFototagsCachedQuery());
            var fototagsViewModel = _mapper.Map<List<FototagViewModel>>(fototagResponse.Data);
            foreach (FototagViewModel tag in fototagsViewModel)
            {
                fTags.Add(new FototagListModel
                {
                    value = tag.Id,
                    text = tag.Tag
                });
            }
            return fTags;
        }


        //---------------------------------------------------------------------------------------------------

    }
}