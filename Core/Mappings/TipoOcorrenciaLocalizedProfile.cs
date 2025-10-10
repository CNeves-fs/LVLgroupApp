using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Features.TiposOcorrenciasLocalized.Commands.Create;
using Core.Features.TiposOcorrenciasLocalized.Commands.Update;
using Core.Features.TiposOcorrenciasLocalized.Response;

namespace Core.Mappings
{
    internal class TipoOcorrenciaLocalizedProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaLocalizedProfile()
        {
            CreateMap<CreateTipoOcorrenciaLocalizedCommand, TipoOcorrenciaLocalized>().ReverseMap();
            CreateMap<UpdateTipoOcorrenciaLocalizedCommand, TipoOcorrenciaLocalized>().ReverseMap();
            CreateMap<TipoOcorrenciaLocalizedCachedResponse, TipoOcorrenciaLocalized>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}