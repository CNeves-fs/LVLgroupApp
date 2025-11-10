using Core.Abstractions;
using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Entities.Clientes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Claims
{
    public class Claim : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        // Cabeçalho, Tab Claim e Gestão

        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        public string CodeId { get; set; }  // Format : YYYYMMDD-EEE-LLLL-XXXX

        [Required]
        public DateTime DataClaim { get; set; }

        [Required]
        public string EmailAutor { get; set; }

        [Required]
        public DateTime DataEntradaSistemaClaim { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int PreviousStatusId { get; set; }

        [Required]
        public string MotivoClaim { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public int GrupolojaId { get; set; }

        [Required]
        public int LojaId { get; set; }

        [Required]
        public DateTime DataLimite { get; set; }

        public int MaxDiasDecisão { get; set; }             // defenido por grupoLoja

        public bool FechoClaimEmLoja { get; set; }     // defenido por Loja


        //Tab Aprovação


        public string DecisãoFinal { get; set; }

        public bool Rejeitada { get; set; }

        public bool Trocadireta { get; set; }

        public bool DevoluçãoDinheiro { get; set; }

        public bool Reparação { get; set; }

        public bool NotaCrédito { get; set; }

        public DateTime DataDecisão { get; set; }

        public string EmailAutorDecisão { get; set; }

        public string ObservaçõesFecho { get; set; }

        public DateTime DataFecho { get; set; }

        public string EmailAutorFechoEmLoja { get; set; }


        // Tab Artigo

        public int? ArtigoId { get; set; }

        [Required]
        public string Tamanho { get; set; }

        public int TamanhoId { get; set; }

        [Required]
        public DateTime DataCompra { get; set; }

        [Required]
        public string DefeitoDoArtigo { get; set; }


        // Tab Opiniões

        public int? ParecerResponsavelId { get; set; }

        public int? ParecerColaboradorId { get; set; }

        public int? ParecerGerenteLojaId { get; set; }

        public int? ParecerSupervisorId { get; set; }

        public int? ParecerRevisorId { get; set; }

        public int? ParecerAdministraçãoId { get; set; }


        // Tab Fotos

        [Required]
        public int TotalFotos { get; set; }

        // Tab Cliente
        [Required]
        public int ClienteId { get; set; }



 











        public virtual Loja Loja { get; set; }

        public virtual Artigo Artigo { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Status Status { get; set; }

        public virtual Parecer ParecerResponsavel { get; set; }

        public virtual Parecer ParecerColaborador { get; set; }

        public virtual Parecer ParecerGerenteLoja { get; set; }

        public virtual Parecer ParecerSupervisor { get; set; }

        public virtual Parecer ParecerRevisor { get; set; }

        public virtual Parecer ParecerAdministração { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}
