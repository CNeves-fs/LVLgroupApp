using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account.Manage
{
    public class SetPasswordModel : BasePageModel<SetPasswordModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<SetPasswordModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public SetPasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<SetPasswordModel> localizer)
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
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            //[Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            //[Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetAsync(string userId = "")
        {
            if (userId == null || userId == "")
            {
                return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToPage("./ChangePassword", new { userId = userId });
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
                return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            if (!ModelState.IsValid)
            {
                Input = new InputModel
                {
                    Id = userId
                };
                return Page();
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer["A palavra-passe foi definida."];

            return RedirectToPage(new { userid = userId });
        }


        //---------------------------------------------------------------------------------------------------

    }
}