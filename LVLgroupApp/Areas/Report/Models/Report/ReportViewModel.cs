using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Report.Models.Report
{
    public class ReportViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        public CurrentRole CurrentRole { get; set; }


        public int ReportTemplateId { get; set; }

        public string EmailAutor { get; set; }

        public DateTime ReportDate { get; set; }

        public bool IncluirWeather { get; set; }

        public Weather Weather { get; set; }

        public string Language { get; set; }

        public string Observacoes { get; set; }




        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int MercadoId { get; set; }




        public string NomeLoja { get; set; }

        public List<Core.Entities.Reports.ReportAnswer> Answers { get; set; }

        public SelectList ReportTemplateTypes { get; set; }

        public bool EditMode { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}