using AutoMapper;
using Core.Features.Fototags.Commands.Create;
using Core.Features.Fototags.Commands.Update;
using Core.Features.Fototags.Queries.GetAllCached;
using Core.Features.Fototags.Queries.GetById;
using Core.Features.Fototags.Response;
using LVLgroupApp.Areas.Claim.Models.Fototag;

namespace LVLgroupApp.Areas.Claim.Mappings
{
    internal class FototagProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public FototagProfile()
        {
            CreateMap<FototagCachedResponse, FototagViewModel>().ReverseMap();
            CreateMap<CreateFototagCommand, FototagViewModel>().ReverseMap();
            CreateMap<UpdateFototagCommand, FototagViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}