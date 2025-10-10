using Core.Entities.Identity;
using LVLgroupApi.Areas.Identity.Models;
using LVLgroupApi.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace LVLgroupApi.Areas.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : BaseController<IdentityController>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IConfiguration _configuration;


        //---------------------------------------------------------------------------------------------------


        public IdentityController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }


        //---------------------------------------------------------------------------------------------------


        [AllowAnonymous] // Permite acesso sem autenticação
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            var userName = model.Email;

            // Check if email is valid
            if (!IsValidEmail(model.Email))
            {
                return BadRequest("Invalid email address.");
            }

            try
            {
                var userCheck = await _userManager.FindByEmailAsync(model.Email);
                // Check if user exists
                if (userCheck == null)
                {
                    return BadRequest("Invalid email or password.");
                }
                else
                {
                    userName = userCheck.UserName;
                }

                var user = await _userManager.FindByNameAsync(userName);
                // Check if user exists
                if (user == null)
                {
                    return BadRequest("Invalid email or password.");
                }

                // Check if user is active and email is confirmed
                if (!user.IsActive || !user.EmailConfirmed)
                {
                    return BadRequest("Invalid email address.");
                }

                var result = await _signInManager.PasswordSignInAsync(userName, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var accessToken = GenerateJWTToken(user);
                    var refreshToken = GenerateRefreshToken();

                    // Save the refresh token in the database
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = GetExpirationTime();
                    await _userManager.UpdateAsync(user);

                    return Ok(new
                    {
                        accessToken,
                        refreshToken,
                        //userId = user.Id,
                        //UserRole = user.RoleName,
                        message = "Login successful!"
                    });
                }
                else
                {
                    return BadRequest("Invalid email or password.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Utilizador não identificado.");
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Utilizador não encontrado.");
                }

                // Invalida o refresh token
                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = DateTime.MinValue;
                await _userManager.UpdateAsync(user);

                return Ok("Logout realizado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar logout.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }


        //---------------------------------------------------------------------------------------------------


        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RefreshToken))
                {
                    return BadRequest("Refresh token not provided.");
                }

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
                if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return Unauthorized("Invalid or expired refresh token.");
                }

                // Gera novos tokens
                var newAccessToken = GenerateJWTToken(user);
                var newRefreshToken = GenerateRefreshToken();

                // Atualiza o refresh token no banco de dados
                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = GetExpirationTime();
                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken,
                    message = "Token renewed successfully!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during token refresh.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria um token JWT para o utilizador user
        /// </summary>
        /// <returns>string</returns>

        internal string GenerateJWTToken(ApplicationUser user)
        {
            try
            {
                // Check if the email is null
                var userEmail = user.Email;
                if (string.IsNullOrEmpty(userEmail))
                {
                    throw new ArgumentNullException("User email cannot be null.");
                }

                // Check if the JWT secret is configured
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new InvalidOperationException("JWT_Secret is not configured in ApplicationSettings.");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, userEmail),
                    new System.Security.Claims.Claim("UserId", user.Id),
                    new System.Security.Claims.Claim("RoleName", user.RoleName),
                    new System.Security.Claims.Claim("Local", user.Local),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                // Set the expiration time for the token
                var expiration = GetExpirationTime();

                var jwtToken = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],      // "LVLgroupApi"
                    audience: jwtSettings["Audience"],  // "LVLgroupApiUsers"
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: expiration,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                );
                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred during JWT token generation: " + ex.Message);
                return string.Empty;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// cria um refresh token JWT para o utilizador user
        /// </summary>
        /// <returns>string</returns>

        internal string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// verifica se a string passada em emailaddress é um email válido
        /// </summary>
        /// <returns>bool</returns>

        internal bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        //---------------------------------------------------------------------------------------------------


        /// <summary>
        /// Calcula o exiration time de um token
        /// </summary>
        /// <returns>DateTime</returns>

        internal DateTime GetExpirationTime()
        {
            try
            {
                // Get the expiration time for the token
                var expiration = DateTime.UtcNow;
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var durationInMinutes = jwtSettings["DurationInMinutes"];
                
                if (string.IsNullOrEmpty(durationInMinutes))
                {
                    // default value = 1 hour
                    return DateTime.UtcNow.Add(TimeSpan.FromMinutes(60));
                }

                if (int.TryParse(durationInMinutes, out int duration))
                {
                    return DateTime.UtcNow.Add(TimeSpan.FromMinutes(duration));
                }

                throw new FormatException("Invalid duration format in JWT settings.");
            }
            catch (FormatException)
            {
                return DateTime.UtcNow;
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}
