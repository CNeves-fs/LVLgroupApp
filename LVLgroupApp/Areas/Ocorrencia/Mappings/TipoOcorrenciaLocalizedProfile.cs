using AutoMapper;
using Core.Features.TiposOcorrenciasLocalized.Commands.Create;
using Core.Features.TiposOcorrenciasLocalized.Commands.Update;
using Core.Features.TiposOcorrenciasLocalized.Response;
using LVLgroupApp.Areas.Ocorrencia.Models.TipoOcorrencia;

namespace LVLgroupApp.Areas.Ocorrencia.Mappings
{
    internal class TipoOcorrenciaLocalizedProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaLocalizedProfile()
        {
            CreateMap<TipoOcorrenciaLocalizedCachedResponse, TipoOcorrenciaLocalizedViewModel>().ReverseMap();
            CreateMap<CreateTipoOcorrenciaLocalizedCommand, TipoOcorrenciaLocalizedViewModel>().ReverseMap();
            CreateMap<UpdateTipoOcorrenciaLocalizedCommand, TipoOcorrenciaLocalizedViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}