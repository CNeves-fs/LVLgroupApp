using Core.Entities.Business;

namespace Core.Features.Gruposlojas.Response
{
    public class GrupolojasCachedResponse
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }

        public string Nome { get; set; }

        public int EmpresaId { get; set; }

        public int MaxDiasDecisão { get; set; }



        //public string SupervisorId { get; set; }


        //---------------------------------------------------------------------------------------------------

    }
}