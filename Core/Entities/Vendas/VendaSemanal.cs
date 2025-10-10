using Core.Abstractions;
using Core.Entities.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Vendas
{
    [Index(nameof(LojaId), nameof(Ano), nameof(NumeroDaSemana), IsUnique = true)]
    public class VendaSemanal : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public DateTime DataInicialDaSemana { get; set; }

        [Required]
        public DateTime DataFinalDaSemana { get; set; }

        [Required]
        public int NumeroDaSemana { get; set; } = 0;

        [Required]
        [Range(1, 12)]
        public int Mes { get; set; } = 0;  // relativo à start date

        [Required]
        [Range(1, 4)]
        public int Quarter { get; set; } = 0;  // relativo à start date

        [Required]
        [Range(2010, 2100)]
        public int Ano { get; set; } = 0;  // ano a que pertence a semana

        [Required]
        public Double ValorTotalDaVenda { get; set; } = 0;

        [Required]
        public Double ValorTotalDaVendaDoAnoAnterior { get; set; } = 0;

        [Required]
        public Double ObjetivoDaVendaSemanal { get; set; } = 0;

        [Required]
        public Double VariaçaoAnual { get; set; } = 0;

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public int GrupolojaId { get; set; }

        [Required]
        public int LojaId { get; set; }

        [Required]
        public int MercadoId { get; set; }



        public virtual Empresa Empresa { get; set; }

        public virtual Grupoloja Grupoloja { get; set; }

        public virtual Loja Loja { get; set; }

        public virtual Mercado Mercado { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}