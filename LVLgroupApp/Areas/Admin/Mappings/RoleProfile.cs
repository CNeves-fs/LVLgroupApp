using LVLgroupApp.Areas.Admin.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace LVLgroupApp.Areas.Admin.Mappings
{
    public class RoleProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}