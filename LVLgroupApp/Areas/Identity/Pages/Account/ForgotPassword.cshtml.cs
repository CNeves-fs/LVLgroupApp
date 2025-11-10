using Core.Entities.Mail;
using Core.Interfaces.Shared;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : BasePageModel<ForgotPasswordModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMailService _emailSender;

        private readonly IStringLocalizer<ForgotPasswordModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ForgotPasswordModel(
            UserManager<ApplicationUser> userManager, 
            IMailService emailSender,
            IStringLocalizer<ForgotPasswordModel> localizer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [BindProperty]
        public InputModel Input { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    _notify.Information(_localizer["Por favor verifique o seu email."]);
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);
                
                var mailRequest = new MailRequest
                {
                    ToEmail = Input.Email,
                    Subject = _localizer["Redefinir palavra-passe"],
                    Body = $"{_localizer["Por favor, redefina sua palavra-passe ao fazer"]} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{_localizer["click aqui"]}</a>."
                };
                await _emailSender.SendAsync(mailRequest);
                _notify.Success(_localizer["Link para alteração de palavra-passe enviado. Por favor verifique o seu email."]);
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }


        //---------------------------------------------------------------------------------------------------

    }
}