using System.ComponentModel.DataAnnotations;

namespace Core.Features.TiposOcorrenciasLocalized.Response
{
    public class TipoOcorrenciaLocalizedCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public int TipoOcorrenciaId { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}