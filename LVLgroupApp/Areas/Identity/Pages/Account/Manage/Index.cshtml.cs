using Core.Entities.Identity;
using LVLgroupApp.Abstractions;
using LVLgroupApp.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LVLgroupApp.Areas.Identity.Pages.Account.Manage
{
    public partial class ManageProfileModel : BasePageModel<ManageProfileModel>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly UserManager<ApplicationUser> _userManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IStringLocalizer<ManageProfileModel> _localizer;


        //---------------------------------------------------------------------------------------------------


        public ManageProfileModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<ManageProfileModel> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
        }


        //---------------------------------------------------------------------------------------------------


        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class InputModel
        {
            public string Id { get; set; }

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            public string UserName { get; set; }

            [Phone]
            public string PhoneNumber { get; set; }

            public byte[] ProfilePicture { get; set; }

            public int RemoveProfilePicture { get; set; } = 0; //flag [0 = don't remove, >0 remove]
        }


        //---------------------------------------------------------------------------------------------------


        private async Task LoadAsync(ApplicationUser user)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var profilePicture = user.ProfilePicture;
            Username = userName;

            Input = new InputModel
            {
                Id = userId,
                PhoneNumber = phoneNumber,
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                ProfilePicture = profilePicture,
                RemoveProfilePicture = 0
            };
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnGetAsync(string userId = "")
        {
            //if (userId == null || userId == "")
            if (string.IsNullOrEmpty(userId))
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/index - OnGetAsync - Não foi possível carregar o utilizador");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/index - OnGetAsync - Não foi possível carregar o utilizador com o ID"} '{userId}'.");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }
            await LoadAsync(user);
            return Page();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<IActionResult> OnPostAsync(string userId = "")
        {
            if (userId == null || userId == "")
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador."]}");
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/index - OnPostAsync - Não foi possível carregar o utilizador");
                return NotFound($"{_localizer["Não foi possível carregar o utilizador."]}");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _notify.Error($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
                _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/index - OnPostAsync - Não foi possível carregar o utilizador com o ID"} '{userId}'.");
                //return NotFound($"{_localizer["Não foi possível carregar o utilizador com o ID"]} '{userId}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                _logger.LogError(_sessionId + " | " + _sessionName + " | identity/account/manage/index - OnPostAsync - Model not valid");
                return Page();
            }

            var currentUserId = _signInManager.UserManager.GetUserId(User);
            var currentUser = await _signInManager.UserManager.FindByIdAsync(currentUserId);

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = _localizer["Erro: Ocorreu um erro inesperado ao tentar definir o numero de telefone."];
                    _notify.Error(StatusMessage);
                    _logger.LogError($"{_sessionId + " | " + _sessionName + " | identity/account/manage/index - OnPostAsync - Erro: Ocorreu um erro inesperado ao tentar definir o numero de telefone."}");
                    return RedirectToPage();
                }
            }
            var firstName = user.FirstName;
            var lastName = user.LastName;
            if (Input.FirstName != firstName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.LastName != lastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                user.ProfilePicture = file.OptimizeImageSize(720, 720);
                await _userManager.UpdateAsync(user);
            }
            else
            {
                // verificar se profile picture deve ser removido
                if (Input.RemoveProfilePicture > 0)
                {
                    // remover profilePicture
                    user.ProfilePicture = null;
                    await _userManager.UpdateAsync(user);
                }
            }
            if (userId == currentUserId) await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation($"{_sessionId + " | " + _sessionName + " | identity/account/manage/index - OnPostAsync - O perfil foi atualizado com sucesso."}");
            StatusMessage = _localizer["O perfil foi atualizado com sucesso."];
            _notify.Success(StatusMessage);
            return Redirect(HttpContext.Request.Headers["Referer"]);
            //return RedirectToPage();
        }


        //---------------------------------------------------------------------------------------------------

    }
}