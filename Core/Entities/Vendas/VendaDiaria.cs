using Core.Abstractions;
using Core.Entities.Business;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Vendas
{
    [Index(nameof(LojaId), nameof(Ano), nameof(Mês), nameof(DiaDoMês), IsUnique = true)]
    [Index(nameof(VendaSemanalId), nameof(DiaDaSemana), IsUnique = true)]
    [Index(nameof(GrupolojaId), IsUnique = false)]
    [Index(nameof(EmpresaId), IsUnique = false)]
    [Index(nameof(MercadoId), IsUnique = false)]
    public class VendaDiaria : AuditableEntity
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        [Required]
        public int VendaSemanalId { get; set; }

        [Required]
        public int LojaId { get; set; }

        [Required]
        public int GrupolojaId { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public int MercadoId { get; set; }

        [Required]
        [Range(2010, 2100)]
        public int Ano { get; set; }

        [Required]
        [Range(1, 12)]
        public int Mês { get; set; }

        [Required]
        [Range(1, 31)]
        public int DiaDoMês { get; set; }

        [Required]
        [Range(1, 7)]
        public int DiaDaSemana { get; set; } 

        [Required]
        public DateTime DataDaVenda { get; set; } 

        [Required]
        public Double ValorDaVenda { get; set; } = 0;

        [Required]
        public int TotalArtigos { get; set; } = 0;

        [Required]
        public Double PercentConv { get; set; } = 0;

        [Required]
        public Weather Weather { get; set; }

        public string Observacoes { get; set; }





        public virtual VendaSemanal VendaSemanal { get; set; }

        public virtual Loja Loja { get; set; }

        public virtual Grupoloja Grupoloja { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual Mercado Mercado { get; set; }



        //---------------------------------------------------------------------------------------------------

    }
}