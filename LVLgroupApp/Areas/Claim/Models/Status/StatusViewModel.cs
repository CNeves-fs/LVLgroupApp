using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Claim.Models.Status

{
    public class StatusViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public int Tipo { get; set; }

        [Required]
        public string Texto { get; set; }

        [Required]
        public string Cortexto { get; set; }

        [Required]
        public string Corfundo { get; set; }

        public SelectList Tipos { get; set; } // _CreateOrEdit


        //---------------------------------------------------------------------------------------------------

    }
}