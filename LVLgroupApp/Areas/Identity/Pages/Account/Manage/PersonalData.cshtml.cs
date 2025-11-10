using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : BasePageModel<PersonalDataModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IStringLocalizer<PersonalDataModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public PersonalDataModel(
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<PersonalDataModel> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [BindProperty]
        public InputModel Input { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class InputModel
        {
            public string Id { get; set; }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGet(string userId = "")
        {
            if (userId == null || userId == "")
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/personaldata - OnGet - Não foi possível carregar o utilizador");
                return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/personaldata - OnGet - Não foi possível carregar o utilizador com o ID"} '{userId}'.");
                return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            Input = new InputModel
            {
                Id = userId
            };
            _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/personaldata - OnGet - return Page");
            return Page();
        }


        //---------------------------------------------------------------------------------------------------

    }
}