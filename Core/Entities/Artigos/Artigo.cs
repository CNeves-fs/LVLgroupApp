using Core.Abstractions;
using Core.Entities.Business;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Artigos
{
    public class Artigo : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        //[Required]
        //[StringLength(20)]
        public string Referencia { get; set; }

        //[Required]
        public int EmpresaId { get; set; }

        public int? GenderId { get; set; }

        [StringLength(20)]
        public string Pele { get; set; }

        [StringLength(20)]
        public string Cor { get; set; }



        public virtual Empresa Empresa { get; set; }

        public virtual Gender Gender { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
