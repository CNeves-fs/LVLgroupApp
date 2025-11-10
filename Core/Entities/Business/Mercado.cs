using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Business
{
    public class Mercado : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [MaxLength(4)]
        public string NomeCurto { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}