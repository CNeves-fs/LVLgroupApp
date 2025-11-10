using AutoMapper;
using Core.Constants;
using Core.Features.Lojas.Commands.Create;
using Core.Features.Lojas.Commands.Delete;
using Core.Features.Lojas.Commands.Update;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetById;
using Core.Features.Gruposlojas.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Mercado;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Models.Loja;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

namespace LVLgroupApp.Areas.Business.Controllers.Loja
{
    [Area("Business")]
    [Authorize]
    public class LojaController : BaseController<LojaController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<LojaController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public LojaController(IStringLocalizer<LojaController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Lojas.View)]
        public IActionResult Index()
        {
            var model = new LojaViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Lojas.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllLojasCachedQuery());
            var viewModel = _mapper.Map<List<LojaViewModel>>(response.Data);
            foreach (var item in viewModel)
            {
                item.GrupolojaNome = await GrupolojaController.GetGrupolojaNomeAsync(item.GrupolojaId, _mapper, _mediator);
                item.EmpresaNome = await EmpresaController.GetEmpresaNomeAsync(item.Id, _mapper, _mediator);
                item.MercadoNome = await MercadoController.GetMercadoNomeAsync(item.Id, _mapper, _mediator);
            }
            return PartialView("_ViewAll", viewModel);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Lojas.Edit)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            //criar modelView para retornar
            var lojaViewModel =  new LojaViewModel();

            if (id > 0) // editar loja
            {
                var response = await _mediator.Send(new GetLojaByIdQuery() { Id = id });
                lojaViewModel = _mapper.Map<LojaViewModel>(response.Data);

                //construir All Mercados para Select dropbox
                lojaViewModel.MercadoId = await MercadoController.GetMercadoIdAsync(id, _mapper, _mediator);
                lojaViewModel.Mercados = await MercadoController.GetSelectListAllMercadosAsync(lojaViewModel.MercadoId, _mapper, _mediator);

                //construir All Empresas para Select dropbox
                lojaViewModel.EmpresaId = await EmpresaController.GetEmpresaIdAsync(id, _mapper, _mediator);
                lojaViewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(lojaViewModel.EmpresaId, _mapper, _mediator);

                //construir gruposLojas de EmpresaId para Select dropbox
                lojaViewModel.Gruposlojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(lojaViewModel.EmpresaId, lojaViewModel.GrupolojaId, _mapper, _mediator);
            }
            else // criar loja
            {
                //construir All Mercados para Select dropbox
                lojaViewModel.Mercados = await MercadoController.GetSelectListAllMercadosAsync(0, _mapper, _mediator);

                //construir All Empresas para Select dropbox
                lojaViewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, _mapper, _mediator);

                //construir gruposLojas de primera Empresa da lista para Select dropbox
                var empId = Int32.Parse(lojaViewModel.Empresas.ElementAt(0).Value);
                lojaViewModel.Gruposlojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(empId, 0, _mapper, _mediator);
            }
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", lojaViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Lojas.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, LojaViewModel loja)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new Loja
                    var createLojaCommand = _mapper.Map<CreateLojaCommand>(loja);
                    var result = await _mediator.Send(createLojaCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Loja com ID"]} {result.Data} {_localizer[" criada."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    //update Loja
                    var updateLojaCommand = _mapper.Map<UpdateLojaCommand>(loja);
                    var result = await _mediator.Send(updateLojaCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Loja com ID"]} {result.Data} {_localizer[" atualizada."]}");
                }

                // return _ViewAll
                var response = await _mediator.Send(new GetAllLojasCachedQuery());
                var viewModel = _mapper.Map<List<LojaViewModel>>(response.Data);
                foreach (var item in viewModel)
                {
                    item.GrupolojaNome = await GrupolojaController.GetGrupolojaNomeAsync(item.GrupolojaId, _mapper, _mediator);
                    item.EmpresaNome = await EmpresaController.GetEmpresaNomeAsync(item.Id, _mapper, _mediator);
                    item.MercadoNome = await MercadoController.GetMercadoNomeAsync(item.Id, _mapper, _mediator);
                }
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", loja);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Lojas.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteLojaCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Loja com ID"]} {id} {_localizer[" removida."]}");
                var response = await _mediator.Send(new GetAllLojasCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<LojaViewModel>>(response.Data);
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


        public async Task<JsonResult> LoadLojas(int empresaId)
        {
            if (empresaId > 0)
            {
                var lojasResponse = await _mediator.Send(new GetLojasByEmpresaIdCachedQuery() { empresaId = empresaId });

                if (lojasResponse.Succeeded)
                {
                    var lojaViewModel = _mapper.Map<List<LojaViewModel>>(lojasResponse.Data);
                    var lojas = new SelectList(lojaViewModel, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), null, null);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { lojasList = lojas });
                    return Json(jsonString);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<JsonResult> LoadLojasInMercado(int mercadoId)
        {
            if (mercadoId > 0)
            {
                var lojasResponse = await _mediator.Send(new GetLojasByMercadoIdCachedQuery() { mercadoId = mercadoId });

                if (lojasResponse.Succeeded)
                {
                    var lojaViewModel = _mapper.Map<List<LojaViewModel>>(lojasResponse.Data);
                    var lojas = new SelectList(lojaViewModel, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), null, null);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { lojasList = lojas });
                    return Json(jsonString);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<JsonResult> LoadLojasInGrupoloja(int grupolojaId)
        {
            if (grupolojaId > 0)
            {
                var lojasResponse = await _mediator.Send(new GetLojasByGrupolojaIdCachedQuery() { grupolojaId = grupolojaId });

                if (lojasResponse.Succeeded)
                {
                    var lojaViewModel = _mapper.Map<List<LojaViewModel>>(lojasResponse.Data);
                    var lojas = new SelectList(lojaViewModel, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), null, null);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { lojasList = lojas });
                    return Json(jsonString);
                }
            }
            //return null;
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(new { lojasList = ""}));
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<List<Core.Entities.Business.Loja>> GetAllLojasFromEmpresaAsync (int empresaId, IMapper mapper, IMediator mediator)
        {
            if (empresaId > 0)
            {
                var lojasResponse = await mediator.Send(new GetLojasByEmpresaIdCachedQuery() { empresaId = empresaId });

                if (lojasResponse.Succeeded)
                {
                    return mapper.Map<List<Core.Entities.Business.Loja>>(lojasResponse.Data);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<List<Core.Entities.Business.Loja>> GetAllLojasFromMercadoAsync(int mercadoId, IMapper mapper, IMediator mediator)
        {
            if (mercadoId > 0)
            {
                var lojasResponse = await mediator.Send(new GetLojasByMercadoIdCachedQuery() { mercadoId = mercadoId });

                if (lojasResponse.Succeeded)
                {
                    return mapper.Map<List<Core.Entities.Business.Loja>>(lojasResponse.Data);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllLojasAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var lojasResponse = await mediator.Send(new GetAllLojasCachedQuery());
            var lojasViewModel = mapper.Map<List<LojaViewModel>>(lojasResponse.Data);
            return new SelectList(lojasViewModel, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListLojasFromMercadoAsync(int mercadoId, int selectedId, IMapper mapper, IMediator mediator)
        {
            if (mercadoId > 0)
            {
                var lojasResponse = await mediator.Send(new GetLojasByMercadoIdCachedQuery() { mercadoId = mercadoId });
                var lojasViewModel = mapper.Map<List<LojaViewModel>>(lojasResponse.Data);
                return new SelectList(lojasViewModel, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), selectedId, null);
            }
            return new SelectList(null, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListLojasFromEmpresaAsync(int empresaId, int selectedId, IMapper mapper, IMediator mediator)
        {
            if (empresaId > 0)
            {
                var lojasResponse = await mediator.Send(new GetLojasByEmpresaIdCachedQuery() { empresaId = empresaId });
                var lojasViewModel = mapper.Map<List<LojaViewModel>>(lojasResponse.Data);
                return new SelectList(lojasViewModel, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), selectedId, null);
            }
            return new SelectList(null, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListLojasFromGrupolojaAsync(int grupolojaId, int selectedId, IMapper mapper, IMediator mediator)
        {
            if (grupolojaId > 0)
            {
                var lojasResponse = await mediator.Send(new GetLojasByGrupolojaIdCachedQuery() { grupolojaId = grupolojaId });
                var lojasViewModel = mapper.Map<List<LojaViewModel>>(lojasResponse.Data);
                return new SelectList(lojasViewModel, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), selectedId, null);
            }
            return null;
            //return new SelectList(null, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListOneLojaAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var lojasResponse = await mediator.Send(new GetAllLojasCachedQuery());
            var lojasViewModel = mapper.Map<List<LojaViewModel>>(lojasResponse.Data);
            lojasViewModel.RemoveAll(l => l.Id != selectedId);
            return new SelectList(lojasViewModel, nameof(LojaViewModel.Id), nameof(LojaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<string> GetLojaNomeAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                var lojaViewModel = mapper.Map<LojaViewModel>(lojaResponse.Data);
                return lojaViewModel.Nome;
            }
            return string.Empty;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<LojaViewModel> GetLojaAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                var grupoResponse = await mediator.Send(new GetGrupolojaByIdQuery() { Id = lojaResponse.Data.GrupolojaId });
                var lojaViewModel = mapper.Map<LojaViewModel>(lojaResponse.Data);
                lojaViewModel.EmpresaId = grupoResponse.Data.EmpresaId;
                return lojaViewModel;
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<bool> GetFechoClaimEmLojaAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                var lojaViewModel = mapper.Map<LojaViewModel>(lojaResponse.Data);
                return lojaViewModel.FechoClaimEmLoja;
            }
            return false;
        }


        //---------------------------------------------------------------------------------------------------

    }
}