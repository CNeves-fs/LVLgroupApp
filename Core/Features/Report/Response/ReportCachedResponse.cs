using Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Report.Response
{
    public class ReportCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

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


        //---------------------------------------------------------------------------------------------------

    }
}