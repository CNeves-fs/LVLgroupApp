using AutoMapper;
using Core.Entities.Artigos;
using Core.Features.Genders.Commands.Create;
using Core.Features.Genders.Queries.GetAllPaged;
using Core.Features.Genders.Queries.GetById;
using Core.Features.Genders.Queries.GetByNome;
using Core.Features.Genders.Response;

namespace Core.Mappings
{
    internal class GenderProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public GenderProfile()
        {
            CreateMap<CreateGenderCommand, Gender>().ReverseMap();
            CreateMap<GenderCachedResponse, Gender>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}