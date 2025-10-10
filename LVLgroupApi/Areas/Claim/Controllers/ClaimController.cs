using Core.Entities.Identity;
using LVLgroupApi.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace LVLgroupApi.Areas.Claim.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : BaseController<ClaimController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStringLocalizer<ClaimController> _localizer;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;


        //---------------------------------------------------------------------------------------------------


        public ClaimController(
            IStringLocalizer<ClaimController> localizer,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _localizer = localizer;
            _userManager = userManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }


        //---------------------------------------------------------------------------------------------------


        [HttpGet("GetAllClaims")]
        [Authorize]
        public async Task<IActionResult> GetAllClaims()
        {
            try
            {
                // Identifica o user a partir do token
                var userId = User.FindFirst("UserId")?.Value;
                var role = User.FindFirst("RoleName")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Utilizador não identificado.");
                }

                // Verifica se o token expirou
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Utilizador não encontrado.");
                }

                if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return Unauthorized("Token expirado. Faça login novamente ou renove o token.");
                }

                // Retorna os dados com base no role do usuário
                var claims = await _context.Claims.Take(100).ToListAsync();

                return Ok(claims);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter claims.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }



        //---------------------------------------------------------------------------------------------------

    }
}
