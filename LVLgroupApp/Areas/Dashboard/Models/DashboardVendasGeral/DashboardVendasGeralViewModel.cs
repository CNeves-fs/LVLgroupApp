using Core.Entities.Charts;
using Core.Entities.Identity;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardVendasGeral
{
    public class DashboardVendasGeralViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public CurrentRole CurrentRole { get; set; }




        public DateTime DataDashboard { get; set; }

        public DateTime AA_DataDashboard { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public string MesLiteral { get; set; }

        public int Dia { get; set; }

        public int NumeroDaSemana { get; set; }




        public SelectList Mercados { get; set; }

        public SelectList Empresas { get; set; }

        public SelectList GruposLojas { get; set; }

        public SelectList Lojas { get; set; }




        // totais unidades ano, mes, dia e semana
        public int TotalUnidadesAno { get; set; }

        public int TotalUnidadesMes { get; set; }

        public int TotalUnidadesDia { get; set; }

        public int TotalUnidadesSemana { get; set; }




        // totais unidades ano anterior
        public int AA_TotalUnidadesAno { get; set; }

        public int AA_TotalUnidadesMes { get; set; }

        public int AA_TotalUnidadesDia { get; set; }

        public int AA_TotalUnidadesSemana { get; set; }




        // totais vendas ano, mes, dia e semana
        public double TotalVendasAno { get; set; }

        public double TotalVendasMes { get; set; }

        public double TotalVendasDia { get; set; }

        public double TotalVendasSemana { get; set; }




        // totais vendas ano anterior
        public double AA_TotalVendasAno { get; set; }

        public double AA_TotalVendasMes { get; set; }

        public double AA_TotalVendasDia { get; set; }

        public double AA_TotalVendasSemana { get; set; }




        // objetivos vendas ano, mes, dia e semana
        public double ObjetivoVendasAno { get; set; }

        public double ObjetivoVendasMes { get; set; }

        public double ObjetivoVendasDia { get; set; }

        public double ObjetivoVendasSemana { get; set; }




        public List<VendasQuarterViewModel> VendasPorTrimestreList { get; set; }

        public List<VendasMonthViewModel> VendasPorMesList { get; set; }

        public List<VendasDayViewModel> VendasPorSemanaList { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}