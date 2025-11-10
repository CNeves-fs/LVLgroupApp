using LVLgroupApp.Areas.Admin.Models;
using AutoMapper;
using System.Security.Claims;

namespace LVLgroupApp.Areas.Admin.Mappings
{
    public class ClaimsProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ClaimsProfile()
        {
            CreateMap<System.Security.Claims.Claim, RoleClaimsViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}