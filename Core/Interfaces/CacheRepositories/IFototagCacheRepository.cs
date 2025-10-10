using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IFototagCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Fototag>> GetCachedListAsync();

        Task<Fototag> GetByIdAsync(int fototagId);


        //---------------------------------------------------------------------------------------------------

    }
}