using AutoMapper;
using Core.Constants;
using Core.Entities.Select2;
using Core.Features.Lojas.Queries.GetById;
using Core.Features.Mercados.Commands.Create;
using Core.Features.Mercados.Commands.Delete;
using Core.Features.Mercados.Commands.Update;
using Core.Features.Mercados.Queries.GetAllCached;
using Core.Features.Mercados.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Models.Mercado;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Business.Controllers.Mercado
{
    [Area("Business")]
    [Authorize]
    public class MercadoController : BaseController<MercadoController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<MercadoController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public MercadoController(IStringLocalizer<MercadoController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Mercados.View)]
        public IActionResult Index()
        {
            var model = new MercadoViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Mercados.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllMercadosCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<MercadoViewModel>>(response.Data);
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Mercados.Edit)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            var mercadosResponse = await _mediator.Send(new GetAllMercadosCachedQuery());

            if (id == 0)
            {
                var mercadoViewModel = new MercadoViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", mercadoViewModel) });
            }
            else
            {
                var response = await _mediator.Send(new GetMercadoByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var mercadoViewModel = _mapper.Map<MercadoViewModel>(response.Data);
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", mercadoViewModel) });
                }
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Mercados.Edit)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, MercadoViewModel mercado)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {                    
                    var createMercadoCommand = _mapper.Map<CreateMercadoCommand>(mercado);
                    var result = await _mediator.Send(createMercadoCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Mercado com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    var updateMercadoCommand = _mapper.Map<UpdateMercadoCommand>(mercado);
                    var result = await _mediator.Send(updateMercadoCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Mercado com ID"]} {result.Data} {_localizer[" atualizado."]}");
                }
                // return _ViewAll
                var response = await _mediator.Send(new GetAllMercadosCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<MercadoViewModel>>(response.Data);
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
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", mercado);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Mercados.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteMercadoCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Mercado com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllMercadosCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<MercadoViewModel>>(response.Data);
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


        public async Task<JsonResult> LoadAllMercados()
        {

            var mercadosResponse = await _mediator.Send(new GetAllMercadosCachedQuery());

            if (mercadosResponse.Succeeded)
            {
                var mercadosViewModel = _mapper.Map<List<MercadoViewModel>>(mercadosResponse.Data);
                var mercados = new SelectList(mercadosViewModel, nameof(MercadoViewModel.Id), nameof(MercadoViewModel.Nome), null, null);

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { mercadosList = mercados });
                return Json(jsonString);
            }

            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllMercadosAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var mercadosResponse = await mediator.Send(new GetAllMercadosCachedQuery());
            var mercadosViewModel = mapper.Map<List<MercadoViewModel>>(mercadosResponse.Data);
            return new SelectList(mercadosViewModel, nameof(MercadoViewModel.Id), nameof(MercadoViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListOneMercadoAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var mercadosResponse = await mediator.Send(new GetAllMercadosCachedQuery());
            var mercadosViewModel = mapper.Map<List<MercadoViewModel>>(mercadosResponse.Data);
            mercadosViewModel.RemoveAll(e => e.Id != selectedId);
            return new SelectList(mercadosViewModel, nameof(MercadoViewModel.Id), nameof(MercadoViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<int> GetMercadoIdAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                if (lojaResponse.Data != null && lojaResponse.Data.MercadoId != null)
                {
                    return (int)lojaResponse.Data.MercadoId;
                }
            }
            return 0;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<string> GetMercadoNomeAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                if (lojaResponse.Data != null && lojaResponse.Data.MercadoId != null)
                {
                    var mercadoResponse = await mediator.Send(new GetMercadoByIdQuery() { Id = (int)lojaResponse.Data.MercadoId });
                    return mercadoResponse.Data.Nome;
                }
            }
            return string.Empty;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<MercadoViewModel> GetMercadoAsync(int mercadoId, IMapper mapper, IMediator mediator)
        {
            if (mercadoId > 0)
            {
                var mercadoResponse = await mediator.Send(new GetMercadoByIdQuery() { Id = mercadoId });
                return mapper.Map<MercadoViewModel>(mercadoResponse.Data);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetListAllMercados(string term)
        {
            var mercadosResponse = await _mediator.Send(new GetAllMercadosCachedQuery());
            var mercadosViewModel = _mapper.Map<List<MercadoViewModel>>(mercadosResponse.Data);
            List<Select2Item> data = new List<Select2Item>();

            data.Add(new Select2Item
            {
                id = 0,
                text = _localizer["Todas os mercados"],
                textColor = "",
                backgroundColor = ""
                //selected = (item.Id == selectedId)
            });

            if (!string.IsNullOrEmpty(term))
            {
                var search = mercadosViewModel.Where(g => g.Nome.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
                foreach (var item in search)
                {
                    data.Add(new Select2Item
                    {
                        id = item.Id,
                        text = item.Nome ?? ""
                        //selected = (item.Id == selectedId)
                    });
                }
            }
            else
            {
                foreach (var item in mercadosViewModel)
                {
                    data.Add(new Select2Item
                    {
                        id = item.Id,
                        text = item.Nome ?? ""
                        //selected = (item.Id == selectedId)
                    });
                }
            };

            return Ok(data);
        }


        //---------------------------------------------------------------------------------------------------

    }
}