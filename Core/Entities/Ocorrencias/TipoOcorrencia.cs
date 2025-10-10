using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Ocorrencias
{
    public class TipoOcorrencia : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DefaultName { get; set; }

        [Required]
        public int CategoriaId { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
