using Core.Entities.Charts;
using DocumentFormat.OpenXml.Vml;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardTops
{
    public class DashboardTopsViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int TotalClientes { get; set; }

        public int TotalArtigos { get; set; }

        public int TotalUsers { get; set; }

        public int TotalLojas { get; set; }

        public List<TopArtigoViewModel> TopArtigosList { get; set; } //Tabela Top Artigos

        public List<TopClienteViewModel> TopClientesList { get; set; } //Tabela Top Clientes

        public List<TopUserViewModel> TopUserResponsávelList { get; set; } //Tabela Top Users responsáveis

        public List<TopUserViewModel> TopUserFecharEmLojaList { get; set; } //Tabela Top Users fechar em Loja

        public List<TopLojaViewModel> TopLojasList { get; set; } //Tabela Top Lojas

        public List<TopLojaViewModel> TopTrocaDiretaList { get; set; } //Tabela Top Lojas troca direta


        //---------------------------------------------------------------------------------------------------

    }
}