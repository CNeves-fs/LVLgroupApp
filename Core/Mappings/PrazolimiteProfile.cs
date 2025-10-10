using AutoMapper;
using Core.Entities.Claims;
using Core.Features.Prazoslimite.Commands.Create;
using Core.Features.Prazoslimite.Queries.GetAllPaged;
using Core.Features.Prazoslimite.Queries.GetById;
using Core.Features.Prazoslimite.Response;

namespace Core.Mappings
{
    internal class PrazolimiteProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public PrazolimiteProfile()
        {
            CreateMap<CreatePrazolimiteCommand, Prazolimite>().ReverseMap();
            CreateMap<PrazolimiteCachedResponse, Prazolimite>().ReverseMap();

        }


        //---------------------------------------------------------------------------------------------------

    }
}