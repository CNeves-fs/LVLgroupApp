using Core.Entities.Identity;
using LVLgroupApp.Areas.Artigo.Models.Artigo;
using LVLgroupApp.Areas.Cliente.Models.Cliente;
using LVLgroupApp.Areas.Claim.Models.Aprovação;
using LVLgroupApp.Areas.Claim.Models.Foto;
using LVLgroupApp.Areas.Claim.Models.ParecerTécnico;
using LVLgroupApp.Areas.Claim.Models.Prazolimite;
using LVLgroupApp.Areas.Claim.Models.Status;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LVLgroupApp.Areas.Claim.Models.Claim
{
    public class ClaimViewModel
    {

        //---------------------------------------------------------------------------------------------------



        // Identificação e status
        public int Id { get; set; }
        public string CodeId { get; set; }  // Format : YYYYMMDD-EEE-LLLL-XXXX
        public StatusViewModel Status { get; set; }
        public DateTime DataEntradaSistemaClaim { get; set; }
        public string EmailAutor { get; set; }
        public PrazolimiteViewModel Prazolimite { get; set; }
        public byte[] Logo { get; set; }
        public DateTime DataLimite { get; set; }
        public int NumeroDiasParaFecho { get; set; }

        // Tab Claim
        [NotMapped]
        public TabClaimViewModel Claim { get; set; }

        // Tab Aprovação
        [NotMapped]
        public TabAprovaçãoViewModel Aprovação { get; set; }

        // Tab Artigo
        [NotMapped]
        public TabArtigoViewModel Artigo { get; set; }

        // Tab Fotos
        public int TotalFotos { get; set; }
        public string ClaimFolder { get; set; }

        // Tab Opiniões
        [NotMapped]
        public TabPareceresViewModel Pareceres { get; set; }
        public int? ParecerResponsavelId { get; set; }
        public int? ParecerColaboradorId { get; set; }
        public int? ParecerGerenteLojaId { get; set; }
        public int? ParecerSupervisorId { get; set; }
        public int? ParecerRevisorId { get; set; }
        public int? ParecerAdministraçãoId { get; set; }

        // Tab Cliente
        public TabClienteViewModel Cliente { get; set; }

        // Controle e gestão
        public SelectList Artigos { get; set; }
        public int MaxDiasDecisão { get; set; }             // defenido en Grupoloja
        public bool FechoClaimEmLoja { get; set; }     // defenido en Loja
        public CurrentRole CurrentRole { get; set; }

        // extra para _ViewAll
        public string EmpresaNome { get; set; }
        public string GrupolojaNome { get; set; }
        public string LojaNome { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}