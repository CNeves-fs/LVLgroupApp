using Core.Entities.Business;
using System;

namespace Core.Features.Claims.Response
{
    public class ClaimCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string CodeId { get; set; }  // Format : YYYYMMDD-EEE-LLLL-XXXX

        public DateTime DataClaim { get; set; }

        public DateTime DataEntradaSistemaClaim { get; set; }

        public int StatusId { get; set; }

        public int PreviousStatusId { get; set; }

        public string MotivoClaim { get; set; }

        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public DateTime DataLimite { get; set; }

        public string EmailAutor { get; set; }

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

        public string Tamanho { get; set; }

        public int TamanhoId { get; set; }

        public DateTime DataCompra { get; set; }

        public string DefeitoDoArtigo { get; set; }

        // Tab Opiniões

        public int? ParecerResponsavelId { get; set; }

        public int? ParecerColaboradorId { get; set; }

        public int? ParecerGerenteLojaId { get; set; }

        public int? ParecerSupervisorId { get; set; }

        public int? ParecerRevisorId { get; set; }

        public int? ParecerAdministraçãoId { get; set; }

        // Tab Fotos

        public int TotalFotos { get; set; }

        // Tab Cliente

        public int ClienteId { get; set; }

        public int MaxDiasDecisão { get; set; }

        public bool FechoClaimEmLoja { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}