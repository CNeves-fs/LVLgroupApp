using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IParecerCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Parecer>> GetCachedListAsync();

        Task<Parecer> GetByIdAsync(int parecerId);


        //---------------------------------------------------------------------------------------------------

    }
}