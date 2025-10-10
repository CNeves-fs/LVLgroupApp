using Core.Entities.Business;
using System.ComponentModel.DataAnnotations;

namespace Core.Features.Prazoslimite.Response
{
    public class PrazolimiteCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Alarme { get; set; }

        public int LimiteMin { get; set; }

        public int LimiteMax { get; set; }

        public string Cortexto { get; set; }

        public string Corfundo { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}