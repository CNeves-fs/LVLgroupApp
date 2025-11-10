using Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Report.Models.Report
{
    public class ReportListViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int ReportTemplateId { get; set; }

        public string ReportTemplateName { get; set; }

        public int ReportTemplateVersion { get; set; }

        public bool ReportTemplateIsActive { get; set; }

        public int ReportTypeId { get; set; }

        public string ReportTypeName { get; set; }

        public string EmailAutor { get; set; }

        public DateTime ReportDate { get; set; }

        public bool IncluirWeather { get; set; }

        public Weather Weather { get; set; }

        public string Language { get; set; }

        public string Observacoes { get; set; }




        public int EmpresaId { get; set; }

        public string EmpresaNome { get; set; }

        public string EmpresaLogo { get; set; }

        public int GrupolojaId { get; set; }

        public string GrupolojaNome { get; set; }

        public int LojaId { get; set; }

        public string LojaNome { get; set; }

        public int MercadoId { get; set; }

        public string MercadoNome { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}