using Core.Entities.Identity;
using System;

namespace LVLgroupApp.Areas.Vendas.Models.VendaSemanal
{
    public class VendaSemanalComparaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public VendaSemanalViewModel AnoAtual { get; set; }

        public VendaSemanalViewModel AnoAnterior { get; set; }

        public CurrentRole CurrentRole { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}