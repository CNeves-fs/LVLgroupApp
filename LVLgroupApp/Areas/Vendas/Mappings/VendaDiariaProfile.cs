using AutoMapper;
using Core.Features.VendasDiarias.Commands.Create;
using Core.Features.VendasDiarias.Commands.Update;
using Core.Features.VendasDiarias.Response;
using LVLgroupApp.Areas.Vendas.Models.VendaDiaria;

namespace LVLgroupApp.Areas.Vendas.Mappings
{
    internal class VendaDiariaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public VendaDiariaProfile()
        {
            CreateMap<VendaDiariaCachedResponse, VendaDiariaViewModel>().ReverseMap();
            CreateMap<CreateVendaDiariaCommand, VendaDiariaViewModel>().ReverseMap();
            CreateMap<UpdateVendaDiariaCommand, VendaDiariaViewModel>().ReverseMap();
            CreateMap<FastUpdateVendaDiariaCommand, VendaDiariaViewModel>().ReverseMap();

            CreateMap<VendaDiariaCachedResponse, VendaDiariaListViewModel>().ReverseMap();
            CreateMap<CreateVendaDiariaCommand, VendaDiariaListViewModel>().ReverseMap();
            CreateMap<UpdateVendaDiariaCommand, VendaDiariaListViewModel>().ReverseMap();
            CreateMap<FastUpdateVendaDiariaCommand, VendaDiariaListViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}