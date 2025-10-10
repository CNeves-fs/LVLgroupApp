using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Claim.Models.Status
{
    public class ForceStatusViewModel
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public int Id { get; set; }

        [Required]
        public string CodeId { get; set; }  // Format : YYYYMMDD-EEE-LLLL-XXXX

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int NextStatusId { get; set; }

        public SelectList AllStatus { get; set; }

        public SelectList GoBackStatus { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}