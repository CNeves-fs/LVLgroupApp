using AutoMapper;
using Core.Features.Empresas.Commands.Create;
using Core.Features.Empresas.Commands.Update;
using Core.Features.Empresas.Queries.GetById;
using Core.Features.Empresas.Response;
using LVLgroupApp.Areas.Business.Models.Empresa;

namespace LVLgroupApp.Areas.Business.Mappings
{
    internal class EmpresaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public EmpresaProfile()
        {
            CreateMap<EmpresaCachedResponse, EmpresaViewModel>().ReverseMap();
            CreateMap<CreateEmpresaCommand, EmpresaViewModel>().ReverseMap();
            CreateMap<UpdateEmpresaCommand, EmpresaViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}