using Core.Entities.Business;
using Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Reports
{
    public class Report

    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public int ReportTemplateId { get; set; }

        [Required]
        public string EmailAutor { get; set; }

        [Required]
        public DateTime ReportDate { get; set; }

        public bool IncluirWeather { get; set; }

        public Weather Weather { get; set; }

        [Required]
        public string Language { get; set; }

        [StringLength(256)]
        public string Observacoes { get; set; }




        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public int GrupolojaId { get; set; }

        [Required]
        public int LojaId { get; set; }

        [Required]
        public int MercadoId { get; set; }




        //public ICollection<ReportAnswer> Answers { get; set; }




        public virtual ReportTemplate ReportTemplate { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual Grupoloja Grupoloja { get; set; }

        public virtual Loja Loja { get; set; }

        public virtual Mercado Mercado { get; set; }





        //---------------------------------------------------------------------------------------------------

    }
}
