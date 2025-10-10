using AutoMapper;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Features.Lojas.Queries.GetAllPaged;
using Core.Features.Claims.Commands.Create;
using Core.Features.Claims.Queries.GetAllPaged;
using Core.Features.Claims.Queries.GetById;
using Core.Features.Claims.Response;

namespace Core.Mappings
{
    internal class ClaimProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ClaimProfile()
        {
            CreateMap<CreateClaimCommand, Claim>().ReverseMap();
            CreateMap<ClaimCachedResponse, Claim>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}