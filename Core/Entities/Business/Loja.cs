using Core.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Business
{
    public class Loja : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [MaxLength(4)]
        public string NomeCurto { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        public string País { get; set; }

        [Required]
        public bool FechoClaimEmLoja { get; set; }

        [Required]
        public int GrupolojaId { get; set; }

        [Required]
        public int? MercadoId { get; set; }

        [Required]
        public bool Active { get; set; }


        public virtual Grupoloja Grupoloja { get; set; }

        public virtual Mercado Mercado { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}