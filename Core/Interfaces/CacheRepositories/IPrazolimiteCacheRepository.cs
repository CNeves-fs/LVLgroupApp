using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IPrazolimiteCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Prazolimite>> GetCachedListAsync();

        Task<Prazolimite> GetByIdAsync(int prazolimiteId);


        //---------------------------------------------------------------------------------------------------

    }
}