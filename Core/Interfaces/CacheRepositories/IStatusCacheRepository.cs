using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IStatusCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Status>> GetCachedListAsync();

        Task<List<Status>> GetCachedTipoListAsync(int tipo);

        Task<Status> GetByIdAsync(int statusId);


        //---------------------------------------------------------------------------------------------------

    }
}