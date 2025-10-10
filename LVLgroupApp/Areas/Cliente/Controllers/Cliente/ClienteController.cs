using AspNetCoreHero.Results;
using AutoMapper;
using Core.Constants;
using Core.Entities.Clientes;
using Core.Entities.Select2;
using Core.Features.Clientes.Commands.Create;
using Core.Features.Clientes.Commands.Delete;
using Core.Features.Clientes.Commands.Update;
using Core.Features.Clientes.Queries.GetAllCached;
using Core.Features.Clientes.Queries.GetById;
using Core.Features.Clientes.Queries.GetByTelefone;
using Infrastructure.Extensions;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Artigo.Models.Artigo;
using LVLgroupApp.Areas.Cliente.Models.Cliente;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Cliente.Controllers.Cliente
{
    [Area("Cliente")]
    [Authorize]
    public class ClienteController : BaseController<ClienteController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<ClienteController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ClienteController(IStringLocalizer<ClienteController> localizer)
        {
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Clientes.View)]
        public IActionResult Index()
        {
            var model = new ClienteViewModel();
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Cliente Contoller - Index - return viewModel");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Clientes.View)]
        public IActionResult LoadAll()
        {
            //var response = await _mediator.Send(new GetAllClientesCachedQuery());
            //if (response.Succeeded)
            //{
            //    var viewModel = _mapper.Map<List<ClienteViewModel>>(response.Data);
            //    return PartialView("_ViewAll", viewModel);
            //}
            //return null;

            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Cliente Contoller - LoadAll - return lista vazia de clienteViewModel");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de clientes para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Clientes.View)]
        public async Task<IActionResult> GetClientes()
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

                // lista de clientes
                var responseAllClientes = await _mediator.Send(new GetAllClientesCachedQuery());
                if (!responseAllClientes.Succeeded) return null;
                var allClientesData = _mapper.Map<List<ClienteViewModel>>(responseAllClientes.Data).AsQueryable();


                // filtrar searchValue
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allClientesData = allClientesData.Where(x => x.Telefone.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                        x.Nome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                        (!x.Email.IsNullOrEmpty() && x.Email.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                                                        (!x.Morada.IsNullOrEmpty() && x.Morada.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                                                        (!x.NIF.IsNullOrEmpty() && x.NIF.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                                                        (!x.IBAN.IsNullOrEmpty() && x.IBAN.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                                                     );
                    }



                // ordenar lista
                // var sortedClientesData = allClientesData.AsQueryable();
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    allClientesData = allClientesData.OrderBy(sortColumn + " " + sortColumnDirection);
                }

                // retornar lista para a datatable
                recordsTotal = allClientesData.Count();
                var data = allClientesData.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - GetClaims - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Clientes.Create)]
        public async Task<JsonResult> OnGetCreateOrEdit(int id = 0)
        {
            if (id == 0)
            {
                // create new cliente
                var clienteViewModel = new ClienteViewModel();
                clienteViewModel.TipoContactoId = 1;
                clienteViewModel.TipoContacto = TiposContactoDeCliente.GetTipoContacto(clienteViewModel.TipoContactoId);
                clienteViewModel.TipoContactoList = GetSelectListTiposContactos(clienteViewModel.TipoContacto);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", clienteViewModel) });
            }
            else
            {
                // editar cliente com Id = id
                var response = await _mediator.Send(new GetClienteByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var clienteViewModel = _mapper.Map<ClienteViewModel>(response.Data);
                    clienteViewModel.Telefone = clienteViewModel.Telefone.FormatTelefone();
                    clienteViewModel.TipoContactoId = Int32.Parse(response.Data.TipoContacto);
                    clienteViewModel.TipoContacto = TiposContactoDeCliente.GetTipoContacto(clienteViewModel.TipoContactoId);
                    clienteViewModel.TipoContactoList = GetSelectListTiposContactos(clienteViewModel.TipoContacto);
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", clienteViewModel) });
                }
                return new JsonResult(new { isValid = false, html = string.Empty});
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Clientes.Create)]
        public async Task<JsonResult> OnPostCreateOrEdit(int id, ClienteViewModel cliente)
        {
            if (!ModelState.IsValid)
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", cliente);
                return new JsonResult(new { isValid = false, html = html });
            }
            else
            {
                // verificar se telefone já existe
                var responseTelefone = await _mediator.Send(new GetClienteByTelefoneQuery() { Telefone = cliente.Telefone.Trim() });
                var telefoneExiste = (responseTelefone.Succeeded && responseTelefone.Data != null);

                if (id == 0)
                {
                    // criar novo cliente 
                    if (telefoneExiste)
                    {
                        // atualizar cliente já existente
                        var updateClienteCommand = _mapper.Map<UpdateClienteCommand>(cliente);
                        updateClienteCommand.Id = responseTelefone.Data.Id;
                        updateClienteCommand.TipoContacto = TiposContactoDeCliente.GetTipoContacto(cliente.TipoContactoId);
                        updateClienteCommand.Telefone = updateClienteCommand.Telefone.CleanTelefone();
                        var resultUpdate = await _mediator.Send(updateClienteCommand);
                        if (resultUpdate.Succeeded)
                        {
                            _notify.Information($"{_localizer["Cliente com ID"]} {resultUpdate.Data} {_localizer[" atualizado."]}");
                        }
                        else
                        {
                            _notify.Error(resultUpdate.Message);
                            return new JsonResult(new { isValid = false, html = string.Empty });
                        }
                    }
                    else
                    {
                        // criar novo cliente
                        var createClienteCommand = _mapper.Map<CreateClienteCommand>(cliente);
                        createClienteCommand.TipoContacto = TiposContactoDeCliente.GetTipoContacto(cliente.TipoContactoId);
                        createClienteCommand.Telefone = createClienteCommand.Telefone.CleanTelefone();
                        var resultCreate = await _mediator.Send(createClienteCommand);
                        if (resultCreate.Succeeded)
                        {
                            _notify.Success($"{_localizer["Cliente com ID"]} {resultCreate.Data} {_localizer[" criado."]}");
                        }
                        else
                        {
                            _notify.Error(resultCreate.Message);
                            return new JsonResult(new { isValid = false, html = string.Empty });
                        }
                    }
                }
                else
                {
                    // editar cliente com Id = id
                    if (telefoneExiste && responseTelefone.Data.Id != id)
                    {
                        // erro inesperado: telefone igual ao de outro cliente já existente
                        // não atualizar cliente
                        _notify.Error($"{_localizer["Este telefone já existe e pertence a outro cliente com o Id"]} {responseTelefone.Data.Id}.");
                        var html = await _viewRenderer.RenderViewToStringAsync("_CreateOrEdit", cliente);
                        return new JsonResult(new { isValid = false, html = html });
                    }

                    // atualizar cliente
                    var updateClienteCommand = _mapper.Map<UpdateClienteCommand>(cliente);
                    updateClienteCommand.Id = id;
                    updateClienteCommand.TipoContacto = cliente.TipoContactoId.ToString();
                    updateClienteCommand.Telefone = updateClienteCommand.Telefone.CleanTelefone();
                    var resultUpdate = await _mediator.Send(updateClienteCommand);
                    if (resultUpdate.Succeeded)
                    {
                        _notify.Information($"{_localizer["Cliente com ID"]} {resultUpdate.Data} {_localizer[" atualizado."]}");
                    }
                    else
                    {
                        _notify.Error(resultUpdate.Message);
                        return new JsonResult(new { isValid = false, html = string.Empty });
                    }
                }

                // Terminar e retornar _ViewAll
                var response = await _mediator.Send(new GetAllClientesCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<ClienteViewModel>>(response.Data);
                    var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                    return new JsonResult(new { isValid = true, html = html });
                }
                else
                {
                    _notify.Error(response.Message);
                    return new JsonResult(new { isValid = false, html = string.Empty });
                }
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Clientes.Delete)]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            var deleteCommand = await _mediator.Send(new DeleteClienteCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["Cliente com ID"]} {id} {_localizer[" removido."]}");
                var response = await _mediator.Send(new GetAllClientesCachedQuery());
                if (response.Succeeded)
                {
                    var viewModel = _mapper.Map<List<ClienteViewModel>>(response.Data);
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


        [Authorize(Policy = Permissions.Clientes.Create)]
        public async Task<IActionResult> OnGetViewTabCliente(int id = 0)
        {
            // Devolve a View (_ViewTabCliente) da tab "Cliente" no _CreateOrEdit da Claim
            // É chamada por ajax call &().load
            // Id tem o ClienteId da ClaimViewModel

            var clienteViewModel = new TabClienteViewModel();

            if (id == 0)
            {
                clienteViewModel.TipoContactoList = GetSelectListTiposContactos(TiposContactoDeCliente.GetTipoContacto(1));
                return PartialView("_ViewTabCliente", clienteViewModel);
            }
            else 
            {
                var response = await _mediator.Send(new GetClienteByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    clienteViewModel = _mapper.Map<TabClienteViewModel>(response.Data);
                    clienteViewModel.TipoContactoList = GetSelectListTiposContactos(clienteViewModel.TipoContacto);
                    return PartialView("_ViewTabCliente", clienteViewModel);
                }
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Clientes.View)]
        public async Task<JsonResult> GetClienteAsync(int clienteId)
        {
            var response = await _mediator.Send(new GetClienteByIdQuery() { Id = clienteId });
            if (response.Succeeded)
            {
                var clienteViewModel = new TabClienteViewModel();
                clienteViewModel = _mapper.Map<TabClienteViewModel>(response.Data);
                clienteViewModel.Id = response.Data.Id;
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(new { cliente = clienteViewModel });
                return Json(jsonString);
            }
            return Json(new { status = "error" });
        }


        //---------------------------------------------------------------------------------------------------

        [Authorize(Policy = Permissions.Clientes.Create)]
        public async Task<IActionResult> OnGetCreateClienteAsync()
        {
            var clienteViewModel = new ClienteViewModel();
            clienteViewModel.TipoContactoList = GetSelectListTiposContactos(TiposContactoDeCliente.GetTipoContacto(1));
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_ViewCreateCliente", clienteViewModel) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Clientes.Create)]
        public async Task<JsonResult> OnPostCreateClienteAsync(ClienteViewModel cliente)
        {
            if (!ModelState.IsValid)
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewCreateCliente", cliente);
                return new JsonResult(new { isValid = false, html = html });
            }
            else
            {
                // verificar se telefone já existe
                var responseTelefone = await _mediator.Send(new GetClienteByTelefoneQuery() { Telefone = cliente.Telefone.Trim() });
                var telefoneExiste = (responseTelefone.Succeeded && responseTelefone.Data != null);

                // criar novo cliente 
                if (telefoneExiste)
                {
                    // atualizar cliente já existente
                    var updateClienteCommand = _mapper.Map<UpdateClienteCommand>(cliente);
                    updateClienteCommand.Id = responseTelefone.Data.Id;
                    var resultUpdate = await _mediator.Send(updateClienteCommand);
                    if (resultUpdate.Succeeded)
                    {
                        _notify.Information($"{_localizer["Cliente com ID"]} {resultUpdate.Data} {_localizer[" atualizado."]}");
                    }
                    else
                    {
                        _notify.Error(resultUpdate.Message);
                        return new JsonResult(new { isValid = false, html = string.Empty });
                    }
                }
                else
                {
                    // criar novo cliente
                    var createClienteCommand = _mapper.Map<CreateClienteCommand>(cliente);
                    var resultCreate = await _mediator.Send(createClienteCommand);
                    if (resultCreate.Succeeded)
                    {
                        _notify.Success($"{_localizer["Cliente com ID"]} {resultCreate.Data} {_localizer[" criado."]}");
                    }
                    else
                    {
                        _notify.Error(resultCreate.Message);
                        return new JsonResult(new { isValid = false, html = string.Empty });
                    }
                }

                // Terminar e retornar 
                return new JsonResult(new { isValid = true, html = string.Empty });

            }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetListAllClientesTelefones(string term, int page)
        {
            int resultCount = 25;
            int offset = (page - 1) * resultCount;
            string searchStr = string.IsNullOrEmpty(term) ? "" : term;

            var clientesResponse = await _mediator.Send(new GetAllClientesCachedQuery());
            var clientesViewModel = _mapper.Map<List<ClienteViewModel>>(clientesResponse.Data);
            var searchData = clientesViewModel.Where(a => a.Nome.Contains(searchStr, StringComparison.OrdinalIgnoreCase)
                                                       || a.Telefone.Contains(term, StringComparison.OrdinalIgnoreCase));
            int count = searchData.Count();

            searchData = searchData.Skip(offset).Take(resultCount);
            var clientesData = searchData.Select(a => new Select2Item { id = a.Id, text = a.Telefone }).ToList();

            int endCount = offset + resultCount;
            bool morePages = count > endCount;

            var data = new
            {
                results = clientesData,
                pagination = new
                {
                    more = morePages
                }
            };

            return Ok(data);
        }


        //---------------------------------------------------------------------------------------------------

        [Authorize(Policy = Permissions.Clientes.View)]
        public static async Task<ClienteViewModel> GetClienteViewModelAsync(int clienteId, IMapper mapper, IMediator mediator)
        {
            var response = await mediator.Send(new GetClienteByIdQuery() { Id = clienteId });
            if (response.Succeeded)
            {
                var clienteViewModel = mapper.Map<ClienteViewModel>(response.Data);
                clienteViewModel.TipoContactoList = GetSelectListTiposContactos(clienteViewModel.TipoContacto);
                return clienteViewModel;
            }
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllClientesAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var clientesResponse = await mediator.Send(new GetAllClientesCachedQuery());
            var clientesViewModel = mapper.Map<List<ClienteViewModel>>(clientesResponse.Data);
            return new SelectList(clientesViewModel, nameof(ClienteViewModel.Id), nameof(ClienteViewModel.Telefone), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<SelectList> GetSelectListAllClientTelefonesAsync(int selectedId, IMapper mapper, IMediator mediator)
        {
            var clientesResponse = await mediator.Send(new GetAllClientesCachedQuery());
            var clientesViewModel = mapper.Map<List<ClienteViewModel>>(clientesResponse.Data);
            return new SelectList(clientesViewModel, nameof(ClienteViewModel.Id), nameof(ClienteViewModel.Telefone), selectedId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static SelectList GetSelectListTiposContactos(string selectedTipo)
        {
            var tcList = TiposContactoDeCliente.TipoContactoList;
            var TipoId = TiposContactoDeCliente.GetTipoContactoId(selectedTipo);
            return new SelectList(tcList, nameof(TipoContacto.Id), nameof(TipoContacto.Tipo), TipoId, null);
        }


        //---------------------------------------------------------------------------------------------------


        public static async Task<string> GetClienteNomeAsync(int clienteId, IMapper mapper, IMediator mediator)
        {
            if (clienteId > 0)
            {
                var clienteResponse = await mediator.Send(new GetClienteByIdQuery() { Id = clienteId });
                return clienteResponse.Data.Nome;
            }
            return string.Empty;
        }


        //---------------------------------------------------------------------------------------------------

    }
}