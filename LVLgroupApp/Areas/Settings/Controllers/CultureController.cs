using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace LVLgroupApp.Areas.Settings.Controllers
{
    [Area("Settings")]
    public class CultureController : Controller
    {

        //---------------------------------------------------------------------------------------------------


        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            //var escapedReturnUrl = System.Uri.EscapeUriString(returnUrl);
            //return LocalRedirect(escapedReturnUrl);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) 
                return Redirect(returnUrl);
            else
            {
                return Redirect("~/Identity/Account/Login");
            }

        }


        //---------------------------------------------------------------------------------------------------

    }
}
