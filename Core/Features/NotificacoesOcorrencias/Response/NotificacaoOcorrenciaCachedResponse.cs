using Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.NotificacoesOcorrencias.Response
{
    public class NotificacaoOcorrenciaCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public NotificationDestinationType TipoDestino { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserEmail { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}