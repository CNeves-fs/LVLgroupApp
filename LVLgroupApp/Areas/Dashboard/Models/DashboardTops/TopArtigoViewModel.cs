using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardTops
{
    public class TopArtigoViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Referencia { get; set; }

        public string EmpresaLogo { get; set; }

        public int? GenderId { get; set; }

        public int TotalClaims { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}