using AutoMapper;
using Core.Features.VendasSemanais.Commands.Create;
using Core.Features.VendasSemanais.Commands.Update;
using Core.Features.VendasSemanais.Response;
using LVLgroupApp.Areas.Vendas.Models.VendaSemanal;

namespace LVLgroupApp.Areas.Vendas.Mappings
{
    internal class VendaSemanalProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public VendaSemanalProfile()
        {
            CreateMap<VendaSemanalCachedResponse, VendaSemanalViewModel>().ReverseMap();
            CreateMap<CreateVendaSemanalCommand, VendaSemanalViewModel>().ReverseMap();
            CreateMap<UpdateVendaSemanalCommand, VendaSemanalViewModel>().ReverseMap();

            CreateMap<VendaSemanalCachedResponse, VendaSemanalListViewModel>().ReverseMap();
            CreateMap<CreateVendaSemanalCommand, VendaSemanalListViewModel>().ReverseMap();
            CreateMap<UpdateVendaSemanalCommand, VendaSemanalListViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}