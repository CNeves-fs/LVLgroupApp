using AutoMapper;
using Azure;
using Core.Constants;
using Core.Entities.Business;
using Core.Entities.Identity;
using Core.Entities.Mail;
using Core.Entities.Select2;
using Core.Enums;
using Core.Features.Charts.CountQueries.CountAllClaimsCached;
using Core.Features.Claims.Queries.GetAllCached;
using Core.Features.Gruposlojas.Queries.GetById;
using Core.Features.Lojas.Queries.GetAllCached;
using Core.Features.Lojas.Queries.GetById;
using Core.Interfaces.Shared;
using Infrastructure.Data.DbContext;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Admin.Models;
using LVLgroupApp.Areas.Business.Controllers.Empresa;
using LVLgroupApp.Areas.Business.Controllers.Grupoloja;
using LVLgroupApp.Areas.Business.Controllers.Loja;
using LVLgroupApp.Areas.Claim.Models.Claim;
using LVLgroupApp.Areas.Notification.Models.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseController<UserController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IStringLocalizer<UserController> _localizer;

        private readonly IMailService _emailSender;

        private readonly DateTime _endDate;


        //---------------------------------------------------------------------------------------------------


        public UserController(UserManager<ApplicationUser> userManager, 
                              SignInManager<ApplicationUser> signInManager, 
                              RoleManager<IdentityRole> roleManager, 
                              IStringLocalizer<UserController> localizer,
                              IMailService emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _localizer = localizer;
            _emailSender = emailSender;

            _endDate = new DateTime(2222, 06, 06);
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
            try
            {
                var users = _context.Users.ToList();
                var allUsers = new List<UserViewModel>();

                foreach(ApplicationUser user in users)
                {
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - LoadAll - Vai ler os roles de: " + user.Email);
                    var roles = await _userManager.GetRolesAsync(user);
                    var userRoles = roles.ToList();

                    var uvm = new UserViewModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        RoleName = roles.Count > 0 ? roles[0] : string.Empty,
                        Email = user.Email,
                        EmailConfirmed = user.EmailConfirmed,
                        ProfilePicture = user.ProfilePicture,
                        IsActive = user.IsActive,
                    };

                    if (user.EmpresaId != null)
                    {
                        var emp = await EmpresaController.GetEmpresaAsync((int)user.EmpresaId, _mapper, _mediator);
                        uvm.NomeEmpresa = emp.Nome;
                    }
                    if (user.GrupolojaId != null)
                    {
                        uvm.NomeGrupoloja = await GrupolojaController.GetGrupolojaNomeAsync((int)user.GrupolojaId, _mapper, _mediator);
                    }
                    if (user.LojaId != null)
                    {
                        uvm.NomeLoja = await LojaController.GetLojaNomeAsync((int)user.LojaId, _mapper, _mediator);
                    }

                    allUsers.Add(uvm);
                }
                return PartialView("_ViewAll", allUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - LoadAll - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.Create)]
        public async Task<IActionResult> OnGetCreate()
        {
            var model = new UserViewModel();
            model.EmailConfirmed = true;
            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Create", model) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Users.Create)]
        public async Task<IActionResult> OnPostCreate(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                MailAddress address = new MailAddress(userModel.Email);
                string userName = address.User;
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userModel.Email,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    EmailConfirmed = userModel.EmailConfirmed
                };
                
                var result = await _userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Colaborador.ToString());
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    if (!userModel.EmailConfirmed)
                    {
                        // ------ send email --------
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = Url.Content("~/") },
                            protocol: Request.Scheme);
                        var mailRequest = new MailRequest
                        {
                            Body = $"{_localizer["Please confirm your account by"]} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{_localizer["clicking here"]}</a>.",
                            ToEmail = userModel.Email,
                            Subject = _localizer["Confirm Registration"]
                        };
                        await _emailSender.SendAsync(mailRequest);
                    }

                    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                    var allUsers = await _userManager.Users.ToListAsync();
                    var users = _mapper.Map<IEnumerable<UserViewModel>>(allUsers);
                    var htmlData = await _viewRenderer.RenderViewToStringAsync("_ViewAll", users);
                    _notify.Success($"{_localizer["Foi criada uma conta para "]} {user.Email}.");
                    return new JsonResult(new { isValid = true, html = htmlData });
                }
                foreach (var error in result.Errors)
                {
                    _notify.Error(error.Description);
                }
                var html = await _viewRenderer.RenderViewToStringAsync("_Create", userModel);
                return new JsonResult(new { isValid = false, html = html });
            }
            else
            {
                var html = await _viewRenderer.RenderViewToStringAsync("_Create", userModel);
                return new JsonResult(new { isValid = false, html = html });
            }
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.Edit)]
        public async Task<IActionResult> OnGetEdit(string userId = "")
        {
            if (userId == null || userId == "")
            {
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Edit", new UserViewModel()) });
            }
            var model = new UserViewModel();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Edit", new UserViewModel()) });
            }
            model.NomeEmpresa = string.Empty;
            model.LojaId = 0;
            if (user.LojaId != null)
            {
                model.LojaId = (int) user.LojaId;
                model.NomeEmpresa = await EmpresaController.GetEmpresaNomeAsync((int) user.LojaId, _mapper, _mediator);
                model.Lojas = await LojaController.GetSelectListAllLojasAsync(model.LojaId, _mapper, _mediator);
            }
            model.Id = userId;
            model.UserName = user.UserName;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Email = user.Email;
            model.EmailConfirmed = user.EmailConfirmed;

            return new JsonResult(new { isValid = true, html = await _viewRenderer.RenderViewToStringAsync("_Edit", model) });
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost]
        [Authorize(Policy = Permissions.Users.Edit)]
        public async Task<IActionResult> OnPostEdit(UserViewModel userModel)
        {
            if (ModelState.IsValid)
            {
                MailAddress address = new MailAddress(userModel.Email);
                string userName = address.User;
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = userModel.Email,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    EmpresaId = userModel.EmpresaId,
                    GrupolojaId = userModel.GrupolojaId,
                    LojaId = userModel.LojaId,
                    EmailConfirmed = userModel.EmailConfirmed
                };
                var result = await _userManager.CreateAsync(user, userModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Colaborador.ToString());
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    if (!userModel.EmailConfirmed)
                    {
                        // ------ send email --------
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code, returnUrl = Url.Content("~/") },
                            protocol: Request.Scheme);
                        var mailRequest = new MailRequest
                        {
                            Body = $"{_localizer["Please confirm your account by"]} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{_localizer["clicking here"]}</a>.",
                            ToEmail = userModel.Email,
                            Subject = _localizer["Confirm Registration"]
                        };
                        await _emailSender.SendAsync(mailRequest);
                    }

                    var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                    var allUsers = await _userManager.Users.ToListAsync();
                    var users = _mapper.Map<IEnumerable<UserViewModel>>(allUsers);
                    var htmlData = await _viewRenderer.RenderViewToStringAsync("_ViewAll", users);
                    _notify.Success($"{_localizer["It has been created an account for"]} {user.Email}.");
                    return new JsonResult(new { isValid = true, html = htmlData });
                }
                foreach (var error in result.Errors)
                {
                    _notify.Error(error.Description);
                }
                var html = await _viewRenderer.RenderViewToStringAsync("_Create", userModel);
                return new JsonResult(new { isValid = false, html = html });
            }
            return default;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.Delete)]
        public async Task<JsonResult> OnPostDelete(string id)
        {
            if (id == null)
            {
                _notify.Error(_localizer["Utilizador não defenido."]);
                return new JsonResult(new { isValid = false });
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _notify.Error(_localizer["Utilizador não encontrado."]);
                return new JsonResult(new { isValid = false });
            }

            var rolesForUser = await _userManager.GetRolesAsync(user);
            using (var transaction = _context.Database.BeginTransaction())
            {
                if (rolesForUser.Count() > 0)
                {
                    foreach (var role in rolesForUser.ToList())
                    {
                        // role é o nome do role
                        var resultRemoveRole = await _userManager.RemoveFromRoleAsync(user, role);
                        if (!resultRemoveRole.Succeeded)
                        {
                            _notify.Error($"{_localizer["Erro ao remover a função "]} {role} {_localizer["do utilizador"]} {user.Email}.");
                            return new JsonResult(new { isValid = false });
                        }
                    }
                }
                var resultDeleteUser = await _userManager.DeleteAsync(user);
                if (!resultDeleteUser.Succeeded)
                {
                    _notify.Error($"{_localizer["Erro ao remover o utilizador"]} {user.Email}.");
                    return new JsonResult(new { isValid = false });
                }
                transaction.Commit();
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsers = await _userManager.Users.ToListAsync();
            var users = _mapper.Map<IEnumerable<UserViewModel>>(allUsers);
            var htmlData = await _viewRenderer.RenderViewToStringAsync("_ViewAll", users);
            _notify.Success($"{_localizer["O utilizador"]} {user.Email} {_localizer["foi removido com sucesso"]}.");
            return new JsonResult(new { isValid = true, html = htmlData });
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.Edit)]
        public bool LockUser(string email, DateTime? endDate)
        {
            if (endDate == null)
                endDate = _endDate;

            var userTask = _userManager.FindByEmailAsync(email);
            userTask.Wait();
            var user = userTask.Result;

            var lockUserTask = _userManager.SetLockoutEnabledAsync(user, true);
            lockUserTask.Wait();

            var lockDateTask = _userManager.SetLockoutEndDateAsync(user, endDate);
            lockDateTask.Wait();

            return lockDateTask.Result.Succeeded && lockUserTask.Result.Succeeded;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.Edit)]
        public bool UnlockUser(string email)
        {
            var userTask = _userManager.FindByEmailAsync(email);
            userTask.Wait();
            var user = userTask.Result;

            var lockDisabledTask = _userManager.SetLockoutEnabledAsync(user, false);
            lockDisabledTask.Wait();

            var setLockoutEndDateTask = _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));
            setLockoutEndDateTask.Wait();

            return setLockoutEndDateTask.Result.Succeeded && lockDisabledTask.Result.Succeeded;
        }


        //---------------------------------------------------------------------------------------------------


        [Authorize(Policy = Permissions.Users.Edit)]
        public async Task<JsonResult> UpdateMigration()
        {
            try
            {
                //var users = _context.Users.Where(u => u.IsActive).ToList();
                var users = _context.Users.ToList();
                foreach (ApplicationUser user in users)
                {
                    var result = await RepairApplicationUserAsync(user);
                    if (!result)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - UpdateMigration -  Erro: Falha na reparação do user: " + user.Email);
                    }
                    else
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - UpdateMigration -  User verificado e consistente: " + user.Email);
                    }
                }
                var allUsers = _context.Users.Where(u => string.IsNullOrEmpty(u.Local)).ToList();
                foreach (ApplicationUser user in allUsers)
                {
                    var result = await RepairApplicationUserLocalAsync(user);
                    if (!result)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - UpdateMigration -  Erro: Falha na reparação do user.Local: " + user.Email);
                    }
                    else
                    {
                        _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - UpdateMigration -  User.Local verificado e consistente: " + user.Email);
                    }
                }
                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - UpdateMigration - Exception vai sair e retornar Error: " + ex.Message);
                return Json(new { status = "fail" });
            }
        }


        //---------------------------------------------------------------------------------------------------


        public IActionResult OnGetListAllUsers(string term, int page)
        {
            int resultCount = 15;
            int offset = (page - 1) * resultCount;
            string searchStr = string.IsNullOrEmpty(term) ? "" : term;

            try
            {
                var users = _context.Users.Where(u => u.IsActive).ToList();
                var allUsers = _mapper.Map<List<UserViewModel>>(users);

                var searchData = allUsers.Where(u => u.Email.Contains(searchStr, StringComparison.OrdinalIgnoreCase) ||
                                                     u.FirstName.Contains(searchStr, StringComparison.OrdinalIgnoreCase) ||
                                                     u.LastName.Contains(searchStr, StringComparison.OrdinalIgnoreCase));

                int count = searchData.Count();
                searchData = searchData.Skip(offset).Take(resultCount);

                var usersData = searchData.Select(u => new {
                    id = u.Id,
                    text = u.Email,
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    roleName = u.RoleName,
                    local = u.Local,
                    profilePicture = Convert.ToBase64String(u.ProfilePicture)
                }).ToList();

                int endCount = offset + resultCount;
                bool morePages = count > endCount;

                var data = new
                {
                    results = searchData,
                    pagination = new
                    {
                        more = morePages
                    }
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - OnGetListAllUsers - Exception vai sair e retornar Error: " + ex.Message);
                return new ObjectResult(new { status = "error" });
            }

        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<bool> RepairApplicationUserAsync(ApplicationUser user)
        {
            try
            {
                //IsActive
                if (!user.IsActive)
                {
                    // tentar remover
                    var response = await _mediator.Send(new GetAllClaimsCachedQuery());
                    if (!response.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync - Erro ao ler todos os users da base de dados. ");
                        return false;
                    }
                    var userClaims = response.Data.Where(c => (c.EmailAutor == user.Email) || (c.EmailAutorDecisão == user.Email) || (c.EmailAutorFechoEmLoja == user.Email)).ToList();
                    if (userClaims.Count == 0)
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            var resultDeleteUser = await _userManager.DeleteAsync(user);
                            if (!resultDeleteUser.Succeeded)
                            {
                                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync - Error deleting user " + user.Email);
                                return false;
                            }
                            transaction.Commit();
                            _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync - User deleted: " + user.Email);
                            return true;
                        }
                    }
                }

                // user isActive ou is not active mas tem claims associadas
                // EmailConfirmed
                user.EmailConfirmed = true;

                //roles - RoleName
                var roles = await _userManager.GetRolesAsync(user);
                var userRoles = roles.ToList();
                user.RoleName = roles.Count > 0 ? roles[0] : string.Empty;
                if (user.IsActive) user.IsActive = roles.Count > 0;

                //Local
                var isSuperadmin = string.IsNullOrEmpty(user.RoleName) ? false : Roles.SuperAdmin.ToString().Equals(user.RoleName, StringComparison.OrdinalIgnoreCase);
                var isAdmin = string.IsNullOrEmpty(user.RoleName) ? false : Roles.Admin.ToString().Equals(user.RoleName, StringComparison.OrdinalIgnoreCase);
                var isRevisor = string.IsNullOrEmpty(user.RoleName) ? false : Roles.Revisor.ToString().Equals(user.RoleName, StringComparison.OrdinalIgnoreCase);
                var isSupervisor = string.IsNullOrEmpty(user.RoleName) ? false : Roles.Supervisor.ToString().Equals(user.RoleName, StringComparison.OrdinalIgnoreCase);
                var isGerenteLoja = string.IsNullOrEmpty(user.RoleName) ? false : Roles.GerenteLoja.ToString().Equals(user.RoleName, StringComparison.OrdinalIgnoreCase);
                var isColaborador = string.IsNullOrEmpty(user.RoleName) ? false : Roles.Colaborador.ToString().Equals(user.RoleName, StringComparison.OrdinalIgnoreCase);
                var isBasic = string.IsNullOrEmpty(user.RoleName) ? false : Roles.Basic.ToString().Equals(user.RoleName, StringComparison.OrdinalIgnoreCase);

                if (!isSuperadmin && !isColaborador && !isRevisor && !isSupervisor && !isGerenteLoja && !isAdmin && !isBasic)
                {
                    _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: user com role inconsistente: " + user.Email);
                    return false;
                }

                if (isSuperadmin || isAdmin || isRevisor)
                {
                    user.Local = "Office";
                    user.LojaId = null;
                    user.GrupolojaId = null;
                    user.EmpresaId = null;
                    user.MercadoId = null;
                    await _userManager.UpdateAsync(user);
                    return true;
                }

                if (isSupervisor)
                {
                    if (user.GrupolojaId == null || user.GrupolojaId == 0)
                    {
                        //user.IsActive = false;
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: user com role Supervisor com GrupolojaId inconsistente: " + user.Email);
                        return false;
                    }
                    if (user.EmpresaId == null || user.EmpresaId == 0)
                    {
                        //user.IsActive = false;
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: user com role Supervisor com EmpresaId inconsistente: " + user.Email);
                        user = await RepairSupervisorAsync(user);
                    }
                    if (user.MercadoId == null || user.MercadoId == 0)
                    {
                        //user.IsActive = false;
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: user com role Supervisor com MercadoId inconsistente: " + user.Email);
                        user = await RepairSupervisorAsync(user);
                    }
                    if (user == null)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: reparação de user falhou!!!");
                        return false;
                    }
                    user.Local = await GrupolojaController.GetGrupolojaNomeAsync((int)user.GrupolojaId, _mapper, _mediator);
                    user.LojaId = null;
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync - User ok : " + user.Email);
                    return true;
                }

                if (isGerenteLoja || isColaborador || isBasic)
                {
                    if (user.LojaId == null || user.LojaId == 0)
                    {
                        //user.IsActive = false;
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: user com role GerenteLoja/Colaborador/Basic com LojaId inconsistente: " + user.Email);
                        return false;
                    }
                    if (user.GrupolojaId == null || user.GrupolojaId == 0)
                    {
                        //user.IsActive = false;
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: user com role GerenteLoja/Colaborador/Basic com GrupolojaId inconsistente: " + user.Email);
                        user = await RepairUserInLojaAsync(user);
                    }
                    if (user.EmpresaId == null || user.EmpresaId == 0)
                    {
                        //user.IsActive = false;
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: user com role GerenteLoja/Colaborador/Basic com EmpresaId inconsistente: " + user.Email);
                        user = await RepairUserInLojaAsync(user);
                    }
                    if (user.MercadoId == null || user.MercadoId == 0)
                    {
                        //user.IsActive = false;
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: user com role GerenteLoja/Colaborador/Basic com MercadoId inconsistente: " + user.Email);
                        user = await RepairUserInLojaAsync(user);
                    }
                    if (user == null)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync -  Erro: reparação de user falhou!!!");
                        return false;
                    }
                    user.Local = await LojaController.GetLojaNomeAsync((int)user.LojaId, _mapper, _mediator);
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync - User ok : " + user.Email);
                    return true;
                }
                // se chegar aqui é porque o user tem role inconsistente
                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - UpdateMigration -  Erro: user com role inconsistente: " + user.Email);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync - Exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        internal async Task<bool> RepairApplicationUserLocalAsync(ApplicationUser user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.Local))
                {
                    // tentar remover
                    var response = await _mediator.Send(new GetAllClaimsCachedQuery());
                    if (!response.Succeeded)
                    {
                        _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserLocalAsync - Erro ao ler todos os users da base de dados. ");
                        return false;
                    }
                    var userClaims = response.Data.Where(c => (c.EmailAutor == user.Email) || (c.EmailAutorDecisão == user.Email) || (c.EmailAutorFechoEmLoja == user.Email)).ToList();
                    if (userClaims.Count == 0)
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            var resultDeleteUser = await _userManager.DeleteAsync(user);
                            if (!resultDeleteUser.Succeeded)
                            {
                                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserLocalAsync - Error deleting user " + user.Email);
                                return false;
                            }
                            transaction.Commit();
                            _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserLocalAsync - User deleted: " + user.Email);
                            return true;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairApplicationUserAsync - Exception vai sair e retornar Error: " + ex.Message);
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// o user com role GerenteLoja/Colaborador/Basic tem de ter sempre uma loja associada
        /// assim como grupo de lojas, empresa e mercado.
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>ApplicationUser reparado</returns>

        internal async Task<ApplicationUser> RepairUserInLojaAsync(ApplicationUser user)
        {
            if (user.LojaId == null || user.LojaId == 0) return null;

            // ler loja da base de dados
            var loja = await LojaController.GetLojaAsync((int)user.LojaId, _mapper, _mediator);
            if (loja == null)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairUserInLojaAsync - Erro ao ler loja da base de dados. ");
                return null;
            }

            // ler empresaId da base de dados
            user.EmpresaId = await EmpresaController.GetEmpresaIdAsync(loja.Id, _mapper, _mediator);
            user.GrupolojaId = loja.GrupolojaId;
            user.MercadoId = loja.MercadoId;

            _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - RepairUserInLojaAsync -  User com role GerenteLoja/Colaborador/Basic reparado: " + user.Email);
            return user;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// o user com role Supervisor tem de ter sempre um grupo de lojas associado
        /// assim como empresa e mercado.
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>ApplicationUser reparado</returns>

        internal async Task<ApplicationUser> RepairSupervisorAsync(ApplicationUser user)
        {
            if (user.GrupolojaId == null || user.GrupolojaId == 0) return null;

            // ler grupoloja da base de dados
            var response = await _mediator.Send(new GetLojasByGrupolojaIdCachedQuery() { grupolojaId = (int)user.GrupolojaId });
            if (!response.Succeeded || response.Data == null || response.Data.Count == 0)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | User Contoller - RepairSupervisorAsync - Erro ao ler grupo de lojas da base de dados. ");
                return null;
            }
            var firstLoja = response.Data.FirstOrDefault();

            // ler empresaId da base de dados
            user.EmpresaId = await EmpresaController.GetEmpresaIdAsync(firstLoja.Id, _mapper, _mediator);
            user.MercadoId = firstLoja.MercadoId;
            user.LojaId = null;

            _logger.LogInformation(_sessionId + " | " + _sessionName + " | User Contoller - RepairUserInLojaAsync -  User com role Supervisor reparado " + user.Email);
            return user;
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve lista com todos os users ativos
        /// </summary>
        /// <returns>List<ApplicationUser></returns>

        public static List<ApplicationUser> GetAllUsersAsync(LVLgroupDbContext context)
        {
            try
            {
                var users = context.Users.ToList();
                return users.Where(u => u.IsActive).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve lista com todos os users ativos de uma loja
        /// </summary>
        /// <returns>List<ApplicationUser></returns>

        public static List<ApplicationUser> GetAllUsersFromLoja(int lojaId, LVLgroupDbContext context)
        {
            try
            {
                var users = context.Users.ToList();
                return users.Where(u => u.IsActive && u.LojaId == lojaId).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve lista com todos os users ativos de um grupoloja
        /// </summary>
        /// <returns>List<ApplicationUser></returns>

        public static List<ApplicationUser> GetAllUsersFromGrupoLoja(int grupolojaId, LVLgroupDbContext context)
        {
            try
            {
                var users = context.Users.ToList();
                return users.Where(u => u.IsActive && u.GrupolojaId == grupolojaId).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve lista com todos os users ativos de uma empresa
        /// </summary>
        /// <returns>List<ApplicationUser></returns>

        public static List<ApplicationUser> GetAllUsersFromEmpresa(int empresaId, LVLgroupDbContext context)
        {
            try
            {
                var users = context.Users.ToList();
                return users.Where(u => u.IsActive && u.EmpresaId == empresaId).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve lista com todos os users ativos de um mercado
        /// </summary>
        /// <returns>List<ApplicationUser></returns>

        public static List<ApplicationUser> GetAllUsersFromMercado(int mercadoId, LVLgroupDbContext context)
        {
            try
            {
                var users = context.Users.ToList();
                return users.Where(u => u.IsActive && u.MercadoId == mercadoId).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// devolve lista com todos os users ativos de um role
        /// </summary>
        /// <returns>List<ApplicationUser></returns>

        public static List<ApplicationUser> GetAllUsersinRole(string roleName, UserManager<ApplicationUser> userManager, LVLgroupDbContext context)
        {
            try
            {
                var users = context.Users.ToList();
                users = users.Where(u => u.IsActive && u.RoleName == roleName).ToList();
                return users;

                //var allUsers = new List<UserViewModel>();

                //foreach (ApplicationUser user in users)
                //{
                //    var roles = await userManager.GetRolesAsync(user);
                //    var userRoles = roles.ToList();

                //    var uvm = new UserViewModel()
                //    {
                //        Id = user.Id,
                //        FirstName = user.FirstName,
                //        LastName = user.LastName,
                //        UserName = user.UserName,
                //        //RoleName = roles.Count > 0 ? roles[0] : string.Empty,
                //        Email = user.Email,
                //        EmailConfirmed = user.EmailConfirmed,
                //        ProfilePicture = user.ProfilePicture,
                //        IsActive = user.IsActive,
                //        LojaId = (int)user.LojaId,
                //        GrupolojaId = (int)user.GrupolojaId,
                //        EmpresaId = (int)user.EmpresaId,
                //        RoleName = roleName
                //    };

                //    allUsers.Add(uvm);
                //}
                //return allUsers;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}