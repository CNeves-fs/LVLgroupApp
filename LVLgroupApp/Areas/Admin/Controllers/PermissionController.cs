using Core.Constants;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Admin.Models;
using LVLgroupApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PermissionController : BaseController<PermissionController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStringLocalizer<PermissionController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public PermissionController(RoleManager<IdentityRole> roleManager,
                                    UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    IStringLocalizer<PermissionController> localizer)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.View)]
        public async Task<ActionResult> Index(string roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.AuditLogs), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Users), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Empresas), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Lojas), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Claims), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Fotos), roleId);

            var role = await _roleManager.FindByIdAsync(roleId);
            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);
            var claimsModel = _mapper.Map<List<RoleClaimsViewModel>>(claims);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claimsModel.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = _mapper.Map<List<RoleClaimsViewModel>>(allPermissions);
            ViewData["Title"] = $"{_localizer["Gestão das Permissões da Função"]} {role.Name}";
            ViewData["Caption"] = $"{_localizer["Permissões da Função"]} {role.Name}.";
            _notify.Success($"{_localizer["Permissões atualizadas para a Função"]} '{role.Name}'");
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Users.Edit)]
        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            //Remove all Claims First
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            _notify.Success($"{_localizer["Permissões atualizadas para a Função"]} '{role.Name}'");
            //var user = await _userManager.GetUserAsync(User);
            //await _signInManager.RefreshSignInAsync(user);

            return RedirectToAction("Index", new { roleId = model.RoleId });
        }


        //---------------------------------------------------------------------------------------------------

    }
}