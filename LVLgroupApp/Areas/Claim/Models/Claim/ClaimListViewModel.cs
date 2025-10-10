using LVLgroupApp.Areas.Claim.Models.Prazolimite;
using LVLgroupApp.Areas.Claim.Models.Status;
using System;

namespace LVLgroupApp.Areas.Claim.Models.Claim
{
    public class ClaimListViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string CodeId { get; set; }  // Format : YYYYMMDD-EEE-LLLL-XXXX

        public DateTime DataClaim { get; set; }

        public int StatusId { get; set; }

        public StatusViewModel Status { get; set; }

        public int EmpresaId { get; set; }

        public string EmpresaLogo { get; set; }

        public string EmpresaNome { get; set; }

        public int GrupolojaId { get; set; }

        public string GrupolojaNome { get; set; }

        public int LojaId { get; set; }

        public string LojaNome { get; set; }

        public DateTime DataLimite { get; set; }

        public int NumeroDiasParaFecho { get; set; }

        public PrazolimiteViewModel Prazolimite { get; set; }

        public int ClienteId { get; set; }

        public string NomeCliente { get; set; }

        public string TelefoneCliente { get; set; }

        public DateTime DataUltimoContacto { get; set; }

        public int ArtigoId { get; set; }

        public string RefArtigo { get; set; }

        public string DefeitoDoArtigo { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}