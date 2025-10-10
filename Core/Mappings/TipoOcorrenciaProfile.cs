using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Features.TiposOcorrencias.Commands.Create;
using Core.Features.TiposOcorrencias.Commands.Update;
using Core.Features.TiposOcorrencias.Response;

namespace Core.Mappings
{
    internal class TipoOcorrenciaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaProfile()
        {
            CreateMap<CreateTipoOcorrenciaCommand, TipoOcorrencia>().ReverseMap();
            CreateMap<UpdateTipoOcorrenciaCommand, TipoOcorrencia>().ReverseMap();
            CreateMap<TipoOcorrenciaCachedResponse, TipoOcorrencia>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}