using Core.Abstractions;
using Core.Entities.Identity;
using Core.Enums;

namespace Core.Entities.Ocorrencias
{
    public class NotificacaoOcorrencia : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public NotificationDestinationType TipoDestino { get; set; }

        public string ApplicationUserId { get; set; }           // utilizador a notificar (ocorrencia)

        public string ApplicationUserEmail { get; set; }        // utilizador a notificar (ocorrencia)



        public virtual TipoOcorrencia TipoOcorrencia { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
