using AutoMapper;
using Core.Entities.Business;
using Core.Features.Gruposlojas.Commands.Create;
using Core.Features.Gruposlojas.Queries.GetAllPaged;
using Core.Features.Gruposlojas.Queries.GetById;
using Core.Features.Gruposlojas.Response;

namespace Core.Mappings
{
    internal class GrupolojaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public GrupolojaProfile()
        {
            CreateMap<CreateGrupolojaCommand, Grupoloja>().ReverseMap();
            CreateMap<GrupolojasCachedResponse, Grupoloja>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}