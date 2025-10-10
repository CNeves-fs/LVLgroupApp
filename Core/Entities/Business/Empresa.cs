using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Business
{
    public class Empresa : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(3)]
        public string NomeCurto { get; set; }

        public byte[] LogoPicture { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
