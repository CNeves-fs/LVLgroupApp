using System;

namespace Core.Features.Ocorrencias.Response
{
    public class OcorrenciaCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string CodeId { get; set; }  // Format : YYYYMMDD-OCORR-LLLL-XXXXX

        public DateTime DataOcorrencia { get; set; }

        public string EmailAutor { get; set; }

        public DateTime DataEntradaSistemaOcorrencia { get; set; }





        public int CategoriaId { get; set; }

        public int InterOcorrenciaId { get; set; }          // interlojas

        public bool MasterOcorrencia { get; set; }          // interlojas

        public int TipoOcorrenciaId { get; set; }

        public int StatusId { get; set; }






        public string OcorrenciaNome { get; set; }

        public string Descrição { get; set; }

        public string Comentário { get; set; }

        public int TotalFicheiros { get; set; }

        public string OcorrenciaFolder { get; set; }





        public int EmpresaId { get; set; }

        public int GrupolojaId { get; set; }

        public int LojaId { get; set; }

        public int MercadoId { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}