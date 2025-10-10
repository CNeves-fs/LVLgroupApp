using AutoMapper;
using Core.Constants;
using Core.Entities.Claims;
using Core.Entities.Select2;
using Core.Enums;
using Core.Features.Statuss.Commands.Create;
using Core.Features.Statuss.Commands.Delete;
using Core.Features.Statuss.Commands.Update;
using Core.Features.Statuss.Queries.GetAllCached;
using Core.Features.Statuss.Queries.GetById;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Claim.Models.Status;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Claim.Controllers.Status
{
    [Area("Claim")]
    [Authorize]
    public class StatusController : BaseController<StatusController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<StatusController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public StatusController(IStringLocalizer<StatusController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Statuss.View)]
        public IActionResult Index()
        {
            var model = new StatusViewModel();
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Statuss.View)]
        public async Task<IActionResult> LoadAll()
        {
            var response = await _mediator.Send(new GetAllStatusCachedQuery());
            if (response.Succeeded)
            {
                var viewModel = _mapper.Map<List<StatusViewModel>>(response.Data);
                return PartialView("_ViewAll", viewModel);
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Statuss.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0) // Criar novo Status
            {
                var statusViewModel = new StatusViewModel();
                statusViewModel.Tipos = new SelectList(TiposStatus.TipoStatusList, nameof(TipoStatus.Id), nameof(TipoStatus.Tipo), 0);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", statusViewModel) });
            }
            else // Editar Status
            {
                var response = await _mediator.Send(new GetStatusByIdQuery() { Id = id });
                var statusViewModel = _mapper.Map<StatusViewModel>(response.Data);
                statusViewModel.Tipos = new SelectList(TiposStatus.TipoStatusList, nameof(TipoStatus.Id), nameof(TipoStatus.Tipo), statusViewModel.Tipo);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", statusViewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Statuss.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, StatusViewModel status)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //create new Status
                    var createStatusCommand = _mapper.Map<CreateStatusCommand>(status);
                    var result = await _mediator.Send(createStatusCommand);
                    if (result.Succeeded)
                    {
                        id = result.Data;
                        _notify.Success($"{_localizer["Status com ID"]} {result.Data} {_localizer[" criado."]}");
                    }
                    else _notify.Error(result.Message);
                }
                else
                {
                    //update Status
                    var updateStatusCommand = _mapper.Map<UpdateStatusCommand>(status);
                    var result = await _mediator.Send(updateStatusCommand);
                    if (result.Succeeded) _notify.Information($"{_localizer["Status com ID"]} {result.Data} {_localizer[" atualizado."]}");
                }

                // return _ViewAll

                var response = await _mediator.Send(new GetAllStatusCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<StatusViewModel>>(response.Data);
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
        [Authorize(Policy = Permissions.Statuss.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteStatusCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Status com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllStatusCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<StatusViewModel>>(response.Data);
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


        public async Task<JsonResult> LoadStatus()
        {

            var statusResponse = await _mediator.Send(new GetAllStatusCachedQuery());

            if (statusResponse.Succeeded)
            {
                var statusViewModel = _mapper.Map<List<StatusViewModel>>(statusResponse.Data);
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(statusViewModel);
                return Json(new { status = "success", statuslist = jsonString });
            }
            return Json(new { status = "error" });
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetListAllStatus(string term)
        {
            var statusResponse = await _mediator.Send(new GetAllStatusCachedQuery());
            var statusViewModel = _mapper.Map<List<StatusViewModel>>(statusResponse.Data);
            List<Select2Item> data = new List<Select2Item>();

            data.Add(new Select2Item
            {
                id = 0,
                text = _localizer["Todos os status"],
                textColor = "var(--bs-body-color)",
                backgroundColor = "var(--bs-body-bg)"
                //selected = (item.Id == selectedId)
            });

            if (!string.IsNullOrEmpty(term))
            {
                var search = statusViewModel.Where(s => s.Texto.Contains(term, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
                foreach (var item in search)
                {
                    data.Add(new Select2Item
                    {
                        id = item.Id,
                        text = item.Texto == null ? "" : item.Texto,
                        textColor = item.Cortexto,
                        backgroundColor = item.Corfundo
                        //selected = (item.Id == selectedId)
                    });
                }
            }
            else
            {
                foreach (var item in statusViewModel)
                {
                    data.Add(new Select2Item
                    {
                        id = item.Id,
                        text = item.Texto == null ? "" : item.Texto,
                        textColor = item.Cortexto,
                        backgroundColor = item.Corfundo
                        //selected = (item.Id == selectedId)
                    });
                }
            };

            return Ok(data);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListStatusTypeAsync(int[] tipos, IMapper mapper, IMediator mediator)
        {
            if (tipos == null || tipos.Length == 0) return null;
            var statusList = new List<StatusViewModel>();
            foreach (var tipo in tipos)
            {
                if (tipo == 0)
                {
                    var s0 = new StatusViewModel();
                    s0.Id = 0;
                    s0.Tipo = tipo;
                    s0.Texto = "MANTER STATUS DA RECLAMAÇÃO";
                    statusList.Add(s0);
                }
                else
                {
                    // tipo > 0
                    var statusResponse = await mediator.Send(new GetAllStatusByTypeCachedQuery() { statustype = tipo });
                    var statusViewModel = mapper.Map<List<StatusViewModel>>(statusResponse.Data);
                    foreach (var status in statusViewModel)
                    {
                        statusList.Add(status);
                    };
                }
            };
            return new SelectList(statusList, nameof(StatusViewModel.Id), nameof(StatusViewModel.Texto), null, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllStatusAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var statusResponse = await mediator.Send(new GetAllStatusCachedQuery());
            var statusViewModel = mapper.Map<List<StatusViewModel>>(statusResponse.Data);
            return new SelectList(statusViewModel, nameof(StatusViewModel.Id), nameof(StatusViewModel.Texto), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<int> GetStatusTipoAsync(int statusId, IMapper mapper, IMediator mediator)
        {
            if (statusId == 0) return 0;
            var statusResponse = await mediator.Send(new GetStatusByIdQuery() { Id = statusId});
            var statusViewModel = mapper.Map<StatusViewModel>(statusResponse.Data);
            return statusViewModel.Tipo;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<StatusViewModel> GetStatusAsync(int statusId, IMapper mapper, IMediator mediator)
        {
            var statusResponse = await mediator.Send(new GetStatusByIdQuery() { Id = statusId });
            var statusViewModel = mapper.Map<StatusViewModel>(statusResponse.Data);
            return statusViewModel;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<List<StatusViewModel>> GetListAllStatusAsync(IMapper mapper, IMediator mediator)
        {
            var statusResponse = await mediator.Send(new GetAllStatusCachedQuery());
            var listStatusViewModel = mapper.Map<List<StatusViewModel>>(statusResponse.Data);
            return listStatusViewModel;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListGoBackStatusAsync(IMapper mapper, IMediator mediator)
        {
            var statusResponse = await mediator.Send(new GetAllStatusCachedQuery());
            var listStatusViewModel = mapper.Map<List<StatusViewModel>>(statusResponse.Data).AsQueryable();
            var gobackList = new List<StatusViewModel>();
            foreach (var item in listStatusViewModel)
            {
                if(item.Tipo == (int)StatusType.AguardaValidação || item.Tipo == (int)StatusType.AguardaDecisão)
                {
                    gobackList.Add(item);
                }
            }
            return new SelectList(gobackList, nameof(StatusViewModel.Id), nameof(StatusViewModel.Texto), 0, null);
        }


        //---------------------------------------------------------------------------------------------------

    }
}