using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardTops
{
    public class TopLojaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Loja { get; set; }

        public string Agrupamento { get; set; }

        public string EmpresaNome { get; set; }

        public string EmpresaLogo { get; set; }

        public int TotalClaims { get; set; }

        public int TrocasDiretas { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}