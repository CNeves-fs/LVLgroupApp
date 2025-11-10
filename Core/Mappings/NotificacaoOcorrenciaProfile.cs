using AutoMapper;
using Core.Entities.Ocorrencias;
using Core.Features.NotificacoesOcorrencias.Commands.Create;
using Core.Features.NotificacoesOcorrencias.Commands.Update;
using Core.Features.NotificacoesOcorrencias.Response;

namespace Core.Mappings
{
    internal class NotificacaoOcorrenciaProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public NotificacaoOcorrenciaProfile()
        {
            CreateMap<CreateNotificacaoOcorrenciaCommand, NotificacaoOcorrencia>().ReverseMap();
            CreateMap<UpdateNotificacaoOcorrenciaCommand, NotificacaoOcorrencia>().ReverseMap();
            CreateMap<NotificacaoOcorrenciaCachedResponse, NotificacaoOcorrencia>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }
}