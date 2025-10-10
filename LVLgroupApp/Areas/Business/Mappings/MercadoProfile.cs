using AutoMapper;
using Core.Features.Mercados.Commands.Create;
using Core.Features.Mercados.Commands.Update;
using Core.Features.Mercados.Response;
using LVLgroupApp.Areas.Business.Models.Mercado;

namespace LVLgroupApp.Areas.Business.Mappings
{
    internal class MercadoProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public MercadoProfile()
        {
            CreateMap<MercadoCachedResponse, MercadoViewModel>().ReverseMap();
            CreateMap<CreateMercadoCommand, MercadoViewModel>().ReverseMap();
            CreateMap<UpdateMercadoCommand, MercadoViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}