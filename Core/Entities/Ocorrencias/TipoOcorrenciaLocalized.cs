using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Ocorrencias
{
    public class TipoOcorrenciaLocalized : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public string Language { get; set; }

        [StringLength(100)]
        public string Name { get; set; }



        public virtual TipoOcorrencia TipoOcorrencia { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
