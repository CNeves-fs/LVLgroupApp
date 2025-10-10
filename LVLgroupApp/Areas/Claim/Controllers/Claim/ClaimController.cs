using Core.Constants;
using Core.Entities.Identity;
using Core.Enums;
using Core.Features.Artigos.Queries.GetAllCached;
using Core.Features.Claims.Commands.Create;
using Core.Features.Claims.Commands.Delete;
using Core.Features.Claims.Commands.Update;
using Core.Features.Claims.Queries.GetAllCached;
using Core.Features.Claims.Queries.GetById;
using Core.Features.Claims.Response;
using Core.Features.Clientes.Commands.Update;
using Core.Features.Clientes.Queries.GetAllCached;
using Core.Features.Clientes.Queries.GetById;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Empresas.Queries.GetById;
using Core.Features.Fotos.Commands.Delete;
using Core.Features.Fotos.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetById;
using Core.Features.Pareceres.Commands.Create;
using Core.Features.Pareceres.Commands.Delete;
using Core.Features.Pareceres.Commands.Update;
using Core.Features.Pareceres.Queries.GetById;
using Core.Features.Statuss.Queries.GetAllCached;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Artigo.Controllers.Artigo;
using LVLgroupApp.Areas.Artigo.Controllers.Gender;
using LVLgroupApp.Areas.Artigo.Models.Artigo;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Claim.Controllers.Foto;
using LVLgroupApp.Areas.Claim.Controllers.Prazolimite;
using LVLgroupApp.Areas.Claim.Controllers.Status;
using LVLgroupApp.Areas.Claim.Models.Aprovação;
using LVLgroupApp.Areas.Claim.Models.Claim;
using LVLgroupApp.Areas.Claim.Models.Foto;
using LVLgroupApp.Areas.Claim.Models.ParecerTécnico;
using LVLgroupApp.Areas.Claim.Models.Status;
using LVLgroupApp.Areas.Cliente.Controllers.Cliente;
using LVLgroupApp.Areas.Cliente.Models.Cliente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Claim.Controllers.Claim
{
    [Area("Claim")]
    [Authorize]
    public class ClaimController : BaseController<ClaimController>
    {

        //---------------------------------------------------------------------------------------------------


        private IWebHostEnvironment _environment;

        private readonly IStringLocalizer<ClaimController> _localizer;

        private readonly SignInManager<ApplicationUser> _signInManager;


        //---------------------------------------------------------------------------------------------------


        public ClaimController(IWebHostEnvironment Environment, IStringLocalizer<ClaimController> localizer, SignInManager<ApplicationUser> signInManager)
        {
            _localizer = localizer;
            _environment = Environment;
            _signInManager = signInManager;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Claims.View)]
        public IActionResult Index()
        {
            var model = new ClaimViewModel();
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - Index - return viewModel");
            return View("Index", model);
        }
        

        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Claims.View)]
        public IActionResult LoadAll()
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - LoadAll - return lista vazia de claimviewModel");
            return PartialView("_ViewAll");
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// implementa o sever side para a datatables.
        /// devolve a lista de claims para a tabela.
        /// </summary>
        /// <returns>jsonData</returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Claims.View)]
        public async Task<IActionResult> GetClaims()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                var statusfilter = Request.Form["statusfilter"].FirstOrDefault();
                var minDias = Request.Form["minDias"].FirstOrDefault();
                var maxDias = Request.Form["maxDias"].FirstOrDefault();
                var limite = Request.Form["limite"].FirstOrDefault();
                var desdedateClaim = Request.Form["desdedateClaim"].FirstOrDefault();
                var atedateClaim = Request.Form["atedateClaim"].FirstOrDefault();

                int intFilterStatus = statusfilter != null ? Convert.ToInt32(statusfilter) : 0;
                int intFilterMinDias = minDias != null ? Convert.ToInt32(minDias) : Int32.MinValue;
                int intFilterMaxDias = maxDias != null ? Convert.ToInt32(maxDias) : Int32.MaxValue;
                if (intFilterMinDias <= -30) intFilterMinDias = Int32.MinValue;
                if (intFilterMaxDias >= 30) intFilterMaxDias = Int32.MaxValue;
                int intLimite = limite != null ? Convert.ToInt32(limite) : 0;
                DateTime dateDesde = String.IsNullOrEmpty(desdedateClaim) ? DateTime.MinValue : DateTime.Parse(desdedateClaim);
                DateTime dateAte = String.IsNullOrEmpty(atedateClaim) ? DateTime.MaxValue : DateTime.Parse(atedateClaim);

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                if (pageSize < 0) pageSize = Int32.MaxValue;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // lista de claims permitidas ao current user
                var allClaims = await GetClaimsListAsync();

                // filtrar por status se necessário
                if (intFilterStatus > 0)
                {
                    allClaims = allClaims.Where(c => c.StatusId == intFilterStatus);
                };
                allClaims = allClaims.Where(c => c.DataClaim >= dateDesde);
                allClaims = allClaims.Where(c => c.DataClaim <= dateAte);


                // construir lista para View Model

                var responseAllEmpresas = await _mediator.Send(new GetAllEmpresasCachedQuery());
                if (!responseAllEmpresas.Succeeded) return new ObjectResult(new { status = "error" });
                var allEmpresas = _mapper.Map<List<Core.Entities.Business.Empresa>>(responseAllEmpresas.Data).AsQueryable();

                var responseAllLojas = await _mediator.Send(new GetAllLojasCachedQuery());
                if (!responseAllLojas.Succeeded) return new ObjectResult(new { status = "error" });
                var allLojas = _mapper.Map<List<Core.Entities.Business.Loja>>(responseAllLojas.Data).AsQueryable();

                var responseAllStatus = await _mediator.Send(new GetAllStatusCachedQuery());
                if (!responseAllStatus.Succeeded) return new ObjectResult(new { status = "error" });
                var allStatus = _mapper.Map<List<Core.Entities.Claims.Status>>(responseAllStatus.Data).AsQueryable();

                var responseAllClientes = await _mediator.Send(new GetAllClientesCachedQuery());
                if (!responseAllClientes.Succeeded) return new ObjectResult(new { status = "error" });
                var allClientes = _mapper.Map<List<Core.Entities.Clientes.Cliente>>(responseAllClientes.Data).AsQueryable();

                var responseAllArtigos = await _mediator.Send(new GetAllArtigosCachedQuery());
                if (!responseAllArtigos.Succeeded) return new ObjectResult(new { status = "error" });
                var allArtigos = _mapper.Map<List<Core.Entities.Artigos.Artigo>>(responseAllArtigos.Data).AsQueryable();

                var claimsData = from claim in allClaims
                                 join e in allEmpresas on claim.EmpresaId equals e.Id into elist
                                 from emp in elist.DefaultIfEmpty()
                                 join l in allLojas on claim.LojaId equals l.Id into llist
                                 from loj in llist.DefaultIfEmpty()
                                 join s in allStatus on claim.StatusId equals s.Id into slist
                                 from stat in slist.DefaultIfEmpty()
                                 join c in allClientes on claim.ClienteId equals c.Id into clist
                                 from cli in clist.DefaultIfEmpty()
                                 join a in allArtigos on claim.ArtigoId equals a.Id into alist
                                 from art in alist.DefaultIfEmpty()
                                 select new ClaimListViewModel()
                                 {
                                     Id = claim.Id,
                                     DataClaim = claim.DataClaim,
                                     EmpresaLogo = Convert.ToBase64String(emp.LogoPicture),
                                     CodeId = claim.CodeId,
                                     DataLimite = claim.DataLimite,
                                     NumeroDiasParaFecho = (int)(claim.DataLimite - DateTime.Now).TotalDays,
                                     Status = _mapper.Map<StatusViewModel>(stat),
                                     Prazolimite = null,
                                     EmpresaNome = emp.Nome,
                                     LojaNome = loj.Nome,
                                     ClienteId = cli.Id,
                                     NomeCliente = cli.Nome,
                                     TelefoneCliente = cli.Telefone,
                                     DataUltimoContacto = cli.DataUltimoContacto,
                                     ArtigoId = art.Id,
                                     RefArtigo = art.Referencia,
                                     DefeitoDoArtigo = claim.DefeitoDoArtigo
                                 };

                // filtrar searchValue
                if (!string.IsNullOrEmpty(searchValue))
                {
                    claimsData = claimsData.Where(x => x.CodeId.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                       x.Status.Texto.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                       x.EmpresaNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                       x.LojaNome.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                       x.RefArtigo.Contains(searchValue, StringComparison.OrdinalIgnoreCase) ||
                                                       (!x.NomeCliente.IsNullOrEmpty() && x.NomeCliente.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                                                       (!x.TelefoneCliente.IsNullOrEmpty() && x.TelefoneCliente.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                                                   );
                }

                // filtrar por NumeroDiasParaFecho se necessário
                if (intFilterMaxDias < 30 || intFilterMinDias > -30)
                {
                    claimsData = claimsData.Where(c => c.NumeroDiasParaFecho >= intFilterMinDias && c.NumeroDiasParaFecho <= intFilterMaxDias);
                }

                // adicionar prazolimite aos elementos da lista
                var claimsList = claimsData.ToList();
                foreach (ClaimListViewModel item in claimsList)
                {
                    if (item.Status.Tipo < TiposStatus.FirstClosedTipoStatus || item.Status.Tipo > TiposStatus.LastClosedTipoStatus)
                    {
                        item.Prazolimite = await PrazolimiteController.GetPrazolimiteAsync(item.DataLimite, _mapper, _mediator);
                    }
                };

                // filtrar Prazolimite se necessário
                if (intLimite == 1000)
                {
                    // reclamações fechadas
                    claimsList = claimsList.Where(c => c.Prazolimite == null).ToList();
                }
                if (intLimite == 2000)
                {
                    // reclamações  não fechadas
                    claimsList = claimsList.Where(c => c.Prazolimite != null).ToList();
                }
                if (intLimite > 0 && intLimite < 1000)
                {
                    // reclamações  com prazos defenidos
                    claimsList = claimsList.Where(c => c.Prazolimite != null)
                        .Where(c => c.Prazolimite.Id == intLimite).ToList();
                }

                // ordenar lista
                var sortedClaimsData = claimsList.AsQueryable();
                if ( !string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection) )
                {
                    sortedClaimsData = sortedClaimsData.OrderBy(sortColumn + " " + sortColumnDirection);
                }

                // retornar lista para a datatable
                recordsTotal = sortedClaimsData.Count();
                var data = sortedClaimsData.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName +  " | Claim Contoller - GetClaims - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o pedido do client para criar claim
        /// é preparado um claimViewModel com params por Tab e
        /// flags de identificação do user
        /// </summary>
        /// <returns></returns>

        [Authorize(Policy = Permissions.Claims.Create)]
        public async Task<IActionResult> OnGetCreateAsync()
        {
            // Criar folder temporário
            ////var claimFolder = Guid.NewGuid().ToString();
            //string claimFolder = HttpContext.Session.GetString("TempFolder");
            //if(!string.IsNullOrEmpty(claimFolder))
            //{
            //    // existe tempFolder guardado em session
            //    var folderDeleted = FotoController.DeleteFolder(claimFolder, _environment, _logger, _sessionId, _sessionName);
            //}
            var claimFolder = Guid.NewGuid().ToString();
            var folderCriado = FotoController.CreateFolder(claimFolder, _environment, _logger, _sessionId, _sessionName);
            if (!folderCriado)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetCreateAsync - Erro ao criar o TempFolder");
                _notify.Error($"{_localizer["Erro ao criar o tempFolder."]}");
                return RedirectToAction("Index");
            }
            //HttpContext.Session.SetString("TempFolder", claimFolder);
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetCreateAsync - Foi criado o Folder temporário");

            var claimViewModel = await InitNewClaimAsync(claimFolder);
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetCreateAsync - return _CreateOrEdit para criar nova Claim");
            return View("_CreateOrEdit", claimViewModel);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende o pedido do client para editar claim
        /// é passado o Id da claim a editar
        /// é preparado um claimViewModel com params por Tab e
        /// flags de identificação do user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimFolder"></param>
        /// <returns></returns>

        [Authorize(Policy = Permissions.Claims.Edit)]
        public async Task<IActionResult> OnGetEdit(int id = 0)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetEdit - Entrou para editar claim id=" + id);

            if (id > 0) // editar claim
            {
                _logger.LogInformation("Claim Contoller - OnGetEdit - db Read Claim para edição = " + id);
                var response = await _mediator.Send(new GetClaimByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var claimViewModel = await MapperEntitieToModelClaimAsync(response.Data);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetEdit - return _CreateOrEdit para editar Claim");
                    return View("_CreateOrEdit", claimViewModel);
                }
                else
                {
                    // error
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetEdit - Erro: id inválido");
                    _notify.Error($"{_localizer["Erro inesperado ao editar a reclamação."]}");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                // error
                _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetEdit - Erro: claim id inválido");
                _notify.Error($"{_localizer["Erro inesperado ao editar a reclamação."]}");
                return RedirectToAction("Index");
            }

        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende a new claim posted pelo client.
        /// se a claim for válida é criada na db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimViewModel"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Claims.Create)]
        public async Task<IActionResult> OnPostCreate(int id, ClaimViewModel claimViewModel)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Entrou para post da reclamação=" + id);

            if (!ModelState.IsValid)
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - ModelState Not Valid");
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Total erros = " + ModelState.ErrorCount);

                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Error Key = " + key);
                    }
                }

                // Current Role
                claimViewModel.CurrentRole = await GetCurrentRoleAsync();

                // Prazo limite
                claimViewModel.Prazolimite = await PrazolimiteController.GetPrazolimiteAsync(claimViewModel.DataLimite, _mapper, _mediator);
                claimViewModel.NumeroDiasParaFecho = (int)(claimViewModel.DataLimite - DateTime.Now).TotalDays;

                // Claim
                claimViewModel.Claim.EditMode = false;
                claimViewModel.Claim.NextStatus = await GetNextStatusOptionsAsync(claimViewModel.Status.Tipo);
                claimViewModel.Claim.Empresas = await InitEmpresasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, false);
                claimViewModel.Claim.Gruposlojas = await InitGrupoLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, claimViewModel.Claim.GrupolojaId, false);
                claimViewModel.Claim.Lojas = await InitLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.GrupolojaId, claimViewModel.Claim.LojaId, false);
                SetTabClaimCurrentRole(ref claimViewModel);

                // Artigo
                claimViewModel.Artigo.Tamanhos = await ArtigoController.GetSelectListTamanhos(claimViewModel.Artigo.ArtigoId, claimViewModel.Artigo.TamanhoId, _mediator, _mapper);

                // Cliente
                claimViewModel.Cliente.TipoContactoList = ClienteController.GetSelectListTiposContactos(claimViewModel.Cliente.TipoContacto);

                // Pareceres
                SetTabPareceresCurrentRole(ref claimViewModel);
                SetTabPareceres(ref claimViewModel);

                // Aprovação
                SetTabAprovaçãoCurrentRole(ref claimViewModel);

                // Role do responsável
                claimViewModel.Pareceres.RoleNameResponsável = await GetRoleNameAsync(claimViewModel.EmailAutor);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - returm _CreateOrEdit");
                return View("_CreateOrEdit", claimViewModel);
            }
            else
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - ModelState IsValid");

                //verificar se status da Claim é válido
                if (!await IsClaimValidAsync(claimViewModel))
                {
                    // status da Claim é inválido
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Claim Is Not Valid");

                    // Current Role
                    claimViewModel.CurrentRole = await GetCurrentRoleAsync();

                    // Prazo limite
                    claimViewModel.Prazolimite = await PrazolimiteController.GetPrazolimiteAsync(claimViewModel.DataLimite, _mapper, _mediator);
                    claimViewModel.NumeroDiasParaFecho = (int)(claimViewModel.DataLimite - DateTime.Now).TotalDays;

                    // Claim
                    claimViewModel.Claim.EditMode = false;
                    claimViewModel.Claim.NextStatus = await GetNextStatusOptionsAsync(claimViewModel.Status.Tipo);
                    claimViewModel.Claim.Empresas = await InitEmpresasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, false);
                    claimViewModel.Claim.Gruposlojas = await InitGrupoLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, claimViewModel.Claim.GrupolojaId, false);
                    claimViewModel.Claim.Lojas = await InitLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.GrupolojaId, claimViewModel.Claim.LojaId, false);
                   // claimViewModel.Logo = await EmpresaController.GetEmpresaLogoAsync(claimViewModel.Claim.EmpresaId, _mapper, _mediator);
                    SetTabClaimCurrentRole(ref claimViewModel);

                    // Artigo
                    claimViewModel.Artigo.Tamanhos = await ArtigoController.GetSelectListTamanhos(claimViewModel.Artigo.ArtigoId, claimViewModel.Artigo.TamanhoId, _mediator, _mapper);

                    // Cliente
                    claimViewModel.Cliente.TipoContactoList = ClienteController.GetSelectListTiposContactos(claimViewModel.Cliente.TipoContacto);

                    // Pareceres
                    SetTabPareceresCurrentRole(ref claimViewModel);
                    SetTabPareceres(ref claimViewModel);

                    // Aprovação
                    SetTabAprovaçãoCurrentRole(ref claimViewModel);
                    claimViewModel.Aprovação.EnableAllEditarDecisão = true;

                    // Role do responsável
                    claimViewModel.Pareceres.RoleNameResponsável = await GetRoleNameAsync(claimViewModel.EmailAutor);

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - return _CreateOrEdit");

                    //return new JsonResult(new { isValid = false, html = html2 });
                    return View("_CreateOrEdit", claimViewModel);
                }
                else
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Claim Is Valid");

                    //converter dados das tabs em claim
                    var recl = await MapperModelToEntitieClaimAsync(claimViewModel);

                    //Criar Claim
                    var createClaimCommand = _mapper.Map<CreateClaimCommand>(recl);
                    var result = await _mediator.Send(createClaimCommand);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Claim criada");

                        recl.Id = result.Data;
                        recl.CodeId = await CodeGenerationAsync(recl.Id, recl.DataClaim, recl.EmpresaId, recl.LojaId);

                        // rename folder temporário para CodeId
                        if (!FotoController.RenameFolder(claimViewModel.ClaimFolder, recl.CodeId, _environment, _logger, _sessionId, _sessionName))
                        {
                            // erro a renomear folder temporário - possível duplo request
                            _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Duplo request detetado");

                            // delete fotos 
                            await FotoController.DeleteAllFotosFromDBAsync(id, _mediator, _mapper, _logger, _sessionId, _sessionName);
                            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Todas as fotos da claim removidas");

                            // remover claim entretanto criada
                            var deleteClaimCommand = new DeleteClaimCommand() { Id = recl.Id };

                            _notify.Error($"Erro: Duplo request detetado. Reclamação {recl.Id} removida");
                        }

                        // registar fotos na ClaimId
                        await FotoController.SetFotosInClaimAsync(claimViewModel.ClaimFolder, recl.CodeId, recl.Id, _environment, _mediator, _mapper, _logger, _sessionId, _sessionName);
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Fotos registadas na Claim (db)");

                        var updateClaimCommand = _mapper.Map<UpdateClaimCommand>(recl);
                        var resultUpdt = await _mediator.Send(updateClaimCommand);
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Claim updated");

                        if (resultUpdt.Succeeded) _notify.Success($"{_localizer["str01"]} {resultUpdt.Data} {_localizer["str02"]}");
                    }
                    else
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - Erro:Claim não foi criada");
                        _notify.Error(result.Message);
                    }


                    // return _ViewAll
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - returm to index");
                    return RedirectToAction("Index");
                }

            }

        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende a claim posted pelo client.
        /// se a claim for válida é atualizada na db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimViewModel"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Claims.Edit)]
        public async Task<IActionResult> OnPostEdit(int id, ClaimViewModel claimViewModel)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - Entrou para post da reclamação=" + id);

            if (!ModelState.IsValid)
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - ModelState Not Valid");
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - Total erros = " + ModelState.ErrorCount);

                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - Error Key = " + key);
                    }
                }

                // Current Role
                claimViewModel.CurrentRole = await GetCurrentRoleAsync();

                // Prazo limite
                claimViewModel.Prazolimite = await PrazolimiteController.GetPrazolimiteAsync(claimViewModel.DataLimite, _mapper, _mediator);
                claimViewModel.NumeroDiasParaFecho = (int)(claimViewModel.DataLimite - DateTime.Now).TotalDays;

                // Claim
                claimViewModel.Claim.EditMode = true;
                claimViewModel.Claim.NextStatus = await GetNextStatusOptionsAsync(claimViewModel.Status.Tipo);
                claimViewModel.Claim.Empresas = await InitEmpresasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, false);
                claimViewModel.Claim.Gruposlojas = await InitGrupoLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, claimViewModel.Claim.GrupolojaId, false);
                claimViewModel.Claim.Lojas = await InitLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.GrupolojaId, claimViewModel.Claim.LojaId, false);
                claimViewModel.Logo = await EmpresaController.GetEmpresaLogoAsync(claimViewModel.Claim.EmpresaId, _mapper, _mediator);
                SetTabClaimCurrentRole(ref claimViewModel);

                // Artigo
                claimViewModel.Artigo.Tamanhos = await ArtigoController.GetSelectListTamanhos(claimViewModel.Artigo.ArtigoId, claimViewModel.Artigo.TamanhoId, _mediator, _mapper);

                // Cliente
                claimViewModel.Cliente.TipoContactoList = ClienteController.GetSelectListTiposContactos(claimViewModel.Cliente.TipoContacto);

                // Pareceres
                SetTabPareceresCurrentRole(ref claimViewModel);
                SetTabPareceres(ref claimViewModel);

                // Aprovação
                SetTabAprovaçãoCurrentRole(ref claimViewModel);

                // Role do responsável
                claimViewModel.Pareceres.RoleNameResponsável = await GetRoleNameAsync(claimViewModel.EmailAutor);

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - returm _CreateOrEdit");
                return View("_CreateOrEdit", claimViewModel);
            }
            else
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - ModelState IsValid");

                //verificar se status da Claim é válido
                if (!await IsClaimValidAsync(claimViewModel))
                {
                    // status da Claim é inválido
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - Claim Is Not Valid");

                    // Current Role
                    claimViewModel.CurrentRole = await GetCurrentRoleAsync();

                    // Prazo limite
                    claimViewModel.Prazolimite = await PrazolimiteController.GetPrazolimiteAsync(claimViewModel.DataLimite, _mapper, _mediator);
                    claimViewModel.NumeroDiasParaFecho = (int)(claimViewModel.DataLimite - DateTime.Now).TotalDays;

                    // Claim
                    claimViewModel.Claim.EditMode = true;
                    claimViewModel.Claim.NextStatus = await GetNextStatusOptionsAsync(claimViewModel.Status.Tipo);
                    claimViewModel.Claim.Empresas = await InitEmpresasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, false);
                    claimViewModel.Claim.Gruposlojas = await InitGrupoLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, claimViewModel.Claim.GrupolojaId, false);
                    claimViewModel.Claim.Lojas = await InitLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.GrupolojaId, claimViewModel.Claim.LojaId, false);
                    claimViewModel.Logo = await EmpresaController.GetEmpresaLogoAsync(claimViewModel.Claim.EmpresaId, _mapper, _mediator);
                    SetTabClaimCurrentRole(ref claimViewModel);

                    // Artigo
                    claimViewModel.Artigo.Tamanhos = await ArtigoController.GetSelectListTamanhos(claimViewModel.Artigo.ArtigoId, claimViewModel.Artigo.TamanhoId, _mediator, _mapper);

                    // Cliente
                    claimViewModel.Cliente.TipoContactoList = ClienteController.GetSelectListTiposContactos(claimViewModel.Cliente.TipoContacto);

                    // Pareceres
                    SetTabPareceresCurrentRole(ref claimViewModel);
                    SetTabPareceres(ref claimViewModel);

                    // Aprovação
                    SetTabAprovaçãoCurrentRole(ref claimViewModel);

                    // Role do responsável
                    claimViewModel.Pareceres.RoleNameResponsável = await GetRoleNameAsync(claimViewModel.EmailAutor);

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCreate - return _CreateOrEdit");
                    //return new JsonResult(new { isValid = false, html = html2 });
                    return View("_CreateOrEdit", claimViewModel);
                }
                else
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - Claim Is Valid");

                    //converter dados das tabs em claim
                    var recl = await MapperModelToEntitieClaimAsync(claimViewModel);

                    //Update Claim
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - vai fazer update Claim editada");

                    //atualizar CodeId
                    recl.Id = id;
                    recl.CodeId = await CodeGenerationAsync(recl.Id, recl.DataClaim, recl.EmpresaId, recl.LojaId);

                    // rename folder original se necessário
                    if (!String.Equals(recl.CodeId, claimViewModel.CodeId))
                    {
                        var folderRenamed = FotoController.RenameFolder(claimViewModel.CodeId, recl.CodeId, _environment, _logger, _sessionId, _sessionName);
                        await FotoController.UpdatePathFotosInClaimAsync(claimViewModel.CodeId, recl.CodeId, recl.Id, _environment, _mediator, _mapper, _logger, _sessionId, _sessionName);
                    }

                    // update db
                    var updateClaimCommand = _mapper.Map<UpdateClaimCommand>(recl);
                    var result = await _mediator.Send(updateClaimCommand);

                    if (result.Succeeded)
                    {
                        _notify.Information($"{_localizer["str01"]} {result.Data} {_localizer["str02"]}");
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - Claim editada foi escrita na db");
                    }
                    else
                    {
                        _notify.Error($"{_localizer["str01"]} {result.Data} {_localizer["str03"]}");
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - Erro:Claim não foi atualizada");
                    }

                    // return _ViewAll
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostEdit - returm to index");
                    return RedirectToAction("Index");
                }

            }

        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função que atende o cancel do editar/criar claim
        /// </summary>
        /// <param name="id"></param>
        /// <param name="claimFolder"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Claims.Edit)]
        public async Task<IActionResult> OnPostCancelAsync(int id = 0, string claimFolder="")
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCancelAsync - Entrou para cancelar editar/criar reclamação claimFolder=" + claimFolder);

            if (id == 0)
            {
                // create claim was canceled
                if (!string.IsNullOrEmpty(claimFolder))
                {
                    FotoController.DeleteFolder(claimFolder, _environment, _logger, _sessionId, _sessionName);
                    HttpContext.Session.Remove("TempFolder");
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostCancelAsync - TempFolder removido tempFolder=" + claimFolder);
                    await FotoController.DeleteFotosInFolderFromDBAsync(claimFolder, _mediator, _mapper, _logger, _sessionId, _sessionName);
                }
            }

            _notify.Information($"{_localizer["Criar nova reclamação foi cancelada."]}");
            //return RedirectToAction("Index");
            return Json(new { redirectToUrl = Url.Action("Index") });
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para atender o delete de uma claim.
        /// faz delete das fotos na db e no folder da aplicação
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Authorize(Policy = Permissions.Claims.Delete)]
        [HttpPost]
        public async Task<JsonResult> OnPostDelete(int id)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostDelete - Entrou para delete de ClaimId=" + id);

            var responseClaim = await _mediator.Send(new GetClaimByIdQuery() { Id = id });
            var claimViewModel = _mapper.Map<ClaimViewModel>(responseClaim.Data);

            // delete fotos 
            await FotoController.DeleteAllFotosFromDBAsync(id, _mediator, _mapper, _logger, _sessionId, _sessionName);
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Todas as fotos da claim removidas");

            FotoController.DeleteFolder(claimViewModel.CodeId, _environment, _logger, _sessionId, _sessionName);
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Folder ClaimId apagado");

            // delete claim
            var deleteCommand = await _mediator.Send(new DeleteClaimCommand { Id = id });
            if (deleteCommand.Succeeded)
            {
                _notify.Information($"{_localizer["str01"]} {id} {_localizer["str04"]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Claim removida com sucesso");
            }
            else
            {
                _notify.Error(deleteCommand.Message);
                _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Claim removida com erro");
            }

            // delete pareceres
            if (claimViewModel.ParecerAdministraçãoId > 0)
            {
                // remover Parecer
                var resultAd = await _mediator.Send(new DeleteParecerCommand { Id = (int) claimViewModel.ParecerAdministraçãoId });
                if (resultAd.Succeeded) _notify.Success($"{_localizer["str05"]} {resultAd.Data} {_localizer["str06"]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Parecer Admin removido");
            }
            if (claimViewModel.ParecerRevisorId > 0)
            {
                // remover Parecer
                var resultRev = await _mediator.Send(new DeleteParecerCommand { Id = (int) claimViewModel.ParecerRevisorId });
                if (resultRev.Succeeded) _notify.Success($"{_localizer["str05"]} {resultRev.Data} {_localizer["str06"]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Parecer Revisor removido");
            }
            if (claimViewModel.ParecerSupervisorId > 0)
            {
                // remover Parecer
                var resultSup = await _mediator.Send(new DeleteParecerCommand { Id = (int) claimViewModel.ParecerSupervisorId });
                if (resultSup.Succeeded) _notify.Success($"{_localizer["str05"]} {resultSup.Data} {_localizer["str06"]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Parecer Supervisor removido");
            }
            if (claimViewModel.ParecerGerenteLojaId > 0)
            {
                // remover Parecer
                var resultGL = await _mediator.Send(new DeleteParecerCommand { Id = (int) claimViewModel.ParecerGerenteLojaId });
                if (resultGL.Succeeded) _notify.Success($"{_localizer["str05"]} {resultGL.Data} {_localizer["str06"]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Parecer Gerente de loja removido");
            }
            if (claimViewModel.ParecerColaboradorId > 0)
            {
                // remover Parecer
                var resultCol = await _mediator.Send(new DeleteParecerCommand { Id = (int) claimViewModel.ParecerColaboradorId });
                if (resultCol.Succeeded) _notify.Success($"{_localizer["str05"]} {resultCol.Data} {_localizer["str06"]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostDelete - Parecer Colaborador removido");
            }
            if (claimViewModel.ParecerResponsavelId > 0)
            {
                // remover Parecer
                var resultResp = await _mediator.Send(new DeleteParecerCommand { Id = (int)claimViewModel.ParecerResponsavelId });
                if (resultResp.Succeeded) _notify.Success($"{_localizer["str05"]} {resultResp.Data} {_localizer["str06"]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostDelete - Parecer Responsavel removido");
            }

            // return _ViewAll
            var viewModel = new List<ClaimListViewModel>();
            var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
            _logger.LogInformation(_sessionId + " | " + _sessionName +  " | Claim Contoller - OnPostCreateOrEdit - returm _ViewAll com lita de Claims");

            return new JsonResult(new { isValid = true, html = html });
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Claims.Edit)]
        public async Task<ActionResult> OnGetPrintPdf(int id)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetPrintPdf - Entrou para print PDF de ClaimId=" + id);

            if (id > 0)
            {
                var responseClaim = await _mediator.Send(new GetClaimByIdQuery() { Id = id });
                if (responseClaim.Succeeded)
                {
                    var claimViewModel = await MapperEntitieToModelClaimAsync(responseClaim.Data);

                    // print claim
                    var document = new PdfDocument();
                    var newPage = document.InsertPage(0);

                    // get an XGraphics object for drawing
                    XGraphics gfx = XGraphics.FromPdfPage(newPage);

                    // create a font
                    XFont fontHeader = new XFont("Arial", 10, XFontStyleEx.Italic);
                    XFont fontFooter = new XFont("Arial", 10, XFontStyleEx.Regular);

                    // draw cabeçalho
                    var cab = _localizer["LVL GROUP - Portal de Reclamações - Impresso em"] + " " + DateTime.Now.ToString("dd/MM/yyyy");
                    gfx.DrawString(cab, fontHeader, XBrushes.Black, new XRect(20, 20, 200, 72), XStringFormats.TopLeft);

                    // draw header
                    document = CreatePdfHeader(claimViewModel, document, gfx, 50);

                    // draw cliente
                    document = WritePdfClientSection(claimViewModel, document, gfx, 190);

                    // draw artigo
                    document = WritePdfArtigoSection(claimViewModel, document, gfx, 300);

                    // draw parecer
                    document = WritePdfParecerSection(claimViewModel, document, gfx, 410);

                    // draw aprovação
                    document = WritePdfAprovaçãoSection(claimViewModel, document, gfx, 520);

                    // draw resolução com o cliente
                    document = WritePdfResoluçãoSection(claimViewModel, document, gfx, 630);

                    // draw footer
                    var footer = DateTime.Now.Year + "  LVL GROUP - ESTABELECER | ADMINISTRAR | MULTIPLICAR";
                    gfx.DrawString(footer, fontFooter, XBrushes.Black, new XRect(20, 820, 200, 72), XStringFormats.TopLeft);


                    // salvar pdf document
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetPrintPdf - return PDF file");
                    byte[] response = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        document.Save(ms);
                        response = ms.ToArray();
                    }
                    string Filename = claimViewModel.CodeId + ".pdf";
                    return File(response, "application/pdf", Filename);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para forçar status da reclamação 'id'.
        /// permite colocar reclamação em AGUARDA VALIDAÇÃO ou AGUARDA DECISÃO
        /// devolve a view ao client.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [Authorize(Policy = Permissions.Claims.Create)]
        public async Task<JsonResult> OnGetForceStatus(int id)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetForceStatus - Entrou para forçar status da claim id=" + id);

            if (id > 0)
            {
                // get view para force status
                _logger.LogInformation("Claim Contoller - OnGetForceStatus - db Read Claim para forçar status da claimId = " + id);
                var response = await _mediator.Send(new GetClaimByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    var forcestatusViewModel = new ForceStatusViewModel
                    {
                        Id = response.Data.Id,
                        CodeId = response.Data.CodeId,
                        StatusId = response.Data.StatusId,
                        NextStatusId = response.Data.StatusId,
                        AllStatus = await StatusController.GetSelectListAllStatusAsync(response.Data.StatusId, _mapper, _mediator),
                        GoBackStatus = await StatusController.GetSelectListGoBackStatusAsync(_mapper, _mediator),
                    };

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetForceStatus - return _ForceStatus para forçar status da Claim");
                    return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_ForceStatus", forcestatusViewModel) });
                }
                // error
                _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetForceStatus - Erro: reclamação inválida");
                _notify.Error($"{_localizer["Erro inesperado - reclamação inválida."]}");

                // return _ViewAll
                var viewModel = new List<ClaimListViewModel>();
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetForceStatus - returm _ViewAll com lita de Claims");

                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                // error
                _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetForceStatus - Erro: claim id inválido");
                _notify.Error($"{_localizer["Erro inesperado - reclamação inválida."]}");

                // return _ViewAll
                var viewModel = new List<ClaimListViewModel>();
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetForceStatus - returm _ViewAll com lita de Claims");

                return new JsonResult(new { isValid = true, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// força o status da reclamação para o valor passado em 'nextstatusId'.
        /// faz update do campo 'StatusId' da reclamação
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nextstatusId"></param>
        /// <param name="forceStatus"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Claims.Create)]
        public async Task<JsonResult> OnPostForceStatusAsync(int id, int nextstatusId, ForceStatusViewModel forceStatus)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostForceStatusAsync - Entrou para forçar status de ClaimId=" + id);

            if (id > 0 && nextstatusId > 0)
            {
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostForceStatusAsync - ModelState is Valid - vai ler claim da db");

                var response = await _mediator.Send(new GetClaimByIdQuery() { Id = id });
                if (response.Succeeded)
                {
                    // update status da claim
                    var recl = _mapper.Map<Core.Entities.Claims.Claim>(response.Data);
                    recl.StatusId = nextstatusId;

                    // update db
                    var updateClaimCommand = _mapper.Map<UpdateClaimCommand>(recl);
                    var result = await _mediator.Send(updateClaimCommand);

                    if (result.Succeeded)
                    {
                        _notify.Information($"{_localizer["str01"]} {result.Data} {_localizer["str02"]}");
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostForceStatusAsync - Claim com status forçado foi escrita na db");
                    }
                    else
                    {
                        _notify.Error($"{_localizer["str01"]} {result.Data} {_localizer["str03"]}");
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostForceStatusAsync - Erro:Claim não foi atualizada");
                    }
                }
                // return _ViewAll
                var viewModel = new List<ClaimListViewModel>();
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                _notify.Error($"{_localizer["str01"]} {id} {_localizer["str03"]}");
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostForceStatusAsync - Erro:Claim não foi atualizada");
                // return _ViewAll
                var viewModel = new List<ClaimListViewModel>();
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", viewModel);
                return new JsonResult(new { isValid = true, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função que atende o botão de limpar os Temp Folders.
        /// remove os registos das fotos na db.
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Claims.Delete)]
        public JsonResult OnPostClean()
        {
            var success = FotoController.DeleteAllTempFolders(_environment, _logger, _sessionId, _sessionName);
            return Json(new { status = "success" });
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atende opção de reparação de uma reclamação.
        /// verifica se as fotos da reclamação existem no folder.
        /// se não existirem procura por elas nos folders temporários.
        /// se encontradas, move-as para o folder da reclamação.
        /// se não encontradas remove os registos das fotos na db.
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        //[Authorize(Policy = Permissions.Claims.Edit)]
        public async Task<JsonResult> OnPostRepairClaimAsync(int id)
        {
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetRepairClaimAsync - Entrou para reparar fotos em ClaimId=" + id);

            try
            {
                if (id > 0)
                {
                    var responseClaim = await _mediator.Send(new GetClaimByIdQuery() { Id = id });
                    if (!responseClaim.Succeeded)
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetRepairClaimAsync - Erro ao ler claim da db");
                        return Json(new { status = "error" });
                    }

                    var claim = _mapper.Map<ClaimListViewModel>(responseClaim.Data);
                    var repaired = await RepairClaimFotosAsync(claim);
                    if (repaired)
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetRepairClaimAsync - Claim reparada com sucesso = " + id);
                        return Json(new { status = "success" });
                    }

                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetRepairClaimAsync - Erro ao reparar Claim = " + id);
                    return Json(new { status = "error" });
                }
                else
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnGetRepairClaimAsync - Claim inválida");
                    return Json(new { status = "error" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _sessionId + " | " + _sessionName + " | Claim Contoller - OnGetRepairClaimAsync - Erro: " + ex.Message);
                return Json(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função que atende o botão de verificar os Folders de fotos.
        /// se uma reclamação não tiver folder, é criado um folder para a claim.
        /// verifica se os registos das fotos na db existem no filesystem.
        /// se uma foto não existir no folder, a foto é procurada nos folders temporários.
        /// se for encontrada, é movida para o folder da reclamação.
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = Permissions.Claims.Delete)]
        public async Task<JsonResult> OnPostVerifyFoldersAsync()
        {
            try
            {
                //verificar se todas as claims têm folders consistentes
                var response = await _mediator.Send(new GetAllClaimsCachedQuery());
                if (response.Succeeded)
                {
                    var claimList = _mapper.Map<List<ClaimListViewModel>>(response.Data).AsQueryable();
                    foreach (var item in claimList)
                    {
                        var repaired = await AdvancedRepairClaimFotosAsync(item);
                    }
                }

                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - OnPostVerifyFoldersAsync - Todas as Claims foram verificadas");
                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _sessionId + " | " + _sessionName + " | Claim Contoller - OnPostVerifyFoldersAsync - Erro: " + ex.Message);
                return Json(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica a consistencia das fotos de uma claim.
        /// </summary>
        /// <param name="claim"></param>
        /// <returns>bool</returns>

        internal async Task<bool> AdvancedRepairClaimFotosAsync(ClaimListViewModel claim)
        {
            if (claim == null) return false;
            //_logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - RepairClaimFotosAsync - Entrou para reparar fotos em ClaimId=" + claim.Id);

            try
            {
                //verificar se claim tem folder
                if (!FotoController.FolderExiste(claim.CodeId, _environment, _logger, _sessionId, _sessionName))
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Claim a corrigir (Folder vai ser criado) = " + claim.CodeId);
                    if (FotoController.CreateFolder(claim.CodeId, _environment, _logger, _sessionId, _sessionName))
                    {
                        //_logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Folder criado com sucesso = " + claim.CodeId);
                    }
                    else
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Erro ao criar Folder = " + claim.CodeId);
                        return false;
                    }
                }

                //verificar se todas as fotos na db existem no folder
                var responseFotos = await _mediator.Send(new GetAllFotosByClaimIdCachedQuery() { claimId = claim.Id });
                if (responseFotos.Succeeded)
                {
                    var fotoList = _mapper.Map<List<FotoViewModel>>(responseFotos.Data).AsQueryable();
                    //_logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Verificar existência de fotos no folder = " + claim.CodeId);
                    foreach (var foto in fotoList)
                    {
                        if (!FotoController.FotoInFolderExiste(foto.FileName, claim.CodeId, _environment, _logger, _sessionId, _sessionName))
                        {
                            // foto não existe no folder
                            _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Foto " + foto.FileName + " não existe no folder = " + claim.CodeId);

                            // procurar foto nos folders temporários
                            var tempFolder = FotoController.FindFotoInTempFolders(foto.FileName, _environment, _logger, _sessionId, _sessionName);
                            if (!string.IsNullOrEmpty(tempFolder))
                            {
                                // mover foto para o folder da claim (o mais provavel é ter que mover todas as fotos desse temp folder)
                                FotoController.MoveFotoFromTempToClaimFolder(foto.FileName, tempFolder, claim.CodeId, _environment, _logger, _sessionId, _sessionName);
                                _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Foto " + foto.FileName + " movida para o folder = " + claim.CodeId);
                            }
                            else
                            {
                                //_logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Erro: Foto " + foto.FileName + " não foi encontrada nos folders temporários = " + claim.CodeId);
                                // remover foto da db
                                var result = await _mediator.Send(new DeleteFotoCommand { Id = foto.Id });
                                if (result.Succeeded)
                                {
                                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Foto " + foto.FileName + " removida da db) = " + claim.CodeId);
                                }
                                else
                                {
                                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Erro: Foto " + foto.FileName + " não foi removida da db) = " + claim.CodeId);
                                }
                                return false;
                            }
                        }
                        else
                        {
                            // foto existe no folder
                            //_logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Foto " + foto.FileName + " existe no folder = " + claim.CodeId);
                        }
                    }
                }
                else
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Erro ao ler da db as fotos da Claim = " + claim.CodeId);
                    return false;
                }

                //_logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Claim verificada = " + claim.CodeId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _sessionId + " | " + _sessionName + " | Claim Contoller - AdvancedRepairClaimFotosAsync - Erro: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Reinicializa e limpa as fotos de uma claim
        /// </summary>
        /// <param name="claim"></param>
        /// <returns>bool</returns>

        internal async Task<bool> RepairClaimFotosAsync(ClaimListViewModel claim)
        {
            if (claim == null) return false;
            //_logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - RepairClaimFotosAsync - Entrou para reparar fotos em ClaimId=" + claim.Id);

            try
            {
                // remover folder da claim se existir
                FotoController.DeleteFolder(claim.CodeId, _environment, _logger, _sessionId, _sessionName);

                // criar novo folder para a claim
                if (FotoController.CreateFolder(claim.CodeId, _environment, _logger, _sessionId, _sessionName))
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - RepairClaimFotosAsync - Folder recriado com sucesso = " + claim.CodeId);
                }
                else
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - RepairClaimFotosAsync - Erro ao criar Folder = " + claim.CodeId);
                    return false;
                }

                // remover fotos da db
                if (await FotoController.DeleteAllFotosFromDBAsync(claim.Id, _mediator, _mapper, _logger, _sessionId, _sessionName))
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | Claim Contoller - RepairClaimFotosAsync - Fotos removidas da db com sucesso.");
                    return true;
                }
                _logger.LogError(_sessionId + " | " + _sessionName + " | Claim Contoller - RepairClaimFotosAsync - Erro ao remover fotos da db.");
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _sessionId + " | " + _sessionName + " | Claim Contoller - RepairClaimFotosAsync - Erro: " + ex.Message);
                return false;
            }
        }

        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara a lista de claims existentes tendo em conta 
        /// o role do user corrente.
        /// a tabela de claims é carregada com esta lista em _ViewAll
        /// </summary>
        /// <returns></returns>

        internal async Task<IQueryable<ClaimListViewModel>> GetClaimsListAsync()
        {
            var userId = _signInManager.UserManager.GetUserId(User);
            var currentUser = await _signInManager.UserManager.FindByIdAsync(userId);

            var isSuperAdmin = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.SuperAdmin.ToString());
            var isAdmin = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Admin.ToString());
            var isSupervisor = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Supervisor.ToString());
            var isRevisor = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Revisor.ToString());
            var isGerenteLoja = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.GerenteLoja.ToString());
            var isColaborador = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Colaborador.ToString());
            var isBasic = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Basic.ToString());


            var empresaId = currentUser.EmpresaId == null ? 0 : (int) currentUser.EmpresaId;
            var grupolojaId = currentUser.GrupolojaId == null ? 0 : (int)currentUser.GrupolojaId;
            var lojaId = currentUser.LojaId == null ? 0 : (int)currentUser.LojaId;

            var viewModelList = new List<ClaimListViewModel>().AsQueryable();

            if (isSuperAdmin || isAdmin || isRevisor) // todas as claims
            {
                var response = await _mediator.Send(new GetAllClaimsCachedQuery());
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<ClaimListViewModel>>(response.Data).AsQueryable();
                }
            }

            if (isSupervisor) // claims de grupoloja
            {
                var response = await _mediator.Send(new GetAllClaimsByGrupolojaIdCachedQuery() { grupolojaId = grupolojaId });
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<ClaimListViewModel>>(response.Data).AsQueryable();
                }
            }

            if (isGerenteLoja || isColaborador || isBasic) // claims de loja
            {
                var response = await _mediator.Send(new GetAllClaimsByLojaIdCachedQuery() { lojaId = lojaId });
                if (response.Succeeded)
                {
                    viewModelList = _mapper.Map<List<ClaimListViewModel>>(response.Data).AsQueryable();
                }
            }

            return viewModelList;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// evocada no atendimento POST OnPostCreateOrEdit.
        /// validação da claim posted pelo client
        /// faz a validação da mudança de status e valida os dados
        /// tendo em conta o novo status e o previous.
        /// o client é informado por notificação.
        /// </summary>
        /// <param name="claimViewModel"></param>
        /// <returns></returns>

        internal async Task<bool> IsClaimValidAsync(ClaimViewModel claimViewModel)
        {
            // validar Data Entrada no Portal
            if (claimViewModel.DataEntradaSistemaClaim.Date < claimViewModel.Claim.DataClaim.Date)
            {
                _notify.Error(_localizer["erro01"]);
                return false;
            }

            // validar Empresa e Artigo
            if (claimViewModel.Artigo.EmpresaId != claimViewModel.Claim.EmpresaId)
            {
                _notify.Error(_localizer["erro02"]);
                return false;
            }

            // validar numero Total de Fotos uploaded
            claimViewModel.TotalFotos = await FotoController.CountFotosInFolderAsync(claimViewModel.ClaimFolder, claimViewModel.Id, _mediator, _mapper, _logger, _sessionId, _sessionName);
            if (claimViewModel.TotalFotos <= 2)     //if (claimViewModel.TotalFotos < 0)
            {
                _notify.Error(_localizer["erro03"]);
                return false;
            }

            // validar parecer técnico do Responsável
            if (String.IsNullOrEmpty(claimViewModel.Pareceres.ParecerResponsavel.Opinião))
            {
                _notify.Error(_localizer["erro04"]);
                return false;
            }

            // validar status corrente e seguinte
            var currentTipoId = await StatusController.GetStatusTipoAsync(claimViewModel.Status.Id, _mapper, _mediator);
            var nextTipoId = await StatusController.GetStatusTipoAsync(claimViewModel.Claim.NextStatusId, _mapper, _mediator);

            // validar status inicial de claim a ser criada
            if (currentTipoId == 0) return IsNewClaimValid(claimViewModel, nextTipoId);

            // validar status final de claim fechada
            if (currentTipoId == (int)StatusType.FechadaEmLojaRejeitada ||
                currentTipoId == (int)StatusType.FechadaEmLojaTrocaDireta ||
                currentTipoId == (int)StatusType.FechadaEmLojaReparaçãoArtigo ||
                currentTipoId == (int)StatusType.FechadaEmLojaTrocaArtigo ||
                currentTipoId == (int)StatusType.FechadaEmLojaDevoluçãoDinheiro ||
                currentTipoId == (int)StatusType.FechadaEmLojaNotaDeCrédito) return IsClosedClaimValid(claimViewModel.Aprovação, currentTipoId);

            // validar status intermédios no ciclo da vida da claim
            switch (currentTipoId)
            {
                case (int)StatusType.PendenteEmLoja:
                    // validar status corrente = "PENDENTE EM LOJA: AGUARDA MAIS FOTOS"
                    // validar status corrente = "PENDENTE EM LOJA: AGUARDA TESTE INFILTRAÇÃO"
                    // validar status corrente = "PENDENTE EM LOJA: AGUARDA TESTE CAMURÇA"
                    // validar status corrente = "PENDENTE EM LOJA: AGUARDA TESTE ALGODÃO"
                    // validar Opinião do Colaborador
                    if (!await AreMultiPareceresUpdated(claimViewModel.Pareceres.ParecerColaborador, "Colaborador", claimViewModel.Pareceres.ParecerGerenteLoja, "Gerente de Loja", nextTipoId, true) &&
                        !await IsParecerUpdated(claimViewModel.Pareceres.ParecerResponsavel, "Responsável", nextTipoId, true)
                       ) return false;
                    break;

                case (int)StatusType.AguardaValidação:
                    if (nextTipoId == (int)StatusType.PendenteEmLoja || nextTipoId == (int)StatusType.AguardaDecisão)
                    {
                        // validar Opinião do Revisor ou Admin
                        if ( !await AreMultiPareceresUpdated(claimViewModel.Pareceres.ParecerRevisor, "Revisor", claimViewModel.Pareceres.ParecerAdministração, "Administrador", nextTipoId, true)) return false;
                    }
                    break;

                case (int)StatusType.AguardaDecisão:
                    if (nextTipoId == (int)StatusType.PendenteEmLoja || 
                        nextTipoId == (int)StatusType.AguardaOpiniãoGerenteLoja ||
                        nextTipoId == (int)StatusType.AguardaOpiniãoSupervisor ||
                        nextTipoId == (int)StatusType.AguardaOpiniãoRevisor ||
                        nextTipoId == (int)StatusType.AguardaOpiniãoFornecedor )
                    {
                        // validar Opinião do Admin
                        if (!await IsParecerUpdated(claimViewModel.Pareceres.ParecerAdministração, "Administrador", nextTipoId, true)) return false;
                    }
                    if (nextTipoId == (int)StatusType.Aceite || nextTipoId == (int)StatusType.NãoAceite)
                    {
                        // validar decisão
                        if (String.IsNullOrEmpty(claimViewModel.Aprovação.DecisãoFinal))
                        {
                            _notify.Error(_localizer["erro05"]);
                            return false;
                        }
                    }
                    break;

                case (int)StatusType.AguardaOpiniãoGerenteLoja:
                    // validar Opinião do Gerente de Loja
                    if (!await IsParecerUpdated(claimViewModel.Pareceres.ParecerGerenteLoja, "Gerente de Loja", nextTipoId, true)) return false;
                    break;

                case (int)StatusType.AguardaOpiniãoSupervisor:
                    // validar Opinião do Supervisor
                    if (!await IsParecerUpdated(claimViewModel.Pareceres.ParecerSupervisor, "Supervisor", nextTipoId, true)) return false;
                    break;

                case (int)StatusType.AguardaOpiniãoRevisor:
                    // validar Opinião do Revisor
                    if (!await IsParecerUpdated(claimViewModel.Pareceres.ParecerRevisor, "Revisor", nextTipoId, true)) return false;
                    break;

                case (int)StatusType.AguardaOpiniãoFornecedor:
                    // validar Opinião do Revisor
                    if (!await IsParecerUpdated(claimViewModel.Pareceres.ParecerRevisor, "Revisor", nextTipoId, true)) return false;
                    break;

                case (int)StatusType.Aceite:
                    // validar decisão
                    if (String.IsNullOrEmpty(claimViewModel.Aprovação.DecisãoFinal))
                    {
                        _notify.Error(_localizer["erro05"]);
                        return false;
                    }

                    if (nextTipoId == (int)StatusType.FechadaEmLojaReparaçãoArtigo)
                    {
                        // validar Fecho de claim em loja
                        if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                        {
                            _notify.Error(_localizer["erro06"]);
                            return false;
                        }
                        if (!claimViewModel.Aprovação.ReparaçãoArtigo)
                        {
                            _notify.Error(_localizer["erro07"]);
                            return false;
                        }
                    }
                    if (nextTipoId == (int)StatusType.FechadaEmLojaTrocaArtigo)
                    {
                        // validar Fecho de claim em loja
                        if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                        {
                            _notify.Error(_localizer["erro06"]);
                            return false;
                        }
                        if (!claimViewModel.Aprovação.TrocaArtigo)
                        {
                            _notify.Error(_localizer["erro08"]);
                            return false;
                        }
                    }
                    if (nextTipoId == (int)StatusType.FechadaEmLojaDevoluçãoDinheiro)
                    {
                        // validar Fecho de claim em loja
                        if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                        {
                            _notify.Error(_localizer["erro06"]);
                            return false;
                        }
                        if (!claimViewModel.Aprovação.DevoluçãoDinheiro)
                        {
                            _notify.Error(_localizer["erro09"]);
                            return false;
                        }
                    }
                    if (nextTipoId == (int)StatusType.FechadaEmLojaNotaDeCrédito)
                    {
                        // validar Fecho de claim em loja
                        if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                        {
                            _notify.Error(_localizer["erro06"]);
                            return false;
                        }
                        if (!claimViewModel.Aprovação.NotaCrédito)
                        {
                            _notify.Error(_localizer["erro10"]);
                            return false;
                        }
                    }
                    break;

                case (int)StatusType.NãoAceite:
                    // validar decisão
                    if (String.IsNullOrEmpty(claimViewModel.Aprovação.DecisãoFinal))
                    {
                        _notify.Error(_localizer["erro05"]);
                        return false;
                    }
                    if (!claimViewModel.Aprovação.Rejeitada)
                    {
                        _notify.Error(_localizer["erro11"]);
                        return false;
                    }
                    if (nextTipoId == (int)StatusType.AguardaRelatório)
                    {
                        return true;
                    }
                    if (nextTipoId == (int)StatusType.FechadaEmLojaRejeitada)
                    {
                        // validar Fecho de claim em loja
                        if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                        {
                            _notify.Error(_localizer["erro06"]);
                            return false;
                        }
                        if (!claimViewModel.Aprovação.Rejeitada)
                        {
                            _notify.Error(_localizer["erro11"]);
                            return false;
                        }
                    }
                    break;

                case (int)StatusType.FechadaEmLojaReparaçãoArtigo:
                    // validar Fecho de claim em loja
                    if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                    {
                        _notify.Error(_localizer["erro06"]);
                        return false;
                    }
                    if (!claimViewModel.Aprovação.ReparaçãoArtigo)
                    {
                        _notify.Error(_localizer["erro07"]);
                        return false;
                    }
                    break;

                case (int)StatusType.FechadaEmLojaDevoluçãoDinheiro:
                    // validar Fecho de claim em loja
                    if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                    {
                        _notify.Error(_localizer["erro06"]);
                        return false;
                    }
                    if (!claimViewModel.Aprovação.DevoluçãoDinheiro)
                    {
                        _notify.Error(_localizer["erro09"]);
                        return false;
                    }
                    break;

                case (int)StatusType.FechadaEmLojaTrocaArtigo:
                    // validar Fecho de claim em loja
                    if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                    {
                        _notify.Error(_localizer["erro06"]);
                        return false;
                    }
                    if (!claimViewModel.Aprovação.TrocaArtigo)
                    {
                        _notify.Error(_localizer["erro08"]);
                        return false;
                    }
                    break;

                case (int)StatusType.FechadaEmLojaNotaDeCrédito:
                    // validar Fecho de claim em loja
                    if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                    {
                        _notify.Error(_localizer["erro06"]);
                        return false;
                    }
                    if (!claimViewModel.Aprovação.NotaCrédito)
                    {
                        _notify.Error(_localizer["erro10"]);
                        return false;
                    }
                    break;

                case (int)StatusType.FechadaEmLojaRejeitada:
                    // validar Fecho de claim em loja
                    if (String.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho))
                    {
                        _notify.Error(_localizer["erro06"]);
                        return false;
                    }
                    if (!claimViewModel.Aprovação.Rejeitada)
                    {
                        _notify.Error(_localizer["erro11"]);
                        return false;
                    }
                    break;

                case (int)StatusType.AguardaRespostaFornecedor:
                    // validar Opinião do Revisor
                    if (!await IsParecerUpdated(claimViewModel.Pareceres.ParecerRevisor, "Revisor", nextTipoId, true)) return false;
                    break;

                case (int)StatusType.AguardaRelatório:
                    //if (!await IsParecerUpdated(claimViewModel.Pareceres.ParecerRevisor, "Revisor", nextTipoId, true)) return false;
                    return true;

                case (int)StatusType.RelatórioDisponível:
                    return true;

                default:
                    _notify.Error(_localizer["erro12"]);
                    return false;
            } 

            return true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// valida uma nova claim acabada de criar
        /// </summary>
        /// <param name="claimViewModel"></param>
        /// <param name="nextTipo"></param>
        /// <returns></returns>

        internal bool IsNewClaimValid(ClaimViewModel claimViewModel, int nextTipo)
        {
            // validar NextStatus
            // NextStatus deverá ser "AguardaValidação" ou "FecharEmLojaTrocaDireta"
            if (claimViewModel.Claim.NextStatusId == 0)
            {
                _notify.Error(_localizer["erro13"]);
                return false;
            }
            if (nextTipo == (int)StatusType.AguardaValidação) return true;
            if (nextTipo == (int)StatusType.FechadaEmLojaTrocaDireta)
            {
                // validar aprovação
                //if (!IsClosedClaimValid(claimViewModel.Aprovação, nextTipo)) return false;
                return IsClosedClaimValid(claimViewModel.Aprovação, nextTipo);
            }
            return true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// valida uma claim já fechada e que foi editada
        /// ou acabada de criar como troca direta.
        /// garante que a tab aprovação está correcta.
        /// o NextStatus é ignorado porque a claim já foi fechada.
        /// </summary>
        /// <param name="aprovação"></param>
        /// <param name="tipoId"></param>
        /// <returns></returns>

        internal bool IsClosedClaimValid(TabAprovaçãoViewModel aprovação, int tipoId)
        {
            if (String.IsNullOrEmpty(aprovação.DecisãoFinal))
            {
                _notify.Error(_localizer["erro05"]);
                return false;
            }
            if (String.IsNullOrEmpty(aprovação.ObservaçõesFecho))
            {
                _notify.Error(_localizer["erro06"]);
                return false;
            }
            if (tipoId == (int)StatusType.FechadaEmLojaTrocaDireta)
            {
                // validar aprovação
                if (!aprovação.TrocaArtigo)
                {
                    _notify.Error(_localizer["erro08"]);
                    return false;
                }
            }
            if (tipoId == (int)StatusType.FechadaEmLojaReparaçãoArtigo)
            {
                // validar aprovação
                if (!aprovação.ReparaçãoArtigo)
                {
                    _notify.Error(_localizer["erro07"]);
                    return false;
                }
            }
            if (tipoId == (int)StatusType.FechadaEmLojaTrocaArtigo)
            {
                // validar aprovação
                if (!aprovação.TrocaArtigo)
                {
                    _notify.Error(_localizer["erro08"]);
                    return false;
                }
            }
            if (tipoId == (int)StatusType.FechadaEmLojaDevoluçãoDinheiro)
            {
                // validar aprovação
                if (!aprovação.DevoluçãoDinheiro)
                {
                    _notify.Error(_localizer["erro09"]);
                    return false;
                }
            }
            if (tipoId == (int)StatusType.FechadaEmLojaNotaDeCrédito)
            {
                // validar aprovação
                if (!aprovação.NotaCrédito)
                {
                    _notify.Error(_localizer["erro10"]);
                    return false;
                }
            }
            return true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// converte uma ClaimViewModel em Claim
        /// a ClaimViewModel posted pelo client integra estruturas de dados
        /// dedicadas a cada Tab da claim. Esta função retira todos os dados
        /// dessas estruturas e cria a entidade necessária para escrever na db.
        /// regista o novo status e o prvious status da claim.
        /// pelo meio insere/atualiza os Pareceres da claim.
        /// atualiza também os dados do cliente associados à claim.
        /// evocada no atendimento POST OnPostCreateOrEdit.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        internal async Task<Core.Entities.Claims.Claim> MapperModelToEntitieClaimAsync(ClaimViewModel model)
        {
            var recl = new Core.Entities.Claims.Claim();

            //Cabeçalho da Claim
            recl.CodeId = model.CodeId;     // Format : YYYYMMDD-EEE-LLLL-XXXX
            recl.DataEntradaSistemaClaim = model.DataEntradaSistemaClaim;
            recl.DataClaim = model.Claim.DataClaim;
            recl.EmailAutor = model.EmailAutor;

            if (model.Claim.NextStatusId > 0)
            {
                recl.StatusId = model.Claim.NextStatusId;
                recl.PreviousStatusId = model.Status.Id;
            }
            else
            {
                //MANTER STATUS DA RECLAMAÇÃO
                recl.StatusId = model.Status.Id;
                recl.PreviousStatusId = model.Status.Id;
            }

            // Claim a ser criada => defenir 'DataLimite', 'MaxDiasDecisão' e 'FechoClaimEmLoja'
            recl.FechoClaimEmLoja = await LojaController.GetFechoClaimEmLojaAsync(model.Claim.LojaId, _mapper, _mediator);
            recl.MaxDiasDecisão = await GrupolojaController.GetGrupolojaMaxDiasDecisãoAsync(model.Claim.LojaId, _mapper, _mediator);
            recl.DataLimite = recl.DataClaim.AddDays(recl.MaxDiasDecisão);

            //Tab Claim
            recl.MotivoClaim = model.Claim.MotivoClaim;
            recl.EmpresaId = model.Claim.EmpresaId;
            recl.GrupolojaId = model.Claim.GrupolojaId;
            recl.LojaId = model.Claim.LojaId;

            //Tab Artigo
            recl.ArtigoId = model.Artigo.ArtigoId;
            recl.Tamanho = model.Artigo.Tamanho;
            recl.TamanhoId = model.Artigo.TamanhoId;
            recl.DataCompra = model.Artigo.DataCompra;
            recl.DefeitoDoArtigo = model.Artigo.DefeitoDoArtigo;

            //Tab Pareceres
            var userId = _signInManager.UserManager.GetUserId(User);
            var currentUser = await _signInManager.UserManager.FindByIdAsync(userId);
            if (currentUser != null)
            {
                recl.ParecerAdministraçãoId = await UpdateParecerAsync(model.Pareceres.ParecerAdministração, currentUser);
                recl.ParecerRevisorId = await UpdateParecerAsync(model.Pareceres.ParecerRevisor, currentUser);
                recl.ParecerSupervisorId = await UpdateParecerAsync(model.Pareceres.ParecerSupervisor, currentUser);
                recl.ParecerGerenteLojaId = await UpdateParecerAsync(model.Pareceres.ParecerGerenteLoja, currentUser);
                recl.ParecerColaboradorId = await UpdateParecerAsync(model.Pareceres.ParecerColaborador, currentUser);
                recl.ParecerResponsavelId = await UpdateParecerAsync(model.Pareceres.ParecerResponsavel, currentUser);
            }

            //Tab Aprovação
            var nextTipoId = await StatusController.GetStatusTipoAsync(model.Claim.NextStatusId, _mapper, _mediator);      
            if (nextTipoId == (int)StatusType.Aceite || 
                nextTipoId == (int)StatusType.NãoAceite || 
                nextTipoId == (int)StatusType.FechadaEmLojaTrocaDireta)
            {
                // atualizar autor da decisão
                recl.EmailAutorDecisão = currentUser.Email;
                recl.DataDecisão = DateTime.Today;
            }
            else
            {
                recl.EmailAutorDecisão = model.Aprovação.EmailAutorDecisão;
                recl.DataDecisão = model.Aprovação.DataDecisão;
            }
            recl.DecisãoFinal = model.Aprovação.DecisãoFinal;

            if (nextTipoId == (int)StatusType.FechadaEmLojaTrocaDireta || 
                nextTipoId == (int)StatusType.FechadaEmLojaTrocaArtigo || 
                nextTipoId == (int)StatusType.FechadaEmLojaReparaçãoArtigo ||
                nextTipoId == (int)StatusType.FechadaEmLojaDevoluçãoDinheiro ||
                nextTipoId == (int)StatusType.FechadaEmLojaNotaDeCrédito ||
                nextTipoId == (int)StatusType.FechadaEmLojaRejeitada ||
                nextTipoId == (int)StatusType.FechadaEmLojaTrocaDireta)
            {
                // atualizar autor do fecho em loja
                recl.EmailAutorFechoEmLoja = currentUser.Email;
                recl.DataFecho = DateTime.Today;
            }
            else
            {
                recl.EmailAutorFechoEmLoja = model.Aprovação.EmailAutorFechoEmLoja;
                recl.DataFecho = model.Aprovação.DataFecho;
            }

            recl.Rejeitada = model.Aprovação.Rejeitada;
            recl.Trocadireta = model.Aprovação.TrocaArtigo;
            recl.Reparação = model.Aprovação.ReparaçãoArtigo;
            recl.DevoluçãoDinheiro = model.Aprovação.DevoluçãoDinheiro;
            recl.NotaCrédito = model.Aprovação.NotaCrédito;

            recl.ObservaçõesFecho = model.Aprovação.ObservaçõesFecho;

            //Tab Fotos
            recl.TotalFotos = model.TotalFotos;

            //Tab Cliente
            //Update Cliente
            var updateClienteCommand = _mapper.Map<UpdateClienteCommand>(model.Cliente);
            var resultCliente = await _mediator.Send(updateClienteCommand);
            if (resultCliente.Succeeded) _notify.Information($"{_localizer["str07"]} {resultCliente.Data} {_localizer["str08"]}");
            recl.ClienteId = model.Cliente.Id;

            return recl;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// converte uma Claim em ClaimViewModel
        /// a ClaimViewModel enviada ao client integra estruturas de dados
        /// dedicadas a cada Tab da claim. Esta função converte a reclamaão
        /// lida da db nas estruturas da ClaimViewModel.
        /// evocada no atendimento GET OnGetEdit (edit claim).
        /// </summary>
        /// <param name="recl"></param>
        /// <returns></returns>

        internal async Task<ClaimViewModel> MapperEntitieToModelClaimAsync(ClaimCachedResponse recl)
        {
            var model = new ClaimViewModel();

            // verificar user corrent
            model.CurrentRole = await GetCurrentRoleAsync();

            // Cabeçalho da Claim
            model.Id = recl.Id;
            model.CodeId = recl.CodeId;     // Format : YYYYMMDD-EEE-LLLL-XXXX
            model.DataEntradaSistemaClaim = recl.DataEntradaSistemaClaim;
            model.EmailAutor = recl.EmailAutor;
            model.Status = await StatusController.GetStatusAsync(recl.StatusId, _mapper, _mediator);
            model.Prazolimite = await PrazolimiteController.GetPrazolimiteAsync(recl.DataLimite, _mapper, _mediator);
            model.DataLimite = recl.DataLimite;
            model.MaxDiasDecisão = recl.MaxDiasDecisão;
            model.NumeroDiasParaFecho = (int)(model.DataLimite - DateTime.Now).TotalDays;
            //model.ClaimFolder = claimFolder;
            model.ClaimFolder = recl.CodeId;

            // Tab Claim
            model.Claim = new TabClaimViewModel();
            model.Claim.EditMode = true;
            model.Claim.DataClaim = recl.DataClaim;
            model.Claim.MotivoClaim = recl.MotivoClaim;
            model.Claim.EmpresaId = recl.EmpresaId;
            model.Claim.GrupolojaId = recl.GrupolojaId;
            model.Claim.LojaId = recl.LojaId;
            model.LojaNome = await LojaController.GetLojaNomeAsync(recl.LojaId, _mapper, _mediator);
            model.Claim.Empresas = await InitEmpresasByRoleAsync(model.CurrentRole, model.Claim.EmpresaId, true);
            model.Claim.Gruposlojas = await InitGrupoLojasByRoleAsync(model.CurrentRole, model.Claim.EmpresaId, model.Claim.GrupolojaId, true);
            model.Claim.Lojas = await InitLojasByRoleAsync(model.CurrentRole, model.Claim.GrupolojaId, model.Claim.LojaId, true);
            model.Logo = await EmpresaController.GetEmpresaLogoAsync(model.Claim.EmpresaId, _mapper, _mediator);
            model.Claim.NextStatusId = 0;
            model.Claim.NextStatus = await GetNextStatusOptionsAsync(model.Status.Tipo);
            //model.Claim.isNewStatusAllowed = true;
            SetTabClaimCurrentRole(ref  model);
            //claimViewModel.Claim.isNewStatusAllowed
            var roleList = GetAllowedRolesAsync(model.Status.Tipo);
            if (model.Claim.isBasic) model.Claim.isNewStatusAllowed = roleList.Contains(Roles.Basic.ToString());
            if (model.Claim.isColaborador) model.Claim.isNewStatusAllowed = roleList.Contains(Roles.Colaborador.ToString());
            if (model.Claim.isGerenteLoja) model.Claim.isNewStatusAllowed = roleList.Contains(Roles.GerenteLoja.ToString());
            if (model.Claim.isSupervisor) model.Claim.isNewStatusAllowed = roleList.Contains(Roles.Supervisor.ToString());
            if (model.Claim.isRevisor) model.Claim.isNewStatusAllowed = roleList.Contains(Roles.Revisor.ToString());
            if (model.Claim.isAdmin) model.Claim.isNewStatusAllowed = roleList.Contains(Roles.Admin.ToString());
            if (model.Claim.isSuperAdmin) model.Claim.isNewStatusAllowed = roleList.Contains(Roles.SuperAdmin.ToString());

            // Tab Aprovação
            model.Aprovação = new TabAprovaçãoViewModel();
            model.Aprovação.EnableAllEditarDecisão = false; // permite apenas a Admins e SuperAdmins, editar 'decisão final'
            model.Aprovação.DecisãoFinal = recl.DecisãoFinal;
            model.Aprovação.DataDecisão = recl.DataDecisão;
            model.Aprovação.EmailAutorDecisão = recl.EmailAutorDecisão;
            model.Aprovação.Rejeitada = recl.Rejeitada;
            model.Aprovação.TrocaArtigo = recl.Trocadireta;
            model.Aprovação.ReparaçãoArtigo = recl.Reparação;
            model.Aprovação.DevoluçãoDinheiro = recl.DevoluçãoDinheiro;
            model.Aprovação.NotaCrédito = recl.NotaCrédito;
            model.Aprovação.ObservaçõesFecho = recl.ObservaçõesFecho;
            model.Aprovação.DataFecho = recl.DataFecho;
            model.Aprovação.EmailAutorFechoEmLoja = recl.EmailAutorFechoEmLoja;
            SetTabAprovaçãoCurrentRole(ref model);

            // Tab Artigo
            model.Artigo = new TabArtigoViewModel();
            model.Artigo.ArtigoId = (int)recl.ArtigoId;
            model.Artigo.Tamanho = recl.Tamanho;
            model.Artigo.TamanhoId = recl.TamanhoId;
            model.Artigo.DataCompra = recl.DataCompra;
            model.Artigo.DefeitoDoArtigo = recl.DefeitoDoArtigo;
            var artigoViewModel = await ArtigoController.GetArtigoViewModelAsync(model.Artigo.ArtigoId, _mediator, _mapper);
            model.Artigo.EmpresaId = artigoViewModel.EmpresaId;
            model.Artigo.Empresa = artigoViewModel.Empresa;
            model.Artigo.Cor = artigoViewModel.Cor;
            model.Artigo.Pele = artigoViewModel.Pele;
            model.Artigo.Referencia = artigoViewModel.Referencia;
            model.Artigo.Tamanhos = await ArtigoController.GetSelectListTamanhos(model.Artigo.ArtigoId, model.Artigo.TamanhoId, _mediator, _mapper);
            model.Artigo.GenderId = artigoViewModel.GenderId;
            var genderViewModel = await GenderController.GetGenderViewModelAsync(model.Artigo.GenderId, _mapper, _mediator);
            model.Artigo.Gender = genderViewModel.Nome;

            // Tab Fotos
            model.TotalFotos = recl.TotalFotos;

            // Tab Pareceres
            model.Pareceres = new TabPareceresViewModel();
            model.Pareceres.ParecerResponsavel = new ParecerViewModel();
            model.ParecerResponsavelId = 0;
            if (recl.ParecerResponsavelId > 0)
            {
                var responseParecerResponsavel = await _mediator.Send(new GetParecerByIdQuery() { Id = (int)recl.ParecerResponsavelId });
                model.Pareceres.ParecerResponsavel = _mapper.Map<ParecerViewModel>(responseParecerResponsavel.Data);
                model.ParecerResponsavelId = recl.ParecerResponsavelId;
            }

            // Role do responsável
            var responsavelUser = await _signInManager.UserManager.FindByEmailAsync(model.EmailAutor);
            model.Pareceres.RoleNameResponsável = "";
            if ( await _signInManager.UserManager.IsInRoleAsync(responsavelUser, Roles.SuperAdmin.ToString()) )
            {
                model.Pareceres.RoleNameResponsável = Roles.SuperAdmin.ToString();
            }
            if (await _signInManager.UserManager.IsInRoleAsync(responsavelUser, Roles.Admin.ToString()))
            {
                model.Pareceres.RoleNameResponsável = Roles.Admin.ToString();
            }
            if (await _signInManager.UserManager.IsInRoleAsync(responsavelUser, Roles.Revisor.ToString()))
            {
                model.Pareceres.RoleNameResponsável = Roles.Revisor.ToString();
            }
            if (await _signInManager.UserManager.IsInRoleAsync(responsavelUser, Roles.Supervisor.ToString()))
            {
                model.Pareceres.RoleNameResponsável = Roles.Supervisor.ToString();
            }
            if (await _signInManager.UserManager.IsInRoleAsync(responsavelUser, Roles.GerenteLoja.ToString()))
            {
                model.Pareceres.RoleNameResponsável = Roles.GerenteLoja.ToString();
            }
            if (await _signInManager.UserManager.IsInRoleAsync(responsavelUser, Roles.Colaborador.ToString()))
            {
                model.Pareceres.RoleNameResponsável = Roles.Colaborador.ToString();
            }
            if (await _signInManager.UserManager.IsInRoleAsync(responsavelUser, Roles.Basic.ToString()))
            {
                model.Pareceres.RoleNameResponsável = Roles.Basic.ToString();
            }

            model.Pareceres.ParecerColaborador = new ParecerViewModel();
            model.ParecerColaboradorId = 0;
            if (recl.ParecerColaboradorId > 0)
            {
                var responseParecerColaborador = await _mediator.Send(new GetParecerByIdQuery() { Id = (int)recl.ParecerColaboradorId });
                model.Pareceres.ParecerColaborador = _mapper.Map<ParecerViewModel>(responseParecerColaborador.Data);
                model.ParecerColaboradorId = recl.ParecerColaboradorId;
            }

            model.Pareceres.ParecerGerenteLoja = new ParecerViewModel();
            model.ParecerGerenteLojaId = 0;
            if (recl.ParecerGerenteLojaId > 0)
            {
                var responseParecerGerenteLoja = await _mediator.Send(new GetParecerByIdQuery() { Id = (int)recl.ParecerGerenteLojaId });
                model.Pareceres.ParecerGerenteLoja = _mapper.Map<ParecerViewModel>(responseParecerGerenteLoja.Data);
                model.ParecerGerenteLojaId = recl.ParecerGerenteLojaId;
            }
                
            model.Pareceres.ParecerSupervisor = new ParecerViewModel();
            model.ParecerSupervisorId = 0;
            if (recl.ParecerSupervisorId > 0)
            {
                var responseParecerSupervisor = await _mediator.Send(new GetParecerByIdQuery() { Id = (int)recl.ParecerSupervisorId });
                model.Pareceres.ParecerSupervisor = _mapper.Map<ParecerViewModel>(responseParecerSupervisor.Data);
                model.ParecerSupervisorId = recl.ParecerSupervisorId;
            }
                
            model.Pareceres.ParecerRevisor = new ParecerViewModel();
            model.ParecerRevisorId = 0;
            if (recl.ParecerRevisorId > 0)
            {
                var responseParecerRevisor = await _mediator.Send(new GetParecerByIdQuery() { Id = (int)recl.ParecerRevisorId });
                model.Pareceres.ParecerRevisor = _mapper.Map<ParecerViewModel>(responseParecerRevisor.Data);
                model.ParecerRevisorId = recl.ParecerRevisorId;
            }
                
            model.Pareceres.ParecerAdministração = new ParecerViewModel();
            model.ParecerAdministraçãoId = 0;
            if (recl.ParecerAdministraçãoId > 0)
            {
                var responseParecerAdministração = await _mediator.Send(new GetParecerByIdQuery() { Id = (int)recl.ParecerAdministraçãoId });
                model.Pareceres.ParecerAdministração = _mapper.Map<ParecerViewModel>(responseParecerAdministração.Data);
                model.ParecerAdministraçãoId = recl.ParecerAdministraçãoId;
            }

            SetTabPareceresCurrentRole(ref model);

            // Cliente
            model.Cliente = new TabClienteViewModel();
            var responseCliente = await _mediator.Send(new GetClienteByIdQuery() { Id = recl.ClienteId });
            model.Cliente = _mapper.Map<TabClienteViewModel>(responseCliente.Data);
            model.Cliente.TipoContactoList = ClienteController.GetSelectListTiposContactos(model.Cliente.TipoContacto);

            return model;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// prepara uma estrutura ClaimViewModel para ser enviada
        /// ao client. Tem em conta o Role do user corrente.
        /// evocada no atendimento GET OnGetCreateOrEdit (criar claim).
        /// </summary>
        /// <param name="claimFolder"></param>
        /// <returns></returns>

        internal async Task<ClaimViewModel> InitNewClaimAsync(string claimFolder)
        {
            //criar modelView para retornar
            var claimViewModel = new ClaimViewModel();

            // Current Role
            claimViewModel.CurrentRole = await GetCurrentRoleAsync();

            //Cabeçalho da Claim
            claimViewModel.Logo = await EmpresaController.GetEmpresaLogoAsync(claimViewModel.CurrentRole.EmpresaId, _mapper, _mediator);
            claimViewModel.CodeId = "< Não Defenido >";
            claimViewModel.Status = new StatusViewModel();
            claimViewModel.Status.Id = 0;
            claimViewModel.Status.Texto = "#";
            claimViewModel.Status.Corfundo = "#";
            claimViewModel.Status.Cortexto = "#";
            claimViewModel.DataEntradaSistemaClaim = DateTime.Now.Date;
            claimViewModel.EmailAutor = claimViewModel.CurrentRole.Email;
            claimViewModel.ClaimFolder = claimFolder;

            //Tab Claim
            claimViewModel.Claim = new TabClaimViewModel();
            claimViewModel.Claim.EditMode = false;
            claimViewModel.Claim.DataClaim = DateTime.Now.Date;
            claimViewModel.Claim.LojaId = claimViewModel.CurrentRole.LojaId;
            claimViewModel.Claim.GrupolojaId = claimViewModel.CurrentRole.GrupolojaId;
            claimViewModel.Claim.EmpresaId = claimViewModel.CurrentRole.EmpresaId;
            claimViewModel.Claim.Empresas = await InitEmpresasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, false);
            claimViewModel.Claim.Gruposlojas = await InitGrupoLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.EmpresaId, claimViewModel.Claim.GrupolojaId, false);
            claimViewModel.Claim.Lojas = await InitLojasByRoleAsync(claimViewModel.CurrentRole, claimViewModel.Claim.GrupolojaId, claimViewModel.Claim.LojaId, false);
            claimViewModel.Claim.NextStatusId = 0;
            claimViewModel.Claim.NextStatus = await GetNextStatusOptionsAsync(0);
            SetTabClaimCurrentRole(ref claimViewModel);

            //claimViewModel.Claim.isNewStatusAllowed
            var roleList = GetAllowedRolesAsync(claimViewModel.Status.Tipo);
            if (claimViewModel.CurrentRole.IsBasic) claimViewModel.Claim.isNewStatusAllowed = roleList.Contains(Roles.Basic.ToString());
            if (claimViewModel.CurrentRole.IsColaborador) claimViewModel.Claim.isNewStatusAllowed = roleList.Contains(Roles.Colaborador.ToString());
            if (claimViewModel.CurrentRole.IsGerenteLoja) claimViewModel.Claim.isNewStatusAllowed = roleList.Contains(Roles.GerenteLoja.ToString());
            if (claimViewModel.CurrentRole.IsSupervisor) claimViewModel.Claim.isNewStatusAllowed = roleList.Contains(Roles.Supervisor.ToString());
            if (claimViewModel.CurrentRole.IsRevisor) claimViewModel.Claim.isNewStatusAllowed = roleList.Contains(Roles.Revisor.ToString());
            if (claimViewModel.CurrentRole.IsAdmin) claimViewModel.Claim.isNewStatusAllowed = roleList.Contains(Roles.Admin.ToString());
            if (claimViewModel.CurrentRole.IsSuperAdmin) claimViewModel.Claim.isNewStatusAllowed = roleList.Contains(Roles.SuperAdmin.ToString());

            // Artigo
            claimViewModel.Artigo = new TabArtigoViewModel();
            claimViewModel.Artigo.DataCompra = DateTime.Now;

            // Cliente
            claimViewModel.Cliente = new TabClienteViewModel();
            claimViewModel.Cliente.TipoContactoList = ClienteController.GetSelectListTiposContactos(TiposContactoDeCliente.GetTipoContacto(1));

            // Pareceres
            claimViewModel.Pareceres = new TabPareceresViewModel();
            claimViewModel.Pareceres.ParecerResponsavel = new ParecerViewModel();
            claimViewModel.Pareceres.ParecerColaborador = new ParecerViewModel();
            claimViewModel.Pareceres.ParecerGerenteLoja = new ParecerViewModel();
            claimViewModel.Pareceres.ParecerSupervisor = new ParecerViewModel();
            claimViewModel.Pareceres.ParecerRevisor = new ParecerViewModel();
            claimViewModel.Pareceres.ParecerAdministração = new ParecerViewModel();
            claimViewModel.Pareceres.IsResponsável = true;
            SetTabPareceresCurrentRole(ref claimViewModel);
            // Role do responsável
            claimViewModel.Pareceres.RoleNameResponsável = await GetRoleNameAsync(claimViewModel.EmailAutor);

            //Aprovação
            claimViewModel.Aprovação = new TabAprovaçãoViewModel();
            claimViewModel.Aprovação.EnableAllEditarDecisão = true; // permite a todos editar 'decisão final'
            SetTabAprovaçãoCurrentRole(ref claimViewModel);
            
            claimViewModel.Aprovação.Rejeitada = true;
            claimViewModel.Aprovação.TrocaArtigo = false;
            claimViewModel.Aprovação.DevoluçãoDinheiro = false;
            claimViewModel.Aprovação.ReparaçãoArtigo = false;
            claimViewModel.Aprovação.NotaCrédito = false;

            return claimViewModel;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica se pelo menos um dedois pareceres foram 
        /// inseridos ou atualizados. é passado role como string
        /// para compor mensagen de erro.
        /// se o nextTipo == 0 indica que o status da claim não
        /// vai mudar e nesse caso retorna true se a opinião do 
        /// parecer não for null.
        /// notification indica se o client deve ser notificado.
        /// </summary>
        /// <param name="parecer1"></param>
        /// <param name="role1"></param>
        /// <param name="parecer2"></param>
        /// <param name="role2"></param>
        /// <param name="nextStatusTipo"></param>
        /// <param name="notification"></param>
        /// <returns></returns>
        internal async Task<bool> AreMultiPareceresUpdated(ParecerViewModel parecer1, string role1, ParecerViewModel parecer2, string role2, int nextStatusTipo, bool notification)
        {
            if (String.IsNullOrEmpty(parecer1.Opinião) && String.IsNullOrEmpty(parecer2.Opinião))
            {
                if (notification)  _notify.Error(_localizer["erro14"] + role1 + _localizer["str09"]  + role2 + _localizer["str10"]);
                return false;
            }

            // não vai mudar de status
            if (nextStatusTipo == 0) return true;

            // novo parecer
            if (parecer1.Id == 0 || parecer2.Id == 0) return true;

            // ler Parecer1 da db
            var responseParecer1 = await _mediator.Send(new GetParecerByIdQuery() { Id = parecer1.Id });
            var parecer1InDb = _mapper.Map<ParecerViewModel>(responseParecer1.Data);
            if (parecer1InDb.Opinião.Equals(parecer1.Opinião))
            {
                // ler Parecer2 da db
                var responseParecer2 = await _mediator.Send(new GetParecerByIdQuery() { Id = parecer2.Id });
                var parecer2InDb = _mapper.Map<ParecerViewModel>(responseParecer2.Data);
                if (parecer2InDb.Opinião.Equals(parecer2.Opinião))
                {
                    if (notification) _notify.Error(_localizer["erro14"] + role1 + _localizer["str09"] + role2 + _localizer["str11"]);
                    return false;
                }
            }
            return true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica se um parecer foi inserido ou atualizado.
        /// é passado role como string para compor mensagen de erro.
        /// se o nextTipo == 0 indica que o status da claim não
        /// vai mudar e nesse caso retorna true se a opinião do 
        /// parecer não for null.
        /// notification indica se o client deve ser notificado.
        /// </summary>
        /// <param name="parecer"></param>
        /// <param name="role"></param>
        /// <param name="nextStatusTipo"></param>
        /// <param name="notification"></param>
        /// <returns></returns>

        internal async Task<bool> IsParecerUpdated(ParecerViewModel parecer, string role, int nextStatusTipo, bool notification)
        {
            if (String.IsNullOrEmpty(parecer.Opinião))
            {
                if (notification) _notify.Error(_localizer["erro15"] + role + _localizer["str10"]);
                return false;
            }

            // não vai mudar de status
            if (nextStatusTipo == 0) return true;

            // novo parecer
            if (parecer.Id == 0) return true;

            // ler Parecer da db
            var responseParecer = await _mediator.Send(new GetParecerByIdQuery() { Id = parecer.Id });
            var parecerInDb = _mapper.Map<ParecerViewModel>(responseParecer.Data);
            if (parecerInDb.Opinião.Equals(parecer.Opinião))
            {
                if (notification)  _notify.Error(_localizer["erro15"] + role + _localizer["str12"]);
                return false;
            }
            return true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica se é necessário atualizar na db o parecer posted na claim
        /// </summary>
        /// <param name="parecer"></param>
        /// <param name="currentuser"></param>
        /// <returns></returns>

        internal async Task<int?> UpdateParecerAsync(ParecerViewModel parecer, ApplicationUser currentuser)
        {
            if (string.IsNullOrEmpty(parecer.Opinião)) return null;

            // inserir ou atualizar db
            if (parecer.Id == 0)
            {
                // insere Parecer
                var createParecerAdCommand = _mapper.Map<CreateParecerCommand>(parecer);
                createParecerAdCommand.Email = currentuser.Email;
                createParecerAdCommand.Data = DateTime.Today;
                var newParecerResult = await _mediator.Send(createParecerAdCommand);
                if (newParecerResult.Succeeded) _notify.Success($"{_localizer["str05"]} {newParecerResult.Data} {_localizer["str13"]}");
                return newParecerResult.Data;
            }

            // parecer já existe
            var responseParecer = await _mediator.Send(new GetParecerByIdQuery() { Id = parecer.Id });
            var oldParecer = _mapper.Map<ParecerViewModel>(responseParecer.Data);
            if (parecer.Opinião.Equals(oldParecer.Opinião)) return parecer.Id;

            // update Parecer
            var updateParecerCommand = _mapper.Map<UpdateParecerCommand>(parecer);
            updateParecerCommand.Email = currentuser.Email;
            updateParecerCommand.Data = DateTime.Today;
            var result = await _mediator.Send(updateParecerCommand);
            if (result.Succeeded) _notify.Information($"{_localizer["str05"]} {result.Data} {_localizer["str08"]}");
            return parecer.Id;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para criar uma SelectList de Empresas de acordo com o Role
        /// do user corrente e da claim estar a ser criada ou editada
        /// </summary>
        /// <param name="cRole"></param>
        /// <param name="selectedId"></param>
        /// <param name="editar"></param>
        /// <returns></returns>

        public async Task<SelectList> InitEmpresasByRoleAsync(CurrentRole cRole, int selectedId, bool editar)
        {
            if (editar) return await EmpresaController.GetSelectListOneEmpresaAsync(selectedId, _mapper, _mediator);
            if (cRole.IsSuperAdmin || cRole.IsAdmin || cRole.IsRevisor)
            {
                //construir SelectList para qualquer Empresa
                return await EmpresaController.GetSelectListAllEmpresasAsync(selectedId, _mapper, _mediator);
            }
            return await EmpresaController.GetSelectListOneEmpresaAsync(selectedId, _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para criar uma SelectList de GrupoLojas de acordo com o Role
        /// do user corrente e em função da Empresa selecionada
        /// </summary>
        /// <param name="cRole"></param>
        /// <param name="empresaId"></param>
        /// <param name="selectedId"></param>
        /// <param name="editar"></param>
        /// <returns></returns>

        public async Task<SelectList> InitGrupoLojasByRoleAsync(CurrentRole cRole, int empresaId, int selectedId, bool editar)
        {
            if (editar) return await GrupolojaController.GetSelectListOneGrupoLojaAsync(selectedId, _mapper, _mediator);
            if (cRole.IsSuperAdmin || cRole.IsAdmin || cRole.IsRevisor)
            {
                //construir SelectList para qualquer GrupoLoja de empresaId
                return await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(empresaId, selectedId, _mapper, _mediator);
            }
            return await GrupolojaController.GetSelectListOneGrupoLojaAsync(selectedId, _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para criar uma SelectList de Lojas de acordo com o Role
        /// do user corrente e em função do GrupoLoja selecionado
        /// </summary>
        /// <param name="cRole"></param>
        /// <param name="grupolojaId"></param>
        /// <param name="selectedId"></param>
        /// <param name="editar"></param>
        /// <returns></returns>

        public async Task<SelectList> InitLojasByRoleAsync(CurrentRole cRole, int grupolojaId, int selectedId, bool editar)
        {
            if (editar) return await LojaController.GetSelectListOneLojaAsync(selectedId, _mapper, _mediator);
            if (cRole.IsSuperAdmin || cRole.IsAdmin || cRole.IsRevisor || cRole.IsSupervisor)
            {
                //construir SelectList para qualquer Loja de grupolojaId
                return await LojaController.GetSelectListLojasFromGrupolojaAsync(grupolojaId, selectedId, _mapper, _mediator);
            }
            //cRole.IsGerenteLoja || cRole.IsColaborador || cRole.IsBasic
            //construir SelectList para Loja em selectedId
            return await LojaController.GetSelectListOneLojaAsync(selectedId, _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// escreve no PDF 'doc', utilizando 'gfx', o header da cliam.
        /// vOff é o offset vertical para começar a escrever
        /// </summary>
        /// <param name="claimViewModel"></param>
        /// <param name="doc"></param>
        /// <param name="gfx"></param>
        /// <param name="vOff"></param>
        /// <returns>PdfDocument</returns>

        internal PdfDocument CreatePdfHeader(ClaimViewModel claimViewModel, PdfDocument doc, XGraphics gfx, double vOff)
        {
            if (claimViewModel == null) return doc;

            // draw logo
            MemoryStream stream = new MemoryStream(claimViewModel.Logo);
            XImage image = XImage.FromStream(stream);
            if (image != null) gfx.DrawImage(image, 20, vOff + 20, 200, 72);

            // draw claim header
            PdfPage page = doc.Pages[0];

            // Create a font
            XFont fontTitle = new XFont("Verdana", 16, XFontStyleEx.Bold);
            XFont fontHeader = new XFont("Verdana", 10, XFontStyleEx.Regular);
            XFont fontStatus = new XFont("Verdana", 12, XFontStyleEx.Bold);

            // create a pen
            XPen XPenGreenYellow = new XPen(XColors.GreenYellow, 2);

            // Draw CodeId
            if (!string.IsNullOrEmpty(claimViewModel.CodeId)) gfx.DrawString(claimViewModel.CodeId, fontTitle, XBrushes.Black, new XRect(260, vOff + 20, 200, 72), XStringFormats.TopLeft);

            // Draw Data da Claim
            var ldr = _localizer["Data da Reclamação:"];
            var vdr = claimViewModel.Claim.DataClaim.ToString(_localizer["dd/MM/yyyy"]);
            gfx.DrawString(ldr, fontHeader, XBrushes.Black, new XRect(260, vOff + 45, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vdr, fontHeader, XBrushes.Black, new XRect(400, vOff + 45, 200, 72), XStringFormats.TopLeft);

            // Draw Data Limite
            var ldl = _localizer["Data Limite:"];
            var vdl = claimViewModel.DataLimite.ToString(_localizer["dd/MM/yyyy"]);
            gfx.DrawString(ldl, fontHeader, XBrushes.Black, new XRect(260, vOff + 55, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vdl, fontHeader, XBrushes.Red, new XRect(400, vOff + 55, 200, 72), XStringFormats.TopLeft);

            // Draw Email do responsável
            var lec = _localizer["Colaborador:"];
            var vec = string.IsNullOrEmpty(claimViewModel.EmailAutor) ?
                      string.Empty : claimViewModel.EmailAutor;
            gfx.DrawString(lec, fontHeader, XBrushes.Black, new XRect(260, vOff + 65, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vec, fontHeader, XBrushes.Black, new XRect(400, vOff + 65, 200, 72), XStringFormats.TopLeft);

            // Draw Loja
            var ll = _localizer["Loja:"];
            var vl = string.IsNullOrEmpty(claimViewModel.LojaNome) ?
                     string.Empty : claimViewModel.LojaNome;
            gfx.DrawString(ll, fontHeader, XBrushes.Black, new XRect(260, vOff + 75, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vl, fontHeader, XBrushes.Black, new XRect(400, vOff + 75, 200, 72), XStringFormats.TopLeft);

            // Draw Status
            var ls = _localizer["Status:"];
            var vs = string.IsNullOrEmpty(claimViewModel.Status.Texto) ?
                     string.Empty : claimViewModel.Status.Texto;
            gfx.DrawString(ls + " " + vs, fontStatus, XBrushes.Black, new XRect(20, vOff + 115, 200, 72), XStringFormats.TopLeft);

            // draw horizontal line
            gfx.DrawLine(XPenGreenYellow, 20, vOff + 145, 570, vOff + 145);

            return doc;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// escreve no PDF 'doc', utilizando 'gfx', a seção Cliente da cliam.
        /// vOff é o offset vertical para começar a escrever
        /// </summary>
        /// <param name="claimViewModel"></param>
        /// <param name="doc"></param>
        /// <param name="gfx"></param>
        /// <param name="vOff"></param>
        /// <returns></returns>

        internal PdfDocument WritePdfClientSection(ClaimViewModel claimViewModel, PdfDocument doc, XGraphics gfx, double vOff)
        {
            if (claimViewModel == null) return doc;

            // get page
            PdfPage page = doc.Pages[0];

            // Create a font
            XFont fontNormal = new XFont("Verdana", 10, XFontStyleEx.Regular);
            XFont fontTitle = new XFont("Verdana", 10, XFontStyleEx.Bold);
            XFont fontUser = new XFont("Times New Roman", 10, XFontStyleEx.Regular);

            // create a pen
            //XPen XPenBlack = new XPen(XColors.Black, 1);
            XPen XPenBlack = new XPen(XColor.FromArgb(70, XColors.Black));
            XPen XPenGreenYellow = new XPen(XColors.GreenYellow, 2);

            // Draw Title
            var cli = _localizer["Opinião do cliente:"];
            gfx.DrawString(cli, fontTitle, XBrushes.Black, new XRect(20, vOff + 20, 200, 72), XStringFormats.TopLeft);

            // Draw Opinião do cliente
            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect1 = new XRect(20, vOff + 35, 230, 60);
            XRect rect2 = new XRect(22, vOff + 35, 230, 60);
            gfx.DrawRectangle(XPenBlack, rect1);
            var mc = string.IsNullOrEmpty(claimViewModel.Claim.MotivoClaim) ?
                     string.Empty : claimViewModel.Claim.MotivoClaim;
            tf.DrawString(mc, fontUser, XBrushes.Black, rect2, XStringFormats.TopLeft);

            // Draw Nome do cliente
            var lnc = _localizer["Nome:"];
            var vnc = string.IsNullOrEmpty(claimViewModel.Cliente.Nome) ?
                      string.Empty : claimViewModel.Cliente.Nome;
            gfx.DrawString(lnc, fontNormal, XBrushes.Black, new XRect(300, vOff + 35, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vnc, fontNormal, XBrushes.Black, new XRect(400, vOff + 35, 200, 72), XStringFormats.TopLeft);

            // Draw Telefone do cliente
            var ltc = _localizer["Telefone:"];
            var vtc = string.IsNullOrEmpty(claimViewModel.Cliente.Telefone) ?
                      string.Empty : claimViewModel.Cliente.Telefone;
            gfx.DrawString(ltc, fontNormal, XBrushes.Black, new XRect(300, vOff + 45, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vtc, fontNormal, XBrushes.Black, new XRect(400, vOff + 45, 200, 72), XStringFormats.TopLeft);

            // Draw Email do cliente
            var lec = _localizer["Email:"];
            var vec = string.IsNullOrEmpty(claimViewModel.Cliente.Email) ? 
                      string.Empty : claimViewModel.Cliente.Email;
            gfx.DrawString(lec, fontNormal, XBrushes.Black, new XRect(300, vOff + 55, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vec, fontNormal, XBrushes.Black, new XRect(400, vOff + 55, 200, 72), XStringFormats.TopLeft);

            // draw horizontal line
            gfx.DrawLine(XPenGreenYellow, 20, vOff + 115, 570, vOff + 115);

            return doc;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// escreve no PDF 'doc', utilizando 'gfx', a seção Artigo da cliam.
        /// vOff é o offset vertical para começar a escrever
        /// </summary>
        /// <param name="claimViewModel"></param>
        /// <param name="doc"></param>
        /// <param name="gfx"></param>
        /// <param name="vOff"></param>
        /// <returns></returns>
        
        internal PdfDocument WritePdfArtigoSection(ClaimViewModel claimViewModel, PdfDocument doc, XGraphics gfx, double vOff)
        {
            if (claimViewModel == null) return doc;

            // get page
            PdfPage page = doc.Pages[0];

            // Create a font
            XFont fontNormal = new XFont("Verdana", 10, XFontStyleEx.Regular);
            XFont fontTitle = new XFont("Verdana", 10, XFontStyleEx.Bold);
            XFont fontUser = new XFont("Times New Roman", 10, XFontStyleEx.Regular);

            // create a pen
            //XPen XPenBlack = new XPen(XColors.Black, 1);
            XPen XPenBlack = new XPen(XColor.FromArgb(70, XColors.Black));
            XPen XPenGreenYellow = new XPen(XColors.GreenYellow, 2);

            // Draw Title
            var art = _localizer["Defeito do artigo:"];
            gfx.DrawString(art, fontTitle, XBrushes.Black, new XRect(20, vOff + 20, 200, 72), XStringFormats.TopLeft);

            // Draw defeito do artigo
            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect1 = new XRect(20, vOff + 35, 230, 60);
            XRect rect2 = new XRect(22, vOff + 35, 230, 60);
            gfx.DrawRectangle(XPenBlack, rect1);
            var da = string.IsNullOrEmpty(claimViewModel.Artigo.DefeitoDoArtigo) ?
                      string.Empty : claimViewModel.Artigo.DefeitoDoArtigo;
            tf.DrawString(da, fontUser, XBrushes.Black, rect2, XStringFormats.TopLeft);

            // Draw Referência do artigo
            var lra = _localizer["Referência:"];
            var vra = string.IsNullOrEmpty(claimViewModel.Artigo.Referencia) ?
                      string.Empty : claimViewModel.Artigo.Referencia;
            gfx.DrawString(lra, fontNormal, XBrushes.Black, new XRect(300, vOff + 35, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vra, fontNormal, XBrushes.Black, new XRect(400, vOff + 35, 200, 72), XStringFormats.TopLeft);

            // Draw género do artigo
            var lga = _localizer["Género:"];
            var vga = string.IsNullOrEmpty(claimViewModel.Artigo.Gender) ?
                      string.Empty : claimViewModel.Artigo.Gender;
            gfx.DrawString(lga, fontNormal, XBrushes.Black, new XRect(300, vOff + 45, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vga, fontNormal, XBrushes.Black, new XRect(400, vOff + 45, 200, 72), XStringFormats.TopLeft);

            // Draw cor do artigo
            var lca = _localizer["Cor:"];
            var vca = string.IsNullOrEmpty(claimViewModel.Artigo.Cor) ? 
                      string.Empty : claimViewModel.Artigo.Cor;
            gfx.DrawString(lca, fontNormal, XBrushes.Black, new XRect(300, vOff + 55, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vca, fontNormal, XBrushes.Black, new XRect(400, vOff + 55, 200, 72), XStringFormats.TopLeft);

            // Draw pele do artigo
            var lpa = _localizer["Pele:"];
            var vpa = string.IsNullOrEmpty(claimViewModel.Artigo.Pele) ?
                      string.Empty : claimViewModel.Artigo.Pele;
            gfx.DrawString(lpa, fontNormal, XBrushes.Black, new XRect(300, vOff + 65, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vpa, fontNormal, XBrushes.Black, new XRect(400, vOff + 65, 200, 72), XStringFormats.TopLeft);

            // Draw tamanho do artigo
            var lta = _localizer["Tamanho:"];
            var vta = string.IsNullOrEmpty(claimViewModel.Artigo.Tamanho) ?
                      string.Empty : claimViewModel.Artigo.Tamanho;
            gfx.DrawString(lta, fontNormal, XBrushes.Black, new XRect(300, vOff + 75, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vta, fontNormal, XBrushes.Black, new XRect(400, vOff + 75, 200, 72), XStringFormats.TopLeft);

            // Draw Data da compra
            var ldc = _localizer["Data da compra:"];
            var vdc = claimViewModel.Artigo.DataCompra.ToString(_localizer["dd/MM/yyyy"]);
            gfx.DrawString(ldc, fontNormal, XBrushes.Black, new XRect(300, vOff + 85, 200, 72), XStringFormats.TopLeft);
            gfx.DrawString(vdc, fontNormal, XBrushes.Black, new XRect(400, vOff + 85, 200, 72), XStringFormats.TopLeft);

            // draw horizontal line
            gfx.DrawLine(XPenGreenYellow, 20, vOff + 115, 570, vOff + 115);

            return doc;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// escreve no PDF 'doc', utilizando 'gfx', a seção Pareceres da cliam.
        /// vOff é o offset vertical para começar a escrever
        /// </summary>
        /// <param name="claimViewModel"></param>
        /// <param name="doc"></param>
        /// <param name="gfx"></param>
        /// <param name="vOff"></param>
        /// <returns></returns>

        internal PdfDocument WritePdfParecerSection(ClaimViewModel claimViewModel, PdfDocument doc, XGraphics gfx, double vOff)
        {
            if (claimViewModel == null) return doc;

            // get page
            PdfPage page = doc.Pages[0];

            // Create a font
            XFont fontNormal = new XFont("Verdana", 8, XFontStyleEx.Regular);
            XFont fontTitle = new XFont("Verdana", 10, XFontStyleEx.Bold);
            XFont fontUser = new XFont("Times New Roman", 10, XFontStyleEx.Regular);

            // create a pen
            //XPen XPenBlack = new XPen(XColors.Black, 1);
            XPen XPenBlack = new XPen(XColor.FromArgb(70, XColors.Black));
            XPen XPenGreenYellow = new XPen(XColors.GreenYellow, 2);

            // Draw Title
            var col = _localizer["Parecer do responsável:"];
            gfx.DrawString(col, fontTitle, XBrushes.Black, new XRect(20, vOff + 20, 550, 72), XStringFormats.TopLeft);
            var dcol = claimViewModel.Pareceres.ParecerResponsavel.Email + " - " + claimViewModel.Pareceres.ParecerResponsavel.Data.ToString(_localizer["dd/MM/yyyy"]);
            gfx.DrawString(dcol, fontNormal, XBrushes.Black, new XRect(20, vOff + 20, 550, 72), XStringFormats.TopRight);

            // Draw parecer do colaborador
            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect1 = new XRect(20, vOff + 35, 550, 60);
            XRect rect2 = new XRect(22, vOff + 35, 550, 60);
            gfx.DrawRectangle(XPenBlack, rect1);
            var vpc = string.IsNullOrEmpty(claimViewModel.Pareceres.ParecerResponsavel.Opinião) ?
                      string.Empty : claimViewModel.Pareceres.ParecerResponsavel.Opinião;
            tf.DrawString(vpc, fontUser, XBrushes.Black, rect2, XStringFormats.TopLeft);

            // draw horizontal line
            gfx.DrawLine(XPenGreenYellow, 20, vOff + 115, 570, vOff + 115);

            return doc;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// escreve no PDF 'doc', utilizando 'gfx', a seção Aprovação da cliam.
        /// vOff é o offset vertical para começar a escrever
        /// </summary>
        /// <param name="claimViewModel"></param>
        /// <param name="doc"></param>
        /// <param name="gfx"></param>
        /// <param name="vOff"></param>
        /// <returns></returns>
        
        internal PdfDocument WritePdfAprovaçãoSection(ClaimViewModel claimViewModel, PdfDocument doc, XGraphics gfx, double vOff)
        {
            if (claimViewModel == null) return doc;

            // get page
            PdfPage page = doc.Pages[0];

            // Create a font
            XFont fontNormal = new XFont("Verdana", 8, XFontStyleEx.Regular);
            XFont fontTitle = new XFont("Verdana", 10, XFontStyleEx.Bold);
            XFont fontUser = new XFont("Times New Roman", 10, XFontStyleEx.Regular);

            // create a pen
            //XPen XPenBlack = new XPen(XColors.Black, 1);
            XPen XPenBlack = new XPen(XColor.FromArgb(70, XColors.Black));
            XPen XPenGreenYellow = new XPen(XColors.GreenYellow, 2);

            // Draw Title
            var apr = _localizer["Decisão final:"];
            gfx.DrawString(apr, fontTitle, XBrushes.Black, new XRect(20, vOff + 20, 550, 72), XStringFormats.TopLeft);
            var aprg = string.IsNullOrEmpty(claimViewModel.Aprovação.EmailAutorDecisão) ||
                       string.IsNullOrEmpty(claimViewModel.Aprovação.DataDecisão.ToString(_localizer["dd/MM/yyyy"])) ?
                       string.Empty : claimViewModel.Aprovação.EmailAutorDecisão + " - " + claimViewModel.Aprovação.DataDecisão.ToString(_localizer["dd/MM/yyyy"]);
            gfx.DrawString(aprg, fontNormal, XBrushes.Black, new XRect(20, vOff + 20, 550, 72), XStringFormats.TopRight);

            // Draw decisão final
            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect1 = new XRect(20, vOff + 35, 550, 60);
            XRect rect2 = new XRect(22, vOff + 35, 550, 60);
            gfx.DrawRectangle(XPenBlack, rect1);
            var df = string.IsNullOrEmpty(claimViewModel.Aprovação.DecisãoFinal) ? 
                     string.Empty : claimViewModel.Aprovação.DecisãoFinal;
            tf.DrawString(df, fontUser, XBrushes.Black, rect2, XStringFormats.TopLeft);

            // draw horizontal line
            gfx.DrawLine(XPenGreenYellow, 20, vOff + 115, 570, vOff + 115);

            return doc;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// escreve no PDF 'doc', utilizando 'gfx', a seção Resolução com o cliente.
        /// vOff é o offset vertical para começar a escrever
        /// </summary>
        /// <param name="claimViewModel"></param>
        /// <param name="doc"></param>
        /// <param name="gfx"></param>
        /// <param name="vOff"></param>
        /// <returns></returns>

        internal PdfDocument WritePdfResoluçãoSection(ClaimViewModel claimViewModel, PdfDocument doc, XGraphics gfx, double vOff)
        {
            if (claimViewModel == null) return doc;

            // get page
            PdfPage page = doc.Pages[0];

            // Create a font
            XFont fontNormal = new XFont("Verdana", 8, XFontStyleEx.Regular);
            XFont fontTitle = new XFont("Verdana", 10, XFontStyleEx.Bold);
            XFont fontUser = new XFont("Times New Roman", 10, XFontStyleEx.Regular);

            // create a pen
            //XPen XPenBlack = new XPen(XColors.Black, 1);
            XPen XPenBlack = new XPen(XColor.FromArgb(70, XColors.Black));
            XPen XPenGreenYellow = new XPen(XColors.GreenYellow, 2);

            // Draw Title
            var apr = _localizer["Resolução com o cliente:"];
            gfx.DrawString(apr, fontTitle, XBrushes.Black, new XRect(20, vOff + 20, 550, 72), XStringFormats.TopLeft);
            var aprg = string.IsNullOrEmpty(claimViewModel.Aprovação.EmailAutorFechoEmLoja) ||
                       string.IsNullOrEmpty(claimViewModel.Aprovação.DataFecho.ToString(_localizer["dd/MM/yyyy"])) ?
                       string.Empty : claimViewModel.Aprovação.EmailAutorFechoEmLoja + " - " + claimViewModel.Aprovação.DataFecho.ToString(_localizer["dd/MM/yyyy"]);
            gfx.DrawString(aprg, fontNormal, XBrushes.Black, new XRect(20, vOff + 20, 550, 72), XStringFormats.TopRight);

            // Draw decisão final
            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect1 = new XRect(20, vOff + 35, 550, 60);
            XRect rect2 = new XRect(22, vOff + 35, 550, 60);
            gfx.DrawRectangle(XPenBlack, rect1);
            var df = string.IsNullOrEmpty(claimViewModel.Aprovação.ObservaçõesFecho) ? 
                     string.Empty : claimViewModel.Aprovação.ObservaçõesFecho;
            tf.DrawString(df, fontUser, XBrushes.Black, rect2, XStringFormats.TopLeft);

            // draw horizontal line
            gfx.DrawLine(XPenGreenYellow, 20, vOff + 115, 570, vOff + 115);

            return doc;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// mecanismo de troca de status por lookup table.
        /// dado o tipo de status corrente é devolvido um array com os
        /// tipos de status possíveis para o status seguinte.
        /// NOTA: podem existir vários status para cada tipo.
        /// </summary>
        /// <param name="currentTypeId"></param>
        /// <returns></returns>

        internal Task<SelectList> GetNextStatusOptionsAsync(int currentTypeId)
        {
            int[][] lookupTable =
            {
                new int[] { 2, 11 },                        //0
                new int[] { 0, 2 },                         //1 "Pendente em Loja"
                new int[] { 0, 1, 3, 5, 7, 8, 9, 16 },      //2 "Aguarda Validação"
                new int[] { 0, 1, 4, 5, 6, 7, 8, 9, 16 },   //3 "Aguarda Decisão"
                new int[] { 0, 3 },                         //4 "Aguarda Opinião Gerente de Loja"
                new int[] { 0, 3 },                         //5 "Aguarda Opinião Supervisor"
                new int[] { 0, 3 },                         //6 "Aguarda Opinião Revisor"
                new int[] { 0, 16 },                        //7 "Aguarda Opinião Fornecedor"
                new int[] { 0, 12, 13, 14, 15 },            //8 "Aceite"
                new int[] { 0, 10, 17 },                    //9 "Não Aceite"
                new int[] { 0 },                            //10 "Fechada em Loja Rejeitada"
                new int[] { 0 },                            //11 "Fechada em Loja Troca Direta"
                new int[] { 0 },                            //12 "Fechada em Loja Reparação Artigo"
                new int[] { 0 },                            //13 "Fechada em Loja Troca Artigo"
                new int[] { 0 },                            //14 "Fechada em Loja Devolução Dinheiro"
                new int[] { 0 },                            //15 "Fechada em Loja Nota de Crédito"
                new int[] { 0, 2, 3 },                      //16 "Aguarda Resposta Fornecedor"
                new int[] { 0, 18 },                        //17 "Aguarda Relatório"
                new int[] { 0, 10 }                         //18 "Relatório ´Disponível"
            };
            return StatusController.GetSelectListStatusTypeAsync(lookupTable[currentTypeId], _mapper, _mediator);
        }


        //---------------------------------------------------------------------------------------------------


        internal List<string> GetAllowedRolesAsync(int currentTypeId)
        {
            string[][] lookupTable =
            {
                /*0 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Admin.ToString(), Roles.Revisor.ToString(), Roles.Supervisor.ToString(), Roles.GerenteLoja.ToString(), Roles. Colaborador.ToString() },
                /*1 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Supervisor.ToString(), Roles.GerenteLoja.ToString(), Roles. Colaborador.ToString() },
                /*2 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Admin.ToString(), Roles.Revisor.ToString() },
                /*3 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Admin.ToString() },
                /*4 */ new string[] { Roles.SuperAdmin.ToString(), Roles.GerenteLoja.ToString() },
                /*5 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Supervisor.ToString() },
                /*6 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Revisor.ToString() },
                /*7 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Revisor.ToString() },
                /*8 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Supervisor.ToString(), Roles.GerenteLoja.ToString(), Roles.Colaborador.ToString() },
                /*9 */ new string[] { Roles.SuperAdmin.ToString(), Roles.Supervisor.ToString(), Roles.GerenteLoja.ToString(), Roles.Colaborador.ToString() },
                /*10*/ new string[] { Roles.SuperAdmin.ToString() },
                /*11*/ new string[] { Roles.SuperAdmin.ToString() },
                /*12*/ new string[] { Roles.SuperAdmin.ToString() },
                /*13*/ new string[] { Roles.SuperAdmin.ToString() },
                /*14*/ new string[] { Roles.SuperAdmin.ToString() },
                /*15*/ new string[] { Roles.SuperAdmin.ToString() },
                /*16*/ new string[] { Roles.SuperAdmin.ToString(), Roles.Revisor.ToString() },
                /*17*/ new string[] { Roles.SuperAdmin.ToString(), Roles.Revisor.ToString() },
                /*18*/ new string[] { Roles.SuperAdmin.ToString(), Roles.GerenteLoja.ToString(), Roles.Colaborador.ToString() }
            };

            if (currentTypeId < lookupTable.Length) return lookupTable[currentTypeId].ToList();
            return null;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// função para gerar o CodeId identificador da claim
        /// o código é gerado a partir da data, loja, empresa e Id da claim.
        /// o Id obriga a que a claim seja primeiro criada na db e
        /// só depois o código pode ser gerado.
        /// Depois do CodeId ser gerado é necessário update da claim na db
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="EmpresaId"></param>
        /// <param name="LojaId"></param>
        /// <returns></returns>

        internal async Task<string> CodeGenerationAsync(int Id, DateTime date, int EmpresaId, int LojaId)
        {
            // Format : YYYYMMDD-EEE-LLLL-XXXX
            var code = date.Date.ToString("yyyyMMdd");

            var lojaResponse = await _mediator.Send(new GetLojaByIdQuery() { Id = LojaId });
            var empresaResponse = await _mediator.Send(new GetEmpresaByIdQuery() { Id = EmpresaId });
            if (empresaResponse.Succeeded && lojaResponse.Succeeded)
            {
                code = code + "-" + empresaResponse.Data.NomeCurto + "-" + lojaResponse.Data.NomeCurto;
            }
            code = code + "-" + Id.ToString("000000");
            return code;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// a partir do email do user devolve uma string com o role desse user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>

        internal async Task<string> GetRoleNameAsync(string userEmail)
        {
            var responselUser = await _signInManager.UserManager.FindByEmailAsync(userEmail);

            if (await _signInManager.UserManager.IsInRoleAsync(responselUser, Roles.SuperAdmin.ToString())) return Roles.SuperAdmin.ToString();
            if (await _signInManager.UserManager.IsInRoleAsync(responselUser, Roles.Admin.ToString())) return Roles.Admin.ToString();
            if (await _signInManager.UserManager.IsInRoleAsync(responselUser, Roles.Revisor.ToString()))return Roles.Revisor.ToString();
            if (await _signInManager.UserManager.IsInRoleAsync(responselUser, Roles.Supervisor.ToString())) return Roles.Supervisor.ToString();
            if (await _signInManager.UserManager.IsInRoleAsync(responselUser, Roles.GerenteLoja.ToString())) return Roles.GerenteLoja.ToString();
            if (await _signInManager.UserManager.IsInRoleAsync(responselUser, Roles.Colaborador.ToString())) return Roles.Colaborador.ToString();
            if (await _signInManager.UserManager.IsInRoleAsync(responselUser, Roles.Basic.ToString())) return Roles.Basic.ToString();

            return "";
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria um CurrentRole para o utilizador corrente "User"
        /// </summary>
        /// <param></param>
        /// <returns></returns>

        internal async Task<CurrentRole> GetCurrentRoleAsync()
        {

            // CurrentRole
            var cr = new CurrentRole();

            try
            {
                var userId = _signInManager.UserManager.GetUserId(User);
                var currentUser = await _signInManager.UserManager.FindByIdAsync(userId);

                cr.Email = currentUser.Email;
                cr.LojaId = currentUser.LojaId == null ? 0 : (int)currentUser.LojaId;
                cr.GrupolojaId = currentUser.GrupolojaId == null ? 0 : (int)currentUser.GrupolojaId;
                cr.EmpresaId = currentUser.EmpresaId == null ? 0 : (int)currentUser.EmpresaId;
                
                cr.IsSuperAdmin = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.SuperAdmin.ToString());
                cr.IsAdmin = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Admin.ToString());
                cr.IsSupervisor = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Supervisor.ToString());
                cr.IsRevisor = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Revisor.ToString());
                cr.IsGerenteLoja = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.GerenteLoja.ToString());
                cr.IsColaborador = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Colaborador.ToString());
                cr.IsBasic = await _signInManager.UserManager.IsInRoleAsync(currentUser, Roles.Basic.ToString());

                cr.RoleName = string.Empty;
                if (cr.IsSuperAdmin) cr.RoleName = Roles.SuperAdmin.ToString();
                if (cr.IsAdmin) cr.RoleName = Roles.Admin.ToString();
                if (cr.IsSupervisor) cr.RoleName = Roles.Supervisor.ToString();
                if (cr.IsRevisor) cr.RoleName = Roles.Revisor.ToString();
                if (cr.IsGerenteLoja) cr.RoleName = Roles.GerenteLoja.ToString();
                if (cr.IsColaborador) cr.RoleName = Roles.Colaborador.ToString();
                if (cr.IsBasic) cr.RoleName = Roles.Basic.ToString();

                return cr;
            }
            catch(Exception ex)
            {
                var str = ex.Message;
                return cr;
            };
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza a TabClaim com o Current role do User corrente
        /// </summary>
        /// <param name="clientClaimViewModel"></param>
        
        internal void SetTabClaimCurrentRole(ref ClaimViewModel clientClaimViewModel)
        {
            // Claim
            clientClaimViewModel.Claim.isBasic = clientClaimViewModel.CurrentRole.IsBasic;
            clientClaimViewModel.Claim.isColaborador = clientClaimViewModel.CurrentRole.IsColaborador;
            clientClaimViewModel.Claim.isGerenteLoja = clientClaimViewModel.CurrentRole.IsGerenteLoja;
            clientClaimViewModel.Claim.isSupervisor = clientClaimViewModel.CurrentRole.IsSupervisor;
            clientClaimViewModel.Claim.isRevisor = clientClaimViewModel.CurrentRole.IsRevisor;
            clientClaimViewModel.Claim.isAdmin = clientClaimViewModel.CurrentRole.IsAdmin;
            clientClaimViewModel.Claim.isSuperAdmin = clientClaimViewModel.CurrentRole.IsSuperAdmin;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza a TabAprovação com o Current role do User corrente
        /// </summary>
        /// <param name="clientClaimViewModel"></param>

        internal void SetTabAprovaçãoCurrentRole(ref ClaimViewModel clientClaimViewModel)
        {
            // Aprovação
            clientClaimViewModel.Aprovação.EmailCurrentUser = clientClaimViewModel.CurrentRole.Email;
            clientClaimViewModel.Aprovação.IsBasic = clientClaimViewModel.CurrentRole.IsBasic;
            clientClaimViewModel.Aprovação.IsColaborador = clientClaimViewModel.CurrentRole.IsColaborador;
            clientClaimViewModel.Aprovação.IsGerenteLoja = clientClaimViewModel.CurrentRole.IsGerenteLoja;
            clientClaimViewModel.Aprovação.IsSupervisor = clientClaimViewModel.CurrentRole.IsSupervisor;
            clientClaimViewModel.Aprovação.IsRevisor = clientClaimViewModel.CurrentRole.IsRevisor;
            clientClaimViewModel.Aprovação.IsAdmin = clientClaimViewModel.CurrentRole.IsAdmin;
            clientClaimViewModel.Aprovação.IsSuperAdmin = clientClaimViewModel.CurrentRole.IsSuperAdmin;
            //clientClaimViewModel.Aprovação.EnableAllEditarDecisão = true;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza a TabPareceres com o Current role do User corrente
        /// </summary>
        /// <param name="clientClaimViewModel"></param>

        internal void SetTabPareceresCurrentRole(ref ClaimViewModel clientClaimViewModel)
        {
            clientClaimViewModel.Pareceres.IsBasic = clientClaimViewModel.CurrentRole.IsBasic;
            clientClaimViewModel.Pareceres.IsColaborador = clientClaimViewModel.CurrentRole.IsColaborador;
            clientClaimViewModel.Pareceres.IsGerenteLoja = clientClaimViewModel.CurrentRole.IsGerenteLoja;
            clientClaimViewModel.Pareceres.IsSupervisor = clientClaimViewModel.CurrentRole.IsSupervisor;
            clientClaimViewModel.Pareceres.IsRevisor = clientClaimViewModel.CurrentRole.IsRevisor;
            clientClaimViewModel.Pareceres.IsAdmin = clientClaimViewModel.CurrentRole.IsAdmin;
            clientClaimViewModel.Pareceres.IsSuperAdmin = clientClaimViewModel.CurrentRole.IsSuperAdmin;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// atualiza o claimviewmodel com os ids dos pareceres da TabPareceres
        /// </summary>
        /// <param name="clientClaimViewModel"></param>

        internal void SetTabPareceres(ref ClaimViewModel clientClaimViewModel)
        {
            clientClaimViewModel.ParecerResponsavelId = clientClaimViewModel.Pareceres.ParecerResponsavel.Id;
            clientClaimViewModel.ParecerColaboradorId = clientClaimViewModel.Pareceres.ParecerColaborador.Id;
            clientClaimViewModel.ParecerGerenteLojaId = clientClaimViewModel.Pareceres.ParecerGerenteLoja.Id;
            clientClaimViewModel.ParecerSupervisorId = clientClaimViewModel.Pareceres.ParecerSupervisor.Id;
            clientClaimViewModel.ParecerAdministraçãoId = clientClaimViewModel.Pareceres.ParecerAdministração.Id;
            clientClaimViewModel.ParecerRevisorId = clientClaimViewModel.Pareceres.ParecerRevisor.Id;
        }


        //---------------------------------------------------------------------------------------------------
    }
}