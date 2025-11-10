using Core.Features.Logs.Commands.AddActivityLog;
using Core.Interfaces.Shared;
using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : BasePageModel<LogoutModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IAuthenticatedUserService _userService;

        private readonly IStringLocalizer<LogoutModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public LogoutModel(
            SignInManager<ApplicationUser> signInManager, 
            ILogger<LogoutModel> logger,
            IMediator mediator,
            IAuthenticatedUserService userService,
            IStringLocalizer<LogoutModel> localizer)
        {
            _signInManager = signInManager;
            _userService = userService;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGet()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out.");
            return RedirectToPage("/Index");
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _mediator.Send(new AddActivityLogCommand() { userId = _userService.UserId, Action = "Logged Out" });
            await _signInManager.SignOutAsync();
            _notify.Information(_localizer["Utilizador terminou sessão."]);
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}