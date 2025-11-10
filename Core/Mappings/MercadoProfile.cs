using AutoMapper;
using Core.Entities.Business;
using Core.Features.Mercados.Commands.Create;
using Core.Features.Mercados.Commands.Update;
using Core.Features.Mercados.Response;

namespace Core.Mappings
{
    internal class MercadoProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public MercadoProfile()
        {
            CreateMap<CreateMercadoCommand, Mercado>().ReverseMap();
            CreateMap<UpdateMercadoCommand, Mercado>().ReverseMap();
            CreateMap<MercadoCachedResponse, Mercado>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}