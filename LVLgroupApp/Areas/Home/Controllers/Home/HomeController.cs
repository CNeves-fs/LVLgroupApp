using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Areas.Home.Models.Error;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Home.Controllers.Home
{
    [Area("Home")]
    public class HomeController : BaseController<HomeController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<HomeController> _localizer;

        private readonly SignInManager<ApplicationUser> _signInManager;


        //---------------------------------------------------------------------------------------------------


        public HomeController(IStringLocalizer<HomeController> localizer, SignInManager<ApplicationUser> signInManager)
        {
            _localizer = localizer;
            _signInManager = signInManager;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> IndexAsync()
        {
            var userId = _signInManager.UserManager.GetUserId(User);
            var currentUser = await _signInManager.UserManager.FindByIdAsync(userId);
            HttpContext.Session.SetString("SessionName", currentUser.Email);

            _logger.LogInformation(_sessionId + "Home Contoller - Index - Início de ssessão");
            _notify.Information(_localizer["Wellcome!"]);
            return View();
        }


        //---------------------------------------------------------------------------------------------------


        //[Route("error/{code}")]
        public IActionResult Error(int statusCode)
        {
            var response = new ApiResponse.ApiResponse(statusCode);
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var featuer = Request.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();//=null
            var model = new ErrorViewModel()
            {
                ErrorCode = response.StatusCode.ToString(),
                ErrorMessage = response.Message,
                OriginalPath = featuer?.OriginalPath,
                RedirectUrl = HttpContext.Request.GetDisplayUrl(),
                RequestId = requestId,
                ShowRequestId = !string.IsNullOrEmpty(requestId)
            };
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------


        //[Route("notfound/{msg}")]
        public IActionResult NotFound(string msg)
        {
            var response = new ApiResponse.ApiResponse(404, msg);
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var featuer = Request.HttpContext.Features.Get<IStatusCodeReExecuteFeature>();//=null
            var model = new ErrorViewModel()
            {
                ErrorCode = response.StatusCode.ToString(),
                ErrorMessage = response.Message,
                OriginalPath = featuer?.OriginalPath,
                RedirectUrl = HttpContext.Request.GetDisplayUrl(),
                RequestId = requestId,
                ShowRequestId = !string.IsNullOrEmpty(requestId)
            };
            return View("Error", model);
        }


        //---------------------------------------------------------------------------------------------------

    }
}