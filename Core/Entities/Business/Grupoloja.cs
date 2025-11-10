using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Business
{
    public class Grupoloja : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public int MaxDiasDecisão { get; set; } = 15;

        [Required]
        public int EmpresaId { get; set; }



        public virtual Empresa Empresa { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}