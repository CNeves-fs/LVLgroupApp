using AutoMapper;
using Core.Constants;
using Core.Entities.Select2;
using Core.Features.Empresas.Commands.Create;
using Core.Features.Empresas.Commands.Delete;
using Core.Features.Empresas.Commands.Update;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Empresas.Queries.GetById;
using Core.Features.Gruposlojas.Queries.GetById;
using Core.Features.Lojas.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Models.Empresa;
using LVLgroupApp.Areas.Business.Models.Grupoloja;
using LVLgroupApp.Areas.Business.Models.Loja;
using LVLgroupApp.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Business.Controllers.Empresa
{
    [Area("Business")]
    [Authorize]
    public class EmpresaController : BaseController<EmpresaController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<EmpresaController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public EmpresaController(IStringLocalizer<EmpresaController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Empresas.View)]
        public IActionResult Index()
        {
            var model = new EmpresaViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Empresas.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllEmpresasCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<EmpresaViewModel>>(response.Data);
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Empresas.Edit)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            var empresasResponse = await _mediator.Send(new GetAllEmpresasCachedQuery());

            if (id == 0)
            {
                var empresaViewModel = new EmpresaViewModel();
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", empresaViewModel) });
            }
            else
            {
                var response = await _mediator.Send(new GetEmpresaByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var empresaViewModel = _mapper.Map<EmpresaViewModel>(response.Data);
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", empresaViewModel) });
                }
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Empresas.Edit)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, EmpresaViewModel empresa)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    empresa.LogoPicture = file.OptimizeImageSize(720, 720);
                }

                if (id == 0)
                {
                    
                    var createEmpresaCommand = _mapper.Map<CreateEmpresaCommand>(empresa);
                    var result = await _mediator.Send(createEmpresaCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Empresa com ID"]} {result.Data} {_localizer[" criada."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    var updateEmpresaCommand = _mapper.Map<UpdateEmpresaCommand>(empresa);
                    var result = await _mediator.Send(updateEmpresaCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Empresa com ID"]} {result.Data} {_localizer[" atualizada."]}");
                }
                // return _ViewAll
                var response = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<EmpresaViewModel>>(response.Data);
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
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", empresa);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Empresas.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteEmpresaCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Empresa com ID"]} {id} {_localizer[" removida."]}");
                var response = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<EmpresaViewModel>>(response.Data);
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


        public static async Task<SelectList> GetSelectListAllEmpresasAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var empresasResponse = await mediator.Send(new GetAllEmpresasCachedQuery());
            var empresasViewModel = mapper.Map<List<EmpresaViewModel>>(empresasResponse.Data);
            return new SelectList(empresasViewModel, nameof(EmpresaViewModel.Id), nameof(EmpresaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListOneEmpresaAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var empresasResponse = await mediator.Send(new GetAllEmpresasCachedQuery());
            var empresasViewModel = mapper.Map<List<EmpresaViewModel>>(empresasResponse.Data);
            empresasViewModel.RemoveAll(e => e.Id != selectedId);
            return new SelectList(empresasViewModel, nameof(EmpresaViewModel.Id), nameof(EmpresaViewModel.Nome), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<int> GetEmpresaIdAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                var lojaViewModel = mapper.Map<LojaViewModel>(lojaResponse.Data);
                var grupolojaResponse = await mediator.Send(new GetGrupolojaByIdQuery() { Id = lojaViewModel.GrupolojaId }); 
                return grupolojaResponse.Data.EmpresaId;
            }
            return 0;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<string> GetEmpresaNomeAsync(int lojaId, IMapper mapper, IMediator mediator)
        {
            if (lojaId > 0)
            {
                var lojaResponse = await mediator.Send(new GetLojaByIdQuery() { Id = lojaId });
                var lojaViewModel = mapper.Map<LojaViewModel>(lojaResponse.Data);
                var grupolojaResponse = await mediator.Send(new GetGrupolojaByIdQuery() { Id = lojaViewModel.GrupolojaId });
                var grupolojaViewModel = mapper.Map<GrupolojaViewModel>(grupolojaResponse.Data);
                var empresaResponse = await mediator.Send(new GetEmpresaByIdQuery() { Id = grupolojaViewModel.EmpresaId });
                var empresaViewModel = mapper.Map<EmpresaViewModel>(empresaResponse.Data);
                return empresaResponse.Data.Nome;
            }
            return string.Empty;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<EmpresaViewModel> GetEmpresaAsync(int empresaId, IMapper mapper, IMediator mediator)
        {
            if (empresaId > 0)
            {
                var empresaResponse = await mediator.Send(new GetEmpresaByIdQuery() { Id = empresaId });
                return mapper.Map<EmpresaViewModel>(empresaResponse.Data);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<byte[]> GetEmpresaLogoAsync(int empresaId, IMapper mapper, IMediator mediator)
        {
            if (empresaId > 0)
            {
                var empresaResponse = await mediator.Send(new GetEmpresaByIdQuery() { Id = empresaId });
                var empresaViewModel = mapper.Map<EmpresaViewModel>(empresaResponse.Data);
                return empresaResponse.Data.LogoPicture;
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetListAllEmpresas(string term)
        {
            var empresasResponse = await _mediator.Send(new GetAllEmpresasCachedQuery());
            var empresasViewModel = _mapper.Map<List<EmpresaViewModel>>(empresasResponse.Data);
            List<Select2Item> data = new List<Select2Item>();

            data.Add(new Select2Item
            {
                id = 0,
                text = _localizer["Todas as empresas"],
                textColor = "",
                backgroundColor = ""
                //selected = (item.Id == selectedId)
            });

            if (!string.IsNullOrEmpty(term))
            {
                var search = empresasViewModel.Where(g => g.Nome.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
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
                foreach (var item in empresasViewModel)
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


        public async Task<JsonResult> LoadAllEmpresas()
        {

            var empresasResponse = await _mediator.Send(new GetAllEmpresasCachedQuery());

            if (empresasResponse.Succeeded)
            {
                var empresasViewModel = _mapper.Map<List<EmpresaViewModel>>(empresasResponse.Data);
                var empresas = new SelectList(empresasViewModel, nameof(EmpresaViewModel.Id), nameof(EmpresaViewModel.Nome), null, null);

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { empresasList = empresas });
                return Json(jsonString);
            }

            return null;
        }


        //---------------------------------------------------------------------------------------------------

    }
}