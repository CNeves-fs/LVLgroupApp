using Core.Entities.Charts;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IClaimCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Claim>> GetCachedListAsync();

        Task<Claim> GetByIdAsync(int claimId);

        Task<List<Claim>> GetByLojaIdAsync(int lojaId);

        Task<List<Claim>> GetByGrupolojaIdAsync(int grupolojaId);

        Task<List<Claim>> GetByEmpresaIdAsync(int empresaId);


        //---------------------------------------------------------------------------------------------------

    }
}