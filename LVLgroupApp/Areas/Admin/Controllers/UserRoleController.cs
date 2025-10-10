using Core.Constants;
using Core.Enums;
using Core.Entities.Identity;
using Infrastructure.Data.Seeds;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Admin.Models;
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
    public class UserRoleController : BaseController<UserRoleController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IStringLocalizer<UserRoleController> _localizer;


        //---------------------------------------------------------------------------------------------------


        public UserRoleController(UserManager<ApplicationUser> userManager, 
                                  SignInManager<ApplicationUser> signInManager, 
                                  RoleManager<IdentityRole> roleManager, 
                                  IStringLocalizer<UserRoleController> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.Edit)]
        public async Task<IActionResult> Index(string userId)
        {
            var viewModel = new List<UserRolesViewModel>();
            var user = await _userManager.FindByIdAsync(userId);
            ViewData["Title"] = $"{user.FirstName} {user.LastName} - {_localizer["Funções!"]}";
            ViewData["Caption"] = $"{_localizer["Gestão de funções para"]} {user.Email}.";
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }

            IComparer<UserRolesViewModel> comparer = new RoleSorting();
            viewModel.Sort(comparer);

            var model = new ManageUserRolesViewModel()
            {
                UserId = userId,
                UserRoles = viewModel
            };

            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Users.Edit)]
        public async Task<IActionResult> Update(string id, ManageUserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));

            user.EmpresaId = null;
            user.GrupolojaId = null;
            user.LojaId = null;
            user.Local = string.Empty;
            await _userManager.UpdateAsync(user);
            
            var currentUser = await _userManager.GetUserAsync(User);
            await _signInManager.RefreshSignInAsync(currentUser);
            await DefaultSuperAdminUser.SeedAsync(_userManager, _roleManager);
            _notify.Success($"{_localizer["Foram atualizadas as funções do utilizador"]} {user.Email}");
            return RedirectToAction("Index", new { userId = id });
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class RoleSorting : IComparer<UserRolesViewModel>
    {
        public int Compare(UserRolesViewModel x, UserRolesViewModel y)
        {
            if (x.RoleName == Roles.Basic.ToString()) return -1;
            if (x.RoleName == Roles.SuperAdmin.ToString()) return 1;

            if (x.RoleName == Roles.Colaborador.ToString() && y.RoleName == Roles.Basic.ToString()) return 1;
            if (x.RoleName == Roles.Colaborador.ToString()) return -1;

            if (x.RoleName == Roles.GerenteLoja.ToString() && (y.RoleName == Roles.Basic.ToString() || y.RoleName == Roles.Colaborador.ToString())) return 1;
            if (x.RoleName == Roles.GerenteLoja.ToString()) return -1;

            if (x.RoleName == Roles.Revisor.ToString() && (y.RoleName == Roles.Basic.ToString() || y.RoleName == Roles.Colaborador.ToString() || y.RoleName == Roles.GerenteLoja.ToString())) return 1;
            if (x.RoleName == Roles.Revisor.ToString()) return -1;

            if (x.RoleName == Roles.Supervisor.ToString() && (y.RoleName == Roles.Basic.ToString() || y.RoleName == Roles.Colaborador.ToString() || y.RoleName == Roles.GerenteLoja.ToString() || y.RoleName == Roles.Revisor.ToString())) return 1;
            if (x.RoleName == Roles.Supervisor.ToString()) return -1;

            if (x.RoleName == Roles.Admin.ToString() && (y.RoleName == Roles.Basic.ToString() || y.RoleName == Roles.Colaborador.ToString() || y.RoleName == Roles.GerenteLoja.ToString() || y.RoleName == Roles.Revisor.ToString() || y.RoleName == Roles.Supervisor.ToString())) return 1;
            if (x.RoleName == Roles.Admin.ToString()) return -1;

            return 0;
        }
    }


    //---------------------------------------------------------------------------------------------------

}