using AutoMapper;
using Core.Entities.Business;
using Core.Features.Empresas.Commands.Create;
using Core.Features.Empresas.Queries.GetById;
using Core.Features.Empresas.Queries.GetByNome;
using Core.Features.Empresas.Response;

namespace Core.Mappings
{
    internal class EmpresaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public EmpresaProfile()
        {
            CreateMap<CreateEmpresaCommand, Empresa>().ReverseMap();
            CreateMap<EmpresaCachedResponse, Empresa>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}