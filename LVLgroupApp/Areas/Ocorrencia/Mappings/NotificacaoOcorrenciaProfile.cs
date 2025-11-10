using AutoMapper;
using Core.Features.NotificacoesOcorrencias.Commands.Create;
using Core.Features.NotificacoesOcorrencias.Commands.Update;
using Core.Features.NotificacoesOcorrencias.Response;
using LVLgroupApp.Areas.Ocorrencia.Models.TipoOcorrencia;

namespace LVLgroupApp.Areas.Ocorrencia.Mappings
{
    internal class NotificacaoOcorrenciaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public NotificacaoOcorrenciaProfile()
        {
            CreateMap<NotificacaoOcorrenciaCachedResponse, NotificacaoOcorrenciaViewModel>().ReverseMap();
            CreateMap<CreateNotificacaoOcorrenciaCommand, NotificacaoOcorrenciaViewModel>().ReverseMap();
            CreateMap<UpdateNotificacaoOcorrenciaCommand, NotificacaoOcorrenciaViewModel>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}