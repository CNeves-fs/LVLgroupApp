using Core.Constants;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseController<RoleController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IStringLocalizer<RoleController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public RoleController(UserManager<ApplicationUser> userManager, 
                              RoleManager<IdentityRole> roleManager, 
                              IStringLocalizer<RoleController> localizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.View)]
        public IActionResult Index()
        {
            return View();
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.View)]
        public async Task<IActionResult> LoadAll()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var model = _mapper.Map<IEnumerable<RoleViewModel>>(roles);
            return PartialView("_ViewAll", model);
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.Create)]
        public async Task<IActionResult> OnGetCreate(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", new RoleViewModel()) });
            else
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) _notify.Error(_localizer["Erro inesperado. Função não encontrada!"]);
                var roleviewModel = _mapper.Map<RoleViewModel>(role);
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", roleviewModel) });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Users.Create)]
        public async Task<IActionResult> OnPostCreate(RoleViewModel role)
        {
            if (ModelState.IsValid && role.Name != "SuperAdmin" && role.Name != "Basic")
            {
                if (string.IsNullOrEmpty(role.Id))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.Name));
                    _notify.Success(_localizer["Função criada"]);
                }
                else
                {
                    var existingRole = await _roleManager.FindByIdAsync(role.Id);
                    existingRole.Name = role.Name;
                    existingRole.NormalizedName = role.Name.ToUpper();
                    await _roleManager.UpdateAsync(existingRole);
                    _notify.Success(_localizer["Função atualizada"]);
                }

                var roles = await _roleManager.Roles.ToListAsync();
                var mappedRoles = _mapper.Map<IEnumerable<RoleViewModel>>(roles);
                var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", mappedRoles);
                return new JsonResult(new { isValid = true, html = html });
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync<RoleViewModel>("_CreateOrEdit", role);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Users.Delete)]
        public async Task<JsonResult> OnPostDelete(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole.Name != "SuperAdmin" && existingRole.Name != "Basic")
            {
                //TODO Check if Any Users already uses this Role
                bool roleIsNotUsed = true;
                var allUsers = await _userManager.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                    {
                        roleIsNotUsed = false;
                    }
                }
                if (roleIsNotUsed)
                {
                    await _roleManager.DeleteAsync(existingRole);
                    _notify.Success($"{_localizer["Função"]} {existingRole.Name} {_localizer["eliminada."]}");
                }
                else
                {
                    _notify.Error(_localizer["A função está atribuída a um ou mais utilizadores. Não pode ser removida."]);
                }
            }
            else
            {
                _notify.Error($"{_localizer["Função"]} {existingRole.Name} {_localizer["não tem permissão para ser eliminada."]}");
            }
            var roles = await _roleManager.Roles.ToListAsync();
            var mappedRoles = _mapper.Map<IEnumerable<RoleViewModel>>(roles);
            var html = await _viewRenderer.RenderViewToStringAsync("_ViewAll", mappedRoles);
            return new JsonResult(new { isValid = true, html = html });
        }


        //---------------------------------------------------------------------------------------------------

    }
}