using Core.Abstractions;
using Core.Entities.Business;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Ocorrencias
{
    public class Ocorrencia : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string CodeId { get; set; }  // Format : YYYYMMDD-OCORR-LLLL-XXXXX

        [Required]
        public DateTime DataOcorrencia { get; set; }

        [Required]
        public string EmailAutor { get; set; }

        [Required]
        public DateTime DataEntradaSistemaOcorrencia { get; set; }





        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public int InterOcorrenciaId { get; set; }          // interlojas

        [Required]
        public bool MasterOcorrencia { get; set; }          // interlojas

        [Required]
        public int TipoOcorrenciaId { get; set; }

        [Required]
        public int StatusId { get; set; }





        [Required]
        [StringLength(128)]
        public string OcorrenciaNome { get; set; }

        [StringLength(128)]
        public string Descrição { get; set; }

        [StringLength(128)]
        public string Comentário { get; set; }

        [Required]
        public int TotalFicheiros { get; set; }

        public string OcorrenciaFolder { get; set; }




        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public int GrupolojaId { get; set; }

        [Required]
        public int LojaId { get; set; }

        [Required]
        public int MercadoId { get; set; }





        public virtual TipoOcorrencia TipoOcorrencia { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual Grupoloja Grupoloja { get; set; }

        public virtual Loja Loja { get; set; }

        public virtual Mercado Mercado { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
