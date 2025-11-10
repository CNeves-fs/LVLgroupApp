using Core.Interfaces.Shared;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LVLgroupApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {

        //---------------------------------------------------------------------------------------------------


        private IHttpContextAccessor _httpContextAccessor;


        //---------------------------------------------------------------------------------------------------


        public string? UserId { get { return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier); } }

        public string? Username { get { return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name); } }

        public string? UserEmail { get { return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email); } }


        //---------------------------------------------------------------------------------------------------


        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        //---------------------------------------------------------------------------------------------------

    }
}