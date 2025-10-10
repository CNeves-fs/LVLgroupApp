using AutoMapper;
using Core.Entities.Vendas;
using Core.Features.VendasSemanais.Commands.Create;
using Core.Features.VendasSemanais.Commands.Update;
using Core.Features.VendasSemanais.Response;

namespace Core.Mappings
{
    internal class VendaSemanalProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public VendaSemanalProfile()
        {
            CreateMap<CreateVendaSemanalCommand, VendaSemanal>().ReverseMap();
            CreateMap<UpdateVendaSemanalCommand, VendaSemanal>().ReverseMap();
            CreateMap<VendaSemanalCachedResponse, VendaSemanal>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}