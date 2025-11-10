using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Features.Ocorrencias.Commands.Create;
using Core.Features.Ocorrencias.Commands.Update;
using Core.Features.Ocorrencias.Response;

namespace Core.Mappings
{
    internal class OcorrenciaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public OcorrenciaProfile()
        {
            CreateMap<CreateOcorrenciaCommand, Ocorrencia>().ReverseMap();
            CreateMap<UpdateOcorrenciaCommand, Ocorrencia>().ReverseMap();
            CreateMap<OcorrenciaCachedResponse, Ocorrencia>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}