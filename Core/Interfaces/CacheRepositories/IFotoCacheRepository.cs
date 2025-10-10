using Core.Entities.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IFotoCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Foto>> GetCachedListAsync();

        Task<List<Foto>> GetByClaimCachedListAsync(int claimId);

        Task<List<Foto>> GetByTempFolderCachedListAsync(string tempFolderGuid);

        Task<List<Foto>> GetAllTempFolderCachedListAsync();

        Task<Foto> GetByIdAsync(int fotoId);


        //---------------------------------------------------------------------------------------------------

    }
}