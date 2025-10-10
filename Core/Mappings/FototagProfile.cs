using AutoMapper;
using Core.Entities.Claims;
using Core.Features.Fototags.Commands.Create;
using Core.Features.Fototags.Queries.GetAllPaged;
using Core.Features.Fototags.Queries.GetById;
using Core.Features.Fototags.Response;

namespace Core.Mappings
{
    internal class FototagProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public FototagProfile()
        {
            CreateMap<CreateFototagCommand, Fototag>().ReverseMap();
            CreateMap<FototagCachedResponse, Fototag>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}