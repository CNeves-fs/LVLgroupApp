using Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.VendasDiarias.Response
{
    public class VendaDiariaCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int VendaSemanalId { get; set; }

        public int LojaId { get; set; }

        public int GrupolojaId { get; set; }

        public int EmpresaId { get; set; }

        public int MercadoId { get; set; }

        public int Ano { get; set; }

        public int Mês { get; set; }

        public int DiaDoMês { get; set; }

        public int DiaDaSemana { get; set; }

        public DateTime DataDaVenda { get; set; }

        public Double ValorDaVenda { get; set; }

        public Double PercentConv { get; set; }

        public int TotalArtigos { get; set; }

        public Weather Weather { get; set; }

        public string Observacoes { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}