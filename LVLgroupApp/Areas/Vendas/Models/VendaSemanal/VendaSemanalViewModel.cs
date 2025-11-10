using System;
using System.Collections.Generic;
using Core.Enums;
using LVLgroupApp.Areas.Vendas.Models.VendaDiaria;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LVLgroupApp.Areas.Vendas.Models.VendaSemanal
{
    public class VendaSemanalViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public DateTime DataInicialDaSemana { get; set; }

        public DateTime DataFinalDaSemana { get; set; }



        public int NumeroDaSemana { get; set; } = 0;

        public int Mes { get; set; } = 0;  // relativo à start date

        public int Quarter { get; set; } = 0;  // relativo à start date

        public int Ano { get; set; } = 0;  // relativo à start date




        public Double Valor2Feira { get; set; } = 0;

        public Double Conv2Feira { get; set; } = 0;

        public int TotalArtigos2Feira { get; set; } = 0;

        public Weather Weather2Feira { get; set; }

        public string Observacoes2Feira { get; set; }



        public Double Valor3Feira { get; set; } = 0;

        public Double Conv3Feira { get; set; } = 0;

        public int TotalArtigos3Feira { get; set; } = 0;

        public Weather Weather3Feira { get; set; }

        public string Observacoes3Feira { get; set; }



        public Double Valor4Feira { get; set; } = 0;

        public Double Conv4Feira { get; set; } = 0;

        public int TotalArtigos4Feira { get; set; } = 0;

        public Weather Weather4Feira { get; set; }

        public string Observacoes4Feira { get; set; }



        public Double Valor5Feira { get; set; } = 0;

        public Double Conv5Feira { get; set; } = 0;

        public int TotalArtigos5Feira { get; set; } = 0;

        public Weather Weather5Feira { get; set; }

        public string Observacoes5Feira { get; set; }



        public Double Valor6Feira { get; set; } = 0;

        public Double Conv6Feira { get; set; } = 0;

        public int TotalArtigos6Feira { get; set; } = 0;

        public Weather Weather6Feira { get; set; }

        public string Observacoes6Feira { get; set; }



        public Double ValorSábado { get; set; } = 0;

        public Double ConvSab { get; set; } = 0;

        public int TotalArtigosSab { get; set; } = 0;

        public Weather WeatherSab { get; set; }

        public string ObservacoesSab { get; set; }


        public Double ValorDomingo { get; set; } = 0;

        public Double ConvDom { get; set; } = 0;

        public int TotalArtigosDom { get; set; } = 0;

        public Weather WeatherDom { get; set; }

        public string ObservacoesDom { get; set; }




        public Double ValorTotalDaVenda { get; set; } = 0;

        public Double ValorTotalDaVendaDoAnoAnterior { get; set; } = 0;

        public Double ObjetivoDaVendaSemanal { get; set; } = 0;

        public Double VariaçaoAnual { get; set; } = 0;



        public Double ObjetivoMensalDaVenda { get; set; } = 0;

        public Double ValorTotalMensalDaVenda { get; set; } = 0;

        public Double ValorAcumuladoMensal { get; set; } = 0;





        public int EmpresaId { get; set; }

        public string EmpresaNome { get; set; }

        public string EmpresaLogo { get; set; }

        public SelectList Empresas { get; set; } //_CreateOrEdit



        public int GrupolojaId { get; set; }

        public string GrupolojaNome { get; set; }

        public SelectList GruposLojas { get; set; } //_CreateOrEdit



        public int LojaId { get; set; }

        public string LojaNome { get; set; }

        public SelectList Lojas { get; set; } //_CreateOrEdit



        public int MercadoId { get; set; }

        public string MercadoNome { get; set; }

        public SelectList Mercados { get; set; } //_CreateOrEdit



        public List<string> Years { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}