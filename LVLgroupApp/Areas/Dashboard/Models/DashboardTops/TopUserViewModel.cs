using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardTops
{
    public class TopUserViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string Email { get; set; }

        public string Role { get; set; }

        public string Nome { get; set; }

        public string ProfilePicture { get; set; }

        public string EmpresaLogo { get; set; }

        public string Loja { get; set; }

        public int TotalClaims { get; set; }

        public int TrocasDiretas { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}