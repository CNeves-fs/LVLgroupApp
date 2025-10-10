using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LVLgroupApp.Areas.Claim.Models.ParecerTécnico
{
    public class TabPareceresViewModel
    {

        //---------------------------------------------------------------------------------------------------


        [Required]
        public ParecerViewModel ParecerResponsavel { get; set; }

        public ParecerViewModel ParecerColaborador { get; set; }

        public ParecerViewModel ParecerGerenteLoja { get; set; }

        public ParecerViewModel ParecerSupervisor { get; set; }

        public ParecerViewModel ParecerRevisor { get; set; }

        public ParecerViewModel ParecerAdministração { get; set; }

        public string RoleNameResponsável { get; set; }

        public bool IsSuperAdmin { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsRevisor { get; set; }

        public bool IsSupervisor { get; set; }

        public bool IsGerenteLoja { get; set; }

        public bool IsColaborador { get; set; }

        public bool IsBasic { get; set; }

        public bool IsResponsável { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}