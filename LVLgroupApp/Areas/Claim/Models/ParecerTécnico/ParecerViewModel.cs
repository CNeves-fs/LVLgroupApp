using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LVLgroupApp.Areas.Claim.Models.ParecerTécnico
{
    public class ParecerViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime Data { get; set; }

        public string Email { get; set; }

        public string Opinião { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}