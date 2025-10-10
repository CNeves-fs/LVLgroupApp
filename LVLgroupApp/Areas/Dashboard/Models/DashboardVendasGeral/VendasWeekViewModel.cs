using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasGeral
{
    public class VendasWeekViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public string NumeroDaSemana { get; set; }

        public int Quantidade { get; set; }

        public double Valor { get; set; }

        public double Objetivo { get; set; }

        public double Variacao { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}