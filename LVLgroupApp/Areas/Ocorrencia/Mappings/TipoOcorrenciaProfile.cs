using AutoMapper;
using Core.Features.TiposOcorrencias.Commands.Create;
using Core.Features.TiposOcorrencias.Commands.Update;
using Core.Features.TiposOcorrencias.Response;
using LVLgroupApp.Areas.Ocorrencia.Models.TipoOcorrencia;

namespace LVLgroupApp.Areas.Ocorrencia.Mappings
{
    internal class TipoOcorrenciaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaProfile()
        {
            CreateMap<TipoOcorrenciaCachedResponse, TipoOcorrenciaViewModel>().ReverseMap();
            CreateMap<CreateTipoOcorrenciaCommand, TipoOcorrenciaViewModel>().ReverseMap();
            CreateMap<UpdateTipoOcorrenciaCommand, TipoOcorrenciaViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}