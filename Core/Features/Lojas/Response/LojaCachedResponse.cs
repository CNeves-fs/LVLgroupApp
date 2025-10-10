using Core.Entities.Business;

namespace Core.Features.Lojas.Response
{
    public class LojaCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public string NomeCurto { get; set; }

        public string Cidade { get; set; }

        public string País { get; set; }

        public int GrupolojaId { get; set; }

        public int? MercadoId { get; set; }

        public bool FechoClaimEmLoja { get; set; }

        public bool Active { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}