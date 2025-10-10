using AutoMapper;
using Core.Features.Claims.Commands.Create;
using Core.Features.Claims.Commands.Update;
using Core.Features.Claims.Response;
using LVLgroupApp.Areas.Claim.Models.Claim;

namespace LVLgroupApp.Areas.Claim.Mappings
{
    internal class ClaimProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ClaimProfile()
        {
            CreateMap<ClaimCachedResponse, ClaimViewModel>().ReverseMap();
            CreateMap<CreateClaimCommand, ClaimViewModel>().ReverseMap();
            CreateMap<UpdateClaimCommand, ClaimViewModel>().ReverseMap();

            CreateMap<ClaimCachedResponse, Core.Entities.Claims.Claim>().ReverseMap();
            CreateMap<CreateClaimCommand, Core.Entities.Claims.Claim>().ReverseMap();
            CreateMap<UpdateClaimCommand, Core.Entities.Claims.Claim>().ReverseMap();

            CreateMap<ClaimCachedResponse, ClaimListViewModel>().ReverseMap();
            CreateMap<CreateClaimCommand, ClaimListViewModel>().ReverseMap();
            CreateMap<UpdateClaimCommand, ClaimListViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}