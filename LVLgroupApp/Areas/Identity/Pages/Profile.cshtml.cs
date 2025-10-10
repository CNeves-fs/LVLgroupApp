using AspNetCoreHero.Results;
using AutoMapper;
using Core.Enums;
using Core.Features.Empresas.Queries.GetAllCached;
using Core.Features.Empresas.Queries.GetById;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetById;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Business.Models.Empresa;
using LVLgroupApp.Areas.Business.Models.Loja;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages
{
    public class ProfileModel : BasePageModel<ProfileModel>
    {

        //---------------------------------------------------------------------------------------------------


        public string Username { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmpresaName { get; set; }
        public string LojaName { get; set; }
        public int EmpresaId { get; set; }
        public int GrupolojaId { get; set; }
        public int LojaId { get; set; }
        public SelectList Empresas { get; set; }
        public SelectList Gruposlojas { get; set; }
        public SelectList Lojas { get; set; }
        public byte[] ProfilePicture { get; set; }
        public bool IsActive { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public List<string> RolesList { get; set; }
        
        public bool IsSuperAdmin { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsRevisor { get; set; }
        public bool IsSupervisor { get; set; }
        public bool IsGerenteLoja { get; set; }
        public bool IsColaborador { get; set; }
        public bool IsBasic { get; set; }


        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IStringLocalizer<ProfileModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ProfileModel(UserManager<ApplicationUser> userManager, IStringLocalizer<ProfileModel> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task OnGetAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                RolesList = roles.ToList();
                IsSuperAdmin = roles.Contains(Roles.SuperAdmin.ToString());
                IsAdmin = roles.Contains(Roles.Admin.ToString());
                IsRevisor = roles.Contains(Roles.Revisor.ToString());
                IsSupervisor = roles.Contains(Roles.Supervisor.ToString());
                IsGerenteLoja = roles.Contains(Roles.GerenteLoja.ToString());
                IsColaborador = roles.Contains(Roles.Colaborador.ToString());
                IsBasic = roles.Contains(Roles.Basic.ToString());

                UserId = userId;
                Username = user.UserName;
                ProfilePicture = user.ProfilePicture;
                FirstName = user.FirstName;
                LastName = user.LastName;
                EmpresaName = "";
                LojaName = "";
                IsActive = user.IsActive;

                if (IsBasic || IsColaborador || IsGerenteLoja || IsSupervisor)
                {
                    // defenir Empresa
                    if (user.EmpresaId  != null)
                    {
                        // user já tem Empresa defenida
                        EmpresaId = (int)user.EmpresaId;
                        Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(EmpresaId, _mapper, _mediator);
                        var emp = await EmpresaController.GetEmpresaAsync(EmpresaId, _mapper, _mediator);
                        EmpresaName = emp.Nome;
                    }
                    else
                    {
                        // user ainda não tem Empresa defenida
                        EmpresaId = 0;
                        Empresas = await EmpresaController.GetSelectListAllEmpresasAsync(EmpresaId, _mapper, _mediator);
                        EmpresaName = string.Empty;
                    }

                    // defenir GrupoLoja
                    if (user.GrupolojaId  != null)
                    {
                        // user já tem GrupoLoja defenido
                        GrupolojaId = (int)user.GrupolojaId;
                        Gruposlojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(EmpresaId, GrupolojaId, _mapper, _mediator);
                    }
                    else
                    {
                        // user já tem GrupoLoja defenido
                        GrupolojaId = 0;
                        Gruposlojas = await GrupolojaController.GetSelectListGruposlojasFromEmpresaAsync(EmpresaId, GrupolojaId, _mapper, _mediator);
                    }
                }

                if (IsBasic || IsColaborador || IsGerenteLoja)
                {
                    // defenir Loja
                    if (user.LojaId != null)
                    {
                        // user já tem Loja defenida
                        LojaId = (int)user.LojaId;
                        Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(GrupolojaId, LojaId, _mapper, _mediator);
                    }
                    else
                    {
                        // user ainda não tem Loja defenida
                        LojaId = 0;
                        Lojas = await LojaController.GetSelectListLojasFromGrupolojaAsync(GrupolojaId, LojaId, _mapper, _mediator);
                    }
                }

            }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostUpdateLojaAsync(string userId, int lojaId, int grupolojaId, int empresaId)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                var currentUser = await _userManager.FindByIdAsync(userId);

                // atualizar RoleName
                var roles = await _userManager.GetRolesAsync(currentUser);
                var userRoles = roles.ToList();
                currentUser.RoleName = roles.Count > 0 ? roles[0] : string.Empty;

                if (empresaId > 0) currentUser.EmpresaId = empresaId;
                else currentUser.EmpresaId = null;
                if (grupolojaId > 0) currentUser.GrupolojaId = grupolojaId;
                else currentUser.GrupolojaId = null;
                if (lojaId > 0)
                {
                    currentUser.LojaId = lojaId;
                    // atualizar mercadaId
                    var loja = await LojaController.GetLojaAsync(lojaId, _mapper, _mediator);
                    currentUser.MercadoId = loja.MercadoId;
                }
                else
                {
                    currentUser.LojaId = null;
                    currentUser.MercadoId = null;
                }

                // atualizar Local
                var isSupervisor = string.IsNullOrEmpty(currentUser.RoleName) ? false : Roles.Supervisor.ToString().Contains(currentUser.RoleName, StringComparison.OrdinalIgnoreCase);
                var isGerenteLoja = string.IsNullOrEmpty(currentUser.RoleName) ? false : Roles.GerenteLoja.ToString().Contains(currentUser.RoleName, StringComparison.OrdinalIgnoreCase);
                var isColaborador = string.IsNullOrEmpty(currentUser.RoleName) ? false : Roles.Colaborador.ToString().Contains(currentUser.RoleName, StringComparison.OrdinalIgnoreCase);
                var isBasic = string.IsNullOrEmpty(currentUser.RoleName) ? false : Roles.Basic.ToString().Contains(currentUser.RoleName, StringComparison.OrdinalIgnoreCase);

                if (isSupervisor) currentUser.Local = await GrupolojaController.GetGrupolojaNomeAsync((int)currentUser.GrupolojaId, _mapper, _mediator);
                if (isGerenteLoja || isColaborador || isBasic) currentUser.Local = await LojaController.GetLojaNomeAsync((int)currentUser.LojaId, _mapper, _mediator);


                //currentUser.ActivatedBy = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                await _userManager.UpdateAsync(currentUser);
                await OnGetAsync(userId);
                _notify.Success($"{_localizer["As definições de"]} {currentUser.Email} {_localizer["foram atualizadas com sucesso."]}");
                
                return RedirectToPage("Profile", new { area = "Identity", userId = userId });
            }
            else return default;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostActivateUserAsync(string userId)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                var currentUser = await _userManager.FindByIdAsync(userId);
                currentUser.IsActive = true;
                currentUser.EmpresaId = null;
                currentUser.GrupolojaId = null;
                currentUser.LojaId = null;
                currentUser.Local = string.Empty;

                var roles = await _userManager.GetRolesAsync(currentUser);
                await _userManager.RemoveFromRolesAsync(currentUser, roles);
                await _userManager.AddToRoleAsync(currentUser, Roles.Colaborador.ToString());

                //currentUser.ActivatedBy = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                await _userManager.UpdateAsync(currentUser);
                await OnGetAsync(userId);
                return RedirectToPage("Profile", new { area = "Identity", userId = userId });
            }
            else return default;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostDeActivateUserAsync(string userId)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                var currentUser = await _userManager.FindByIdAsync(userId);
                currentUser.IsActive = false;
                await _userManager.UpdateAsync(currentUser);
                return RedirectToPage("Profile", new { area = "Identity", userId = userId });
            }
            else return default;
        }

        //---------------------------------------------------------------------------------------------------

    }
}