using Core.Features.Logs.Commands.AddActivityLog;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : BasePageModel<LoginModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<LoginModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<LoginModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var userName = Input.Email;
                if (IsValidEmail(Input.Email))
                {
                    var userCheck = await _userManager.FindByEmailAsync(Input.Email);
                    if (userCheck != null)
                    {
                        userName = userCheck.UserName;
                    }
                }
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        return RedirectToPage("./Deactivated");
                    }
                    else if (!user.EmailConfirmed)
                    {
                        _notify.Error(_localizer["O seu email não foi confirmado."]);
                        ModelState.AddModelError(string.Empty, _localizer["O seu email não foi confirmado."]);
                        return Page();
                    }
                    else
                    {
                        var result = await _signInManager.PasswordSignInAsync(userName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {
                            await _mediator.Send(new AddActivityLogCommand() { userId = user.Id, Action = "Logged In", email = user.Email });
                            _logger.LogInformation($"{"User logged in as"} {userName}.");
                            _notify.Success($"{_localizer["Início de sessão de"]} {userName}.");
                            return LocalRedirect(returnUrl);
                        }
                        await _mediator.Send(new AddActivityLogCommand() { userId = user.Id, Action = "Log-In Failed", email = user.Email });
                        if (result.RequiresTwoFactor)
                        {
                            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                        }
                        if (result.IsLockedOut)
                        {
                            _notify.Warning(_localizer["Conta do utilizador bloqueada."]);
                            _logger.LogWarning("User account locked out.");
                            return RedirectToPage("./Lockout");
                        }
                        else
                        {
                            _notify.Error(_localizer["Tentativa de login inválida."]);
                            ModelState.AddModelError(string.Empty, _localizer["Tentativa de login inválida."]);
                            return Page();
                        }
                    }
                }
                else
                {
                    _notify.Error(_localizer["Email ou nome do utilizador não encontrado."]);
                    ModelState.AddModelError(string.Empty, _localizer["Email ou nome do utilizador não encontrado."]);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


        //---------------------------------------------------------------------------------------------------


        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}