using AutoMapper;
using Core.Entities.Vendas;
using Core.Features.VendasDiarias.Commands.Create;
using Core.Features.VendasDiarias.Commands.Update;
using Core.Features.VendasDiarias.Response;

namespace Core.Mappings
{
    internal class VendaDiariaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public VendaDiariaProfile()
        {
            CreateMap<CreateVendaDiariaCommand, VendaDiaria>().ReverseMap();
            CreateMap<UpdateVendaDiariaCommand, VendaDiaria>().ReverseMap();
            CreateMap<FastUpdateVendaDiariaCommand, VendaDiaria>().ReverseMap();
            CreateMap<VendaDiariaCachedResponse, VendaDiaria>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}