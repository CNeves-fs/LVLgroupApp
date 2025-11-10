using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Artigos
{
    public class Gender : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        public string TamanhosNum { get; set; }

        [Required]
        public string TamanhosAlf { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
