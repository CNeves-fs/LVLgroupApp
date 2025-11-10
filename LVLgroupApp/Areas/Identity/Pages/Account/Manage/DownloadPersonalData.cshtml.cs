using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace LVLgroupApp.Areas.Identity.Pages.Account.Manage
{
    public class DownloadPersonalDataModel : BasePageModel<DownloadPersonalDataModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IStringLocalizer<DownloadPersonalDataModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public DownloadPersonalDataModel(
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<DownloadPersonalDataModel> localizer)
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


        public async Task<IActionResult> OnPostAsync()
        {
            var userId = Input.Id;
            if (userId == null || userId == "")
            {
                var errorMsg1 = $"{_localizer["Não foi possível carregar o utilizador."]}";
                _notify.Error(errorMsg1);
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/DownloadPersonalDataModel - OnPostAsync - " + errorMsg1);
                return Redirect("/Home/Home/NotFound?msg=" + HttpUtility.UrlEncode(errorMsg1));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                var errorMsg2 = $"{_localizer["Não foi possível carregar o utilizador com o ID"]} {userId}.";
                _notify.Error(errorMsg2);
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/DownloadPersonalDataModel - OnPostAsync - "} {errorMsg2}");
                return Redirect("/Home/Home/NotFound?msg=" + HttpUtility.UrlEncode(errorMsg2));
            }

            _logger.LogInformation("User with ID '{UserId}' asked for their personal data.", _userManager.GetUserId(User));

            // Only include personal data for download
            var personalData = new Dictionary<string, string>();
            var personalDataProps = typeof(ApplicationUser).GetProperties().Where(
                            prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
            }

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
            return new FileContentResult(JsonSerializer.SerializeToUtf8Bytes(personalData), "application/json");
        }


        //---------------------------------------------------------------------------------------------------

    }
}