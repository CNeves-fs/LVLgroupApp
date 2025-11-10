using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Text;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailChangeModel : BasePageModel<ConfirmEmailChangeModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<ConfirmEmailChangeModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ConfirmEmailChangeModel(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<ConfirmEmailChangeModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [TempData]
        public string StatusMessage { get; set; }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"{_localizer["Não é possível carregar o utilizador com o email"]} '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                StatusMessage = _localizer["Erro ao alterar o email."];
                return Page();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                StatusMessage = _localizer["Erro ao alterar o nome do utilizador."];
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = _localizer["Obrigado por confirmar a alteração do seu email."];
            return Page();
        }


        //---------------------------------------------------------------------------------------------------

    }
}