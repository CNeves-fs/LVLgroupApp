using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Claims
{
    public class Status : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int Tipo { get; set; }

        [StringLength(80)]
        public string Texto { get; set; }

        [StringLength(20)]
        public string Cortexto { get; set; }

        [StringLength(20)]
        public string Corfundo { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
