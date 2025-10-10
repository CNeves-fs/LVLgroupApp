using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web;

namespace LVLgroupApp.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : BasePageModel<DeletePersonalDataModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<DeletePersonalDataModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<DeletePersonalDataModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        [BindProperty]
        public InputModel Input { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class InputModel
        {
            public string Id { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }


        //---------------------------------------------------------------------------------------------------


        public bool RequirePassword { get; set; }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGet(string userId = "")
        {
            if (userId == null || userId == "")
            {
                var errorMsg1 = $"{_localizer["Não foi possível carregar o utilizador."]}";
                _notify.Error(errorMsg1);
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/deletepersonaldata - OnGet - " + errorMsg1);
                return Redirect("/Home/Home/NotFound?msg=" + HttpUtility.UrlEncode(errorMsg1));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                var errorMsg2 = $"{_localizer["Não foi possível carregar o utilizador com o ID"]} {userId}.";
                _notify.Error(errorMsg2);
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/deletepersonaldata - OnGet - "} {errorMsg2}");
                return Redirect("/Home/Home/NotFound?msg=" + HttpUtility.UrlEncode(errorMsg2));
            }

            Input = new InputModel
            {
                Id = userId
            };
            RequirePassword = await _userManager.HasPasswordAsync(user);
            _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/deletepersonaldata - OnGet - return Page");
            return Page();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostAsync()
        {
            var userId = Input.Id;
            if (userId == null || userId == "")
            {
                var errorMsg1 = $"{_localizer["Não foi possível carregar o utilizador."]}";
                _notify.Error(errorMsg1);
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/deletepersonaldata - OnGet - " + errorMsg1);
                return Redirect("/Home/Home/NotFound?msg=" + HttpUtility.UrlEncode(errorMsg1));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                var errorMsg2 = $"{_localizer["Não foi possível carregar o utilizador com o ID"]} {userId}.";
                _notify.Error(errorMsg2);
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/deletepersonaldata - OnGet - "} {errorMsg2}");
                return Redirect("/Home/Home/NotFound?msg=" + HttpUtility.UrlEncode(errorMsg2));
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, _localizer["Incorrect password."]);
                    _notify.Error(_localizer["Incorrect password."]);
                    Input = new InputModel
                    {
                        Id = userId
                    };
                    _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/deletepersonaldata - OnPostAsync - return Page - Incorrect password");
                    return Page();
                }
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError($"{_localizer["Ocorreu um erro inesperado ao remover o utilizador com o ID"]} '{userId}'.");
                _notify.Error($"{_localizer["Ocorreu um erro inesperado ao remover o utilizador com o ID"]} '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }


        //---------------------------------------------------------------------------------------------------

    }
}