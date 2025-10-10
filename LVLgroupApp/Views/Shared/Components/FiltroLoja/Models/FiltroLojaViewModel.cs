using Core.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace LVLgroupApp.Views.Shared.Components.FiltroLoja.Models
{
    public class FiltroLojaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public CurrentRole CurrentRole { get; set; }

        public SelectList Empresas { get; set; }

        public SelectList Mercados { get; set; }

        public SelectList GruposLojas { get; set; }

        public SelectList Lojas { get; set; }

        public string ApplyFiltroLoja_Function { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}