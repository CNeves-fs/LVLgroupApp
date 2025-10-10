using Core.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LVLgroupApp.Areas.Dashboard.Models.DashboardTops
{
    public class TopClienteViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public int TotalClaims { get; set; }

        public int TotalClaimsAceites { get; set; }

        public int TotalClaimsNaoAceites { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}