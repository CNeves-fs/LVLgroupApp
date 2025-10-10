using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Claims
{
    public class Prazolimite : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Alarme { get; set; }

        [Required]
        public int LimiteMin { get; set; }

        [Required]
        public int LimiteMax { get; set; }

        [StringLength(50)]
        public string Cortexto { get; set; }

        [StringLength(50)]
        public string Corfundo { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
