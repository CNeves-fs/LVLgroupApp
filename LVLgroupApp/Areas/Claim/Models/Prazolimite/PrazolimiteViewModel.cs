using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Claim.Models.Prazolimite

{
    public class PrazolimiteViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public string Alarme { get; set; }

        [Required]
        public int LimiteMin { get; set; }

        [Required]
        public int LimiteMax { get; set; }

        [Required]
        public string Cortexto { get; set; }

        [Required]
        public string Corfundo { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}