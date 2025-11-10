using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : BasePageModel<ChangePasswordModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<ChangePasswordModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<ChangePasswordModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class InputModel
        {
            public string Id { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetAsync(string userId = "")
        {
            if (userId == null || userId == "")
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/changepassword - OnGetAsync - Não foi possível carregar o utilizador");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/changepassword - OnGetAsync - Não foi possível carregar o utilizador com o ID"} '{userId}'.");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword", new {userId = userId });
            }

            Input = new InputModel
            {
                Id = userId
            };
            return Page();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostAsync()
        {
            var userId = Input.Id;
            if (userId == null || userId == "")
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/changepassword - OnPostAsync - Não foi possível carregar o utilizador");
                return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/changepassword - OnPostAsync - Não foi possível carregar o utilizador com o ID"} '{userId}'.");
                return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            if (!ModelState.IsValid)
            {
                Input = new InputModel
                {
                    Id = userId
                };
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/changepassword - OnPostAsync - Model not valid return page");
                return Page();
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                _notify.Error($"{_localizer["Erro ao alterar a palavra-passe."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/changepassword - OnPostAsync - Change password failed");
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation($"{_sessionId + " | " + _sessionName + " | identity/account/manage/changepassword - OnPostAsync - User " + userId + " changed the password successfully."} ");
            _logger.LogInformation($"User '{userId}' changed the password successfully.");
            StatusMessage = _localizer["A palavra-passe foi alterada."];
            _notify.Success(StatusMessage);

            return RedirectToPage(new { userid = userId });
        }


        //---------------------------------------------------------------------------------------------------

    }
}