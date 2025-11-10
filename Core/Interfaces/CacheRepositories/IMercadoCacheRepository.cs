using Core.Entities.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IMercadoCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Mercado>> GetCachedListAsync();

        Task<Mercado> GetByIdAsync(int mercadoId);

        Task<Mercado> GetByNomeAsync(string nome);


        //---------------------------------------------------------------------------------------------------

    }
}