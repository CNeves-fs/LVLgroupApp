using AutoMapper;
using Core.Constants;
using Core.Entities.Identity;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Empresas.Queries.GetById;
using Core.Features.Gruposlojas.Commands.Create;
using Core.Features.Gruposlojas.Commands.Delete;
using Core.Features.Gruposlojas.Commands.Update;
using Core.Features.Gruposlojas.Queries.GetAllCached;
using Core.Features.Gruposlojas.Queries.GetById;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Models.Empresa;
using LVLgroupApp.Areas.Business.Models.Grupoloja;
using LVLgroupApp.Areas.Business.Models.Loja;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Business.Controllers.Grupoloja
{
    [Area("Business")]
    [Authorize]
    public class GrupolojaController : BaseController<GrupolojaController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<GrupolojaController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public GrupolojaController(IStringLocalizer<GrupolojaController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Gruposlojas.View)]
        public IActionResult Index()
        {
            var model = new GrupolojaViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Gruposlojas.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllGruposlojasCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<GrupolojaViewModel>>(response.Data);
                foreach (var item in viewModel)
                {
                    if (item.EmpresaId > 0)
                    {
                        var response_empId = await _mediator.Send(new GetEmpresaByIdQuery() { Id = (int)item.EmpresaId });
                        if (response_empId.Succeeded)
                        {
                            item.EmpresaNome = response_empId.Data.Nome;
                        }
                    }
                }
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Gruposlojas.Edit)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            var empresasResponse = await _mediator.Send(new GetAllEmpresasCachedQuery());

            if (id == 0) // Criar novo Grupoloja
            {
                var grupolojaViewModel = new GrupolojaViewModel();
                grupolojaViewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(0, _mapper, _mediator);

                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", grupolojaViewModel) });
            }
            else // Editar Grupoloja
            {
                var response = await _mediator.Send(new GetGrupolojaByIdQuery() { Id = id });
                var grupolojaViewModel = _mapper.Map<GrupolojaViewModel>(response.Data);
                grupolojaViewModel.Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(response.Data.Id, _mapper, _mediator);

                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", grupolojaViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Gruposlojas.Edit)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, GrupolojaViewModel grupoloja)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new Grupoloja
                    var createGrupolojaCommand = _mapper.Map<CreateGrupolojaCommand>(grupoloja);
                    var result = await _mediator.Send(createGrupolojaCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Grupo de lojas com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    //update Grupoloja
                    var updateGrupolojaCommand = _mapper.Map<UpdateGrupolojaCommand>(grupoloja);
                    var result = await _mediator.Send(updateGrupolojaCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Grupo de Lojas with ID"]} {result.Data} {_localizer[" atualizado."]}");
                }

                // return _ViewAll

                var response = await _mediator.Send(new GetAllGruposlojasCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<GrupolojaViewModel>>(response.Data);
                    foreach (var item in viewModel)
                    {
                        if (item.EmpresaId > 0)
                        {
                            var response_empId = await _mediator.Send(new GetEmpresaByIdQuery() { Id = (int)item.EmpresaId });
                            if (response_empId.Succeeded)
                            {
                                item.EmpresaNome = response_empId.Data.Nome;
                            }
                        }
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
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", grupoloja);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Gruposlojas.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteGrupolojaCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Grupo de lojas com ID"]} {id} {_localizer[" remivido."]}");
                var response = await _mediator.Send(new GetAllGruposlojasCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<GrupolojaViewModel>>(response.Data);
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


        public async Task<JsonResult> LoadGruposlojas(int empresaId)
        {
            if (empresaId > 0)
            {
                var gruposlojasResponse = await _mediator.Send(new GetAllGruposlojasByEmpresaIdCachedQuery() { empresaId = empresaId });

                if (gruposlojasResponse.Succeeded)
                {
                    var grupolojaViewModel = _mapper.Map<List<GrupolojaViewModel>>(gruposlojasResponse.Data);
                    var gruposlojas = new SelectList(grupolojaViewModel, nameof(GrupolojaViewModel.Id), nameof(GrupolojaViewModel.Nome), null, null);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { gruposlojasList = gruposlojas });
                    return Json(jsonString);
                }
            }
            //return null;
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(new { gruposlojasList = "" }));
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllGruposlojasAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var gruposlojasResponse = await mediator.Send(new GetAllGruposlojasCachedQuery());
            var gruposlojasViewModel = mapper.Map<List<GrupolojaViewModel>>(gruposlojasResponse.Data);
            return new SelectList(gruposlojasViewModel, nameof(GrupolojaViewModel.Id), nameof(GrupolojaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListGruposlojasFromEmpresaAsync(int empresaId, int selectedId, IMapper mapper, IMediator mediator)
        {
            if (empresaId > 0)
            {
                var gruposlojasResponse = await mediator.Send(new GetAllGruposlojasByEmpresaIdCachedQuery() { empresaId = empresaId });
                var gruposlojasViewModel = mapper.Map<List<GrupolojaViewModel>>(gruposlojasResponse.Data);
                return new SelectList(gruposlojasViewModel, nameof(GrupolojaViewModel.Id), nameof(GrupolojaViewModel.Nome), selectedId, null);
            }
            return null;
            //return new SelectList(null, nameof(GrupolojaViewModel.Id), nameof(GrupolojaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListOneGrupoLojaAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var gruposlojasResponse = await mediator.Send(new GetAllGruposlojasCachedQuery());
            var gruposlojasViewModel = mapper.Map<List<GrupolojaViewModel>>(gruposlojasResponse.Data);
            gruposlojasViewModel.RemoveAll(g => g.Id != selectedId);
            return new SelectList(gruposlojasViewModel, nameof(GrupolojaViewModel.Id), nameof(GrupolojaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para criar uma SelectList de um item com Empresa
        /// </summary>
        /// <param name="cRole"></param>
        /// <param name="selectedId"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<SelectList> GetGrupoLojaByRoleAsync(CurrentRole cRole, int selectedId)
        {
            if (cRole.IsSuperAdmin || cRole.IsAdmin || cRole.IsRevisor)
            {
                //construir SelectList para qualquer Empresa
                return await EmpresaController.GetSelectListAllEmpresasAsync(selectedId, _mapper, _mediator);

            }
            //cRole.IsSupervisor || cRole.IsGerenteLoja || cRole.IsColaborador || cRole.IsBasic
            //construir SelectList para Empresa em selectedId
            return await EmpresaController.GetSelectListOneEmpresaAsync(selectedId, _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<int> GetGrupolojaFromLojaIdAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                var lojaViewModel = mapper.Map<LojaViewModel>(lojaResponse.Data);
                return lojaViewModel.GrupolojaId;
            }
            return 0;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<string> GetGrupolojaNomeAsync(int grupolojaId, IMapper mapper, IMediator mediator)
        {
            if (grupolojaId > 0)
            {
                var grupolojaResponse = await mediator.Send(new GetGrupolojaByIdQuery() { Id = grupolojaId });
                var grupolojaViewModel = mapper.Map<GrupolojaViewModel>(grupolojaResponse.Data);
                return grupolojaViewModel.Nome;
            }
            return string.Empty;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<string> GetGrupolojaNomeFromLojaIdAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                var lojaViewModel = mapper.Map<LojaViewModel>(lojaResponse.Data);
                var grupolojaResponse = await mediator.Send(new GetGrupolojaByIdQuery() { Id = lojaViewModel.GrupolojaId });
                var grupolojaViewModel = mapper.Map<GrupolojaViewModel>(grupolojaResponse.Data);
                return grupolojaViewModel.Nome;
            }
            return string.Empty;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<int> GetGrupolojaMaxDiasDecisãoAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                var lojaViewModel = mapper.Map<LojaViewModel>(lojaResponse.Data);
                var grupolojaResponse = await mediator.Send(new GetGrupolojaByIdQuery() { Id = lojaViewModel.GrupolojaId });
                var grupolojaViewModel = mapper.Map<GrupolojaViewModel>(grupolojaResponse.Data);
                return grupolojaViewModel.MaxDiasDecisão;
            }
            return 0;
        }


        //---------------------------------------------------------------------------------------------------

    }
}