using Core.Enums;

namespace LVLgroupApp.Areas.Ocorrencia.Models.TipoOcorrencia
{
    public class NotificacaoOcorrenciaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public NotificationDestinationType TipoDestino { get; set; }

        public string ApplicationUserId { get; set; }           // utilizador a notificar (ocorrencia)

        public string ApplicationUserEmail { get; set; }        // utilizador a notificar (ocorrencia)


        //---------------------------------------------------------------------------------------------------

    }
}
