using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Core.Entities.Artigos;
using Core.Entities.Charts;
using Core.Entities.Claims;
using Core.Enums;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

namespace Infrastructure.CacheRepositories
{
    public class ClaimCacheRepository : IClaimCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IClaimRepository _claimRepository;

        private readonly IStatusCacheRepository _statusCacheRepository;


        //---------------------------------------------------------------------------------------------------


        public ClaimCacheRepository(IDistributedCache distributedCache, IClaimRepository claimRepository, IStatusCacheRepository statusCacheRepository)
        {
            _distributedCache = distributedCache;
            _claimRepository = claimRepository;
            _statusCacheRepository = statusCacheRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Claim> GetByIdAsync(int claimId)
        {
            string cacheKey = ClaimCacheKeys.GetKey(claimId);
            var claim = await _distributedCache.GetAsync<Claim>(cacheKey);
            if (claim == null)
            {
                claim = await _claimRepository.GetByIdAsync(claimId);
                // Throw.Exception.IfNull(claim, "Claim", "Claim not Found");
                if (claim == null) return null;
                await _distributedCache.SetAsync(cacheKey, claim);
            }
            return claim;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Claim>> GetByLojaIdAsync(int lojaId)
        {
            string cacheKey = ClaimCacheKeys.ListFromLojaKey(lojaId);
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListFromLojaAsync(lojaId);
                await _distributedCache.SetAsync(cacheKey, claimList);
            }
            return claimList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Claim>> GetByGrupolojaIdAsync(int grupolojaId)
        {
            string cacheKey = ClaimCacheKeys.ListFromGrupolojaKey(grupolojaId);
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListFromGrupolojaAsync(grupolojaId);
                //await _distributedCache.SetAsync(cacheKey, claimList);
            }
            return claimList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Claim>> GetByEmpresaIdAsync(int empresaId)
        {
            string cacheKey = ClaimCacheKeys.ListFromEmpresaKey(empresaId);
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListFromEmpresaAsync(empresaId);
                //await _distributedCache.SetAsync(cacheKey, claimList);
            }
            return claimList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Claim>> GetCachedListAsync()
        {
            string cacheKey = ClaimCacheKeys.ListKey;
            var claimList = await _distributedCache.GetAsync<List<Claim>>(cacheKey);
            if (claimList == null)
            {
                claimList = await _claimRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, claimList);
            }
            return claimList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}