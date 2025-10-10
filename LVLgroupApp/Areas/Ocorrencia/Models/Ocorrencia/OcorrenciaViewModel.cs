using Core.Entities.Identity;
using LVLgroupApp.Areas.Ocorrencia.Models.OcorrenciaDocument;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace LVLgroupApp.Areas.Ocorrencia.Models.Ocorrencia
{
    public class OcorrenciaViewModel
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string CodeId { get; set; }  // Format : YYYYMMDD-OCORR-LLLL-XXXXX

        public DateTime DataOcorrencia { get; set; }

        public string EmailAutor { get; set; }

        public DateTime DataEntradaSistemaOcorrencia { get; set; }





        public int CategoriaId { get; set; }

        public string CategoriaNome { get; set; }

        public int InterOcorrenciaId { get; set; }          // interlojas

        public bool MasterOcorrencia { get; set; }          // interlojas

        public int TipoOcorrenciaId { get; set; }

        public int StatusId { get; set; }

        public string StatusNome { get; set; }









        public string OcorrenciaNome { get; set; }

        public string Descrição { get; set; }

        public string Comentário { get; set; }

        public int TotalFicheiros { get; set; }

        public string OcorrenciaFolder { get; set; }






        public SelectList Categorias { get; set; }

        public SelectList Tipos { get; set; }

        public SelectList Status { get; set; }

        public byte[] Logo { get; set; }

        public List<string> Years { get; set; }

        public bool EditMode { get; set; }

        public CurrentRole CurrentRole { get; set; }

        public OcorrenciaDocumentViewModel OcorrenciaDocument { get; set; }

        
        
        
        
        public int VendaSemanalId { get; set; }

        public Double ObjetivoDaVendaSemanal { get; set; } = 0;

        public Double ValorTotalDaVenda { get; set; } = 0;

        public Double ObjetivoMensalDaVenda { get; set; } = 0;

        public Double ValorTotalMensalDaVenda { get; set; } = 0;

        public Double ValorAcumuladoMensal { get; set; } = 0;

        public int Ano { get; set; }

        public int Mes { get; set; }

        public string MesLiteral { get; set; }

        public int NumeroDaSemana { get; set; }






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



        public int ToEmpresaId { get; set; }

        public int ToGrupolojaId { get; set; }

        public int ToLojaId { get; set; }

        public int ToMercadoId { get; set; }




        //---------------------------------------------------------------------------------------------------

    }
}