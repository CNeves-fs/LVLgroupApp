using AutoMapper;
using Core.Entities.Identity;
using LVLgroupApp.Areas.Admin.Models;

namespace LVLgroupApp.Areas.Admin.Mappings
{
    public class UserProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<ApplicationUser, SelectUserViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}