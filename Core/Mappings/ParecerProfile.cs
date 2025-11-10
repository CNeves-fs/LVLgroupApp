using AutoMapper;
using Core.Entities.Claims;
using Core.Features.Pareceres.Commands.Create;
using Core.Features.Pareceres.Queries.GetAllPaged;
using Core.Features.Pareceres.Queries.GetById;
using Core.Features.Pareceres.Response;
using Core.Features.Pareceres.Commands.Update;

namespace Core.Mappings
{
    internal class ParecerProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public ParecerProfile()
        {
            CreateMap<CreateParecerCommand, Parecer>().ReverseMap();
            CreateMap<UpdateParecerCommand, Parecer>().ReverseMap();
            CreateMap<ParecerCachedResponse, Parecer>().ReverseMap();
 
        }


        //---------------------------------------------------------------------------------------------------

    }
}