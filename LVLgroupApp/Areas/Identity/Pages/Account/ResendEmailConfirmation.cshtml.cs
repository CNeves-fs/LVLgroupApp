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
    public class ResendEmailConfirmationModel : BasePageModel<ResendEmailConfirmationModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IEmailSender _emailSender;

        private readonly IStringLocalizer<ResendEmailConfirmationModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ResendEmailConfirmationModel(
            UserManager<ApplicationUser> userManager, 
            IEmailSender emailSender,
            IStringLocalizer<ResendEmailConfirmationModel> localizer)
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


        public void OnGet()
        {
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, _localizer["Email de verificação enviado. Por favor verifique o seu email."]);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                Input.Email,
                _localizer["Confirme o seu email"],
                $"{_localizer["Por favor, confirme a sua conta ao fazer"]} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{_localizer["click aqui"]}</a>.");

            ModelState.AddModelError(string.Empty, _localizer["Email de verificação enviado. Por favor verifique o seu email."]);
            return Page();
        }


        //---------------------------------------------------------------------------------------------------

    }
}