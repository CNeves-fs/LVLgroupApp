using AutoMapper;
using Core.Constants;
using Core.Features.Prazoslimite.Commands.Create;
using Core.Features.Prazoslimite.Commands.Delete;
using Core.Features.Prazoslimite.Commands.Update;
using Core.Features.Prazoslimite.Queries.GetAllCached;
using Core.Features.Prazoslimite.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Claim.Models.Prazolimite;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Claim.Controllers.Prazolimite
{
    [Area("Claim")]
    [Authorize]
    public class PrazolimiteController : BaseController<PrazolimiteController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<PrazolimiteController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public PrazolimiteController(IStringLocalizer<PrazolimiteController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Prazoslimite.View)]
        public IActionResult Index()
        {
            var model = new PrazolimiteViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Prazoslimite.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllPrazoslimiteCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<PrazolimiteViewModel>>(response.Data);
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Prazoslimite.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar novo Prazolimite
            {
                var statusViewModel = new PrazolimiteViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", statusViewModel) });
            }
            else // Editar Prazolimite
            {
                var response = await _mediator.Send(new GetPrazolimiteByIdQuery() { Id = id });
                var statusViewModel = _mapper.Map<PrazolimiteViewModel>(response.Data);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", statusViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Prazoslimite.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, PrazolimiteViewModel status)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new Prazolimite
                    var createPrazolimiteCommand = _mapper.Map<CreatePrazolimiteCommand>(status);
                    var result = await _mediator.Send(createPrazolimiteCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Prazo limite com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    //update Prazolimite
                    var updatePrazolimiteCommand = _mapper.Map<UpdatePrazolimiteCommand>(status);
                    var result = await _mediator.Send(updatePrazolimiteCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Prazo limite com ID"]} {result.Data} {_localizer[" atualizado."]}");
                }

                // return _ViewAll

                var response = await _mediator.Send(new GetAllPrazoslimiteCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<PrazolimiteViewModel>>(response.Data);
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
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", status);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Prazoslimite.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeletePrazolimiteCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Prazo limite com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllPrazoslimiteCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<PrazolimiteViewModel>>(response.Data);
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


        public async Task<JsonResult> LoadPrazoslimite()
        {
            var prazoslimiteResponse = await _mediator.Send(new GetAllPrazoslimiteCachedQuery());

            if (prazoslimiteResponse.Succeeded)
            {
                var prazolimiteViewModel = _mapper.Map<List<PrazolimiteViewModel>>(prazoslimiteResponse.Data);
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(prazolimiteViewModel);
                return Json(new { status = "success", prazoslimitelist = jsonString });
            }
            return Json(new { status = "error" });
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllPrazoslimiteAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var prazolimiteResponse = await mediator.Send(new GetAllPrazoslimiteCachedQuery());
            var prazolimiteViewModel = mapper.Map<List<PrazolimiteViewModel>>(prazolimiteResponse.Data);
            return new SelectList(prazolimiteViewModel, nameof(PrazolimiteViewModel.Id), nameof(PrazolimiteViewModel.Alarme), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<PrazolimiteViewModel> GetPrazolimiteAsync(DateTime limite, IMapper mapper, IMediator mediator)
        {
            var diasToLimite = (int) (limite - DateTime.Now).TotalDays;
            var prazoslimiteResponse = await mediator.Send(new GetAllPrazoslimiteCachedQuery());
            var prazoslimiteViewModel = mapper.Map<List<PrazolimiteViewModel>>(prazoslimiteResponse.Data);
            foreach (PrazolimiteViewModel prazo in prazoslimiteViewModel)
            {
                if (prazo.LimiteMax >= diasToLimite && prazo.LimiteMin <= diasToLimite) return prazo;
            }
            // retornar alerta vermelho
            return prazoslimiteViewModel[0];
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<JsonResult> GetPrazolimiteAsync(int limite)
        {
            var prazoslimiteResponse = await _mediator.Send(new GetAllPrazoslimiteCachedQuery());
            var prazoslimiteViewModel = _mapper.Map<List<PrazolimiteViewModel>>(prazoslimiteResponse.Data);
            foreach (PrazolimiteViewModel prazo in prazoslimiteViewModel)
            {
                if (prazo.LimiteMax >= limite && prazo.LimiteMin <= limite)
                {
                    var jsonStringFound = Newtonsoft.Json.JsonConvert.SerializeObject(new { prazolimite = prazo });
                    return Json(new { status = "success", prazolimite = jsonStringFound });
                }
            }
            // retornar alerta vermelho
            var jsonStringNotFound = Newtonsoft.Json.JsonConvert.SerializeObject(new { prazolimite = prazoslimiteViewModel[0] });
            return Json(new { status = "success", prazolimite = jsonStringNotFound });
        }


        //---------------------------------------------------------------------------------------------------

    }
}