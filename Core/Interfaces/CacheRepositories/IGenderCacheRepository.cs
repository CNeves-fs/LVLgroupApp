using Core.Entities.Artigos;
using Core.Entities.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IGenderCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Gender>> GetCachedListAsync();

        Task<Gender> GetByIdAsync(int genderId);

        Task<Gender> GetByNomeAsync(string nome);


        //---------------------------------------------------------------------------------------------------

    }
}