using Core.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasTops
{
    public class DashboardVendasTopsViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public CurrentRole CurrentRole { get; set; }




        public DateTime DataInicialDaSemana { get; set; }

        public DateTime DataFinalDaSemana { get; set; }

        public int AnoDashboard { get; set; }

        public int SemanaDashboard { get; set; }




        // totais vendas semana LVLgroupApp/Portugal/Espanha/Canárias
        public double LVL_TotalVendasSemana { get; set; }

        public double PT_TotalVendasSemana { get; set; }

        public double ES_TotalVendasSemana { get; set; }

        public double CAN_TotalVendasSemana { get; set; }




        public SelectList Mercados { get; set; }

        public SelectList Empresas { get; set; }

        public SelectList GruposLojas { get; set; }

        public SelectList AllGruposLojas { get; set; }

        public SelectList Lojas { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}