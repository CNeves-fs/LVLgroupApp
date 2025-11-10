using AutoMapper;
using Core.Constants;
using Core.Entities.Select2;
using Core.Features.Genders.Commands.Create;
using Core.Features.Genders.Commands.Delete;
using Core.Features.Genders.Commands.Update;
using Core.Features.Genders.Queries.GetAllCached;
using Core.Features.Genders.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Artigo.Models.Gender;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Artigo.Controllers.Gender
{
    [Area("Artigo")]
    [Authorize]
    public class GenderController : BaseController<GenderController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<GenderController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public GenderController(IStringLocalizer<GenderController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Genders.View)]
        public IActionResult Index()
        {
            var model = new GenderViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Genders.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllGendersCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<GenderViewModel>>(response.Data);
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Genders.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar novo Gender
            {
                var genderViewModel = new GenderViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", genderViewModel) });
            }
            else // Editar Gender
            {
                var response = await _mediator.Send(new GetGenderByIdQuery() { Id = id });
                var genderViewModel = _mapper.Map<GenderViewModel>(response.Data);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", genderViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Genders.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, GenderViewModel gender)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new Gender
                    var createGenderCommand = _mapper.Map<CreateGenderCommand>(gender);
                    var result = await _mediator.Send(createGenderCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Género com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    //update Gender
                    var updateGenderCommand = _mapper.Map<UpdateGenderCommand>(gender);
                    var result = await _mediator.Send(updateGenderCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Género com ID"]} {result.Data} {_localizer[" atualizado."]}");
                }

                // return _ViewAll

                var response = await _mediator.Send(new GetAllGendersCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<GenderViewModel>>(response.Data);
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
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", gender);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Genders.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteGenderCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Género com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllGendersCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<GenderViewModel>>(response.Data);
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


        public async Task<IActionResult> OnGetListAllGenders(string term)
        {
            var gendersResponse = await _mediator.Send(new GetAllGendersCachedQuery());
            var gendersViewModel = _mapper.Map<List<GenderViewModel>>(gendersResponse.Data);
            List<Select2Item> data = new List<Select2Item>();

            data.Add(new Select2Item
            {
                id = 0,
                text = _localizer["Todos os genders"],
                textColor = "",
                backgroundColor = ""
                //selected = (item.Id == selectedId)
            });

            if (!string.IsNullOrEmpty(term))
            {
                var search = gendersViewModel.Where(g => g.Nome.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
                foreach (var item in search)
                {
                    data.Add(new Select2Item
                    {
                        id = item.Id,
                        text = item.Nome == null ? "" : item.Nome
                        //selected = (item.Id == selectedId)
                    });
                }
            }
            else
            {
                foreach (var item in gendersViewModel)
                {
                    data.Add(new Select2Item
                    {
                        id = item.Id,
                        text = item.Nome == null ? "" : item.Nome
                        //selected = (item.Id == selectedId)
                    });
                }
            };

            return Ok(data);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<JsonResult> LoadGenders()
        {

            var gendersResponse = await _mediator.Send(new GetAllGendersCachedQuery());

            if (gendersResponse.Succeeded)
            {
                var genderViewModel = _mapper.Map<List<GenderViewModel>>(gendersResponse.Data);
                var genders = new SelectList(genderViewModel, nameof(GenderViewModel.Id), nameof(GenderViewModel.Nome), null, null);

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { gendersList = genders });
                return Json(jsonString);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<GenderViewModel> GetGenderViewModelAsync(int genderId, IMapper mapper, IMediator mediator)
        {
            var genderResponse = await mediator.Send(new GetGenderByIdQuery() { Id = genderId});
            return mapper.Map<GenderViewModel>(genderResponse.Data);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllGendersAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var gendersResponse = await mediator.Send(new GetAllGendersCachedQuery());
            var gendersViewModel = mapper.Map<List<GenderViewModel>>(gendersResponse.Data);
            return new SelectList(gendersViewModel, nameof(GenderViewModel.Id), nameof(GenderViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------

    }
}