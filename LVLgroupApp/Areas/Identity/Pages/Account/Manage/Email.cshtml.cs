using Core.Entities.Mail;
using Core.Interfaces.Shared;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : BasePageModel<EmailModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<EmailModel> _localizer;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMailService _emailSender;


        //---------------------------------------------------------------------------------------------------


        public EmailModel(
            IStringLocalizer<EmailModel> localizer,
            UserManager<ApplicationUser> userManager,
            IMailService emailSender)
        {
            _localizer = localizer;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        //---------------------------------------------------------------------------------------------------


        public string UserId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class InputModel
        {
            public string Id { get; set; }

            [Required]
            [EmailAddress]
            //[Display(Name = "New email")]
            public string NewEmail { get; set; }
        }


        //---------------------------------------------------------------------------------------------------


        private async Task LoadAsync(ApplicationUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            Email = email;
            UserId = userId;

            Input = new InputModel
            {
                NewEmail = email,
                Id = userId
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetAsync(string userId = "")
        {
            if (userId == null || userId == "")
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnGetAsync - Não foi possível carregar o utilizador");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnGetAsync - Não foi possível carregar o utilizador com o ID"} '{userId}'.");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            await LoadAsync(user);
            _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnGetAsync - return Page");
            return Page();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var userId = Input.Id;
            if (userId == null || userId == "")
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostChangeEmailAsync - Não foi possível carregar o utilizador");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostChangeEmailAsync - Não foi possível carregar o utilizador com o ID"} '{userId}'.");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostChangeEmailAsync - return Page - Model Invalid");
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                //var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId = userId, email = Input.NewEmail, code = code },
                    protocol: Request.Scheme);
                var mailRequest = new MailRequest
                {
                    ToEmail = Input.NewEmail,
                    Subject = _localizer["Confirme o seu email"],
                    Body = $"{_localizer["Por favor, confirme sua conta ao fazer"]} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{_localizer["click aqui"]}</a>."
                };
                await _emailSender.SendAsync(mailRequest);

                StatusMessage = _localizer["Foi enviado link de confirmação para alteração do email. Verifique o seu email."];
                _notify.Success(StatusMessage);
                _logger.LogInformation(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostChangeEmailAsync - Foi enviado link de confirmação para alteração do email");
                return RedirectToPage(new { userid = userId });
            }

            StatusMessage = _localizer["O email não foi alterado."];
            _notify.Success(StatusMessage);
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostChangeEmailAsync - O email não foi alterado");
            return RedirectToPage(new { userid = userId });
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var userId = Input.Id;
            if (userId == null || userId == "")
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostSendVerificationEmailAsync - Não foi possível carregar o utilizador");
                return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostSendVerificationEmailAsync - Não foi possível carregar o utilizador com o ID");
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
                return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostSendVerificationEmailAsync - return Page - Model Invalid");
                return Page();
            }

            //var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = userId, code = code },
                protocol: Request.Scheme);
            var mailRequest = new MailRequest
            {
                ToEmail = Input.NewEmail,
                Subject = _localizer["Confirme o seu email"],
                Body = $"{_localizer["Por favor, confirme sua conta ao fazer"]} <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>{_localizer["click aqui"]}</a>."
            };
            await _emailSender.SendAsync(mailRequest);

            StatusMessage = _localizer["Foi enviado email de verificação. Verifique o seu email."];
            _notify.Success(StatusMessage);
            _logger.LogInformation(_sessionId + " | " + _sessionName + " | identity/account/manage/email - OnPostSendVerificationEmailAsync - Foi enviado email de verificação");
            return RedirectToPage(new { userid = userId });
        }


        //---------------------------------------------------------------------------------------------------

    }
}