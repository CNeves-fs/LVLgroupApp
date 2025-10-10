using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Claim.Models.Claim
{
    public class TabClaimViewModel
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public DateTime DataClaim { get; set; } = DateTime.Now;

        [Required]
        public int NextStatusId { get; set; }

        [Required]
        public int LojaId { get; set; }

        [Required]
        public int GrupolojaId { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public string MotivoClaim { get; set; }

        //------------------//

        public SelectList NextStatus { get; set; }

        public SelectList Lojas { get; set; }

        public SelectList Gruposlojas { get; set; }

        public SelectList Empresas { get; set; }

        //------------------//

        public bool isSuperAdmin { get; set; }

        public bool isAdmin { get; set; }

        public bool isRevisor { get; set; }

        public bool isSupervisor { get; set; }

        public bool isGerenteLoja { get; set; }

        public bool isColaborador { get; set; }

        public bool isBasic { get; set; }

        //------------------//

        public bool isNewStatusAllowed { get; set; }

        public bool EditMode { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}