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
    public class ConfirmEmailModel : BasePageModel<ConfirmEmailModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IStringLocalizer<ConfirmEmailModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ConfirmEmailModel(
            UserManager<ApplicationUser> userManager, 
            IStringLocalizer<ConfirmEmailModel> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [TempData]
        public string StatusMessage { get; set; }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"{_localizer["Não é possível carregar o utilizador com o email"]} '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? _localizer["Obrigado por confirmar o seu email."] : _localizer["Erro ao confirmar o seu email."];
            return Page();
        }


        //---------------------------------------------------------------------------------------------------

    }
}