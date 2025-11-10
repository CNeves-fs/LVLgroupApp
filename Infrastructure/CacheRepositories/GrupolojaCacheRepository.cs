using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class GrupolojaCacheRepository : IGrupolojaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IGrupolojaRepository _grupolojaRepository;


        //---------------------------------------------------------------------------------------------------


        public GrupolojaCacheRepository(IDistributedCache distributedCache, IGrupolojaRepository grupolojaRepository)
        {
            _distributedCache = distributedCache;
            _grupolojaRepository = grupolojaRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Grupoloja> GetByIdAsync(int grupolojaId)
        {
            string cacheKey = GrupolojaCacheKeys.GetKey(grupolojaId);
            var grupoloja = await _distributedCache.GetAsync<Grupoloja>(cacheKey);
            if (grupoloja == null)
            {
                grupoloja = await _grupolojaRepository.GetByIdAsync(grupolojaId);
                // Throw.Exception.IfNull(grupoloja, "Grupoloja", "Grupoloja not Found");
                if (grupoloja == null) return null;
                await _distributedCache.SetAsync(cacheKey, grupoloja);
            }
            return grupoloja;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Grupoloja>> GetCachedListAsync()
        {
            string cacheKey = GrupolojaCacheKeys.ListKey;
            var grupolojaList = await _distributedCache.GetAsync<List<Grupoloja>>(cacheKey);
            if (grupolojaList == null)
            {
                grupolojaList = await _grupolojaRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, grupolojaList);
            }
            return grupolojaList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Grupoloja>> GetByEmpresaIdCachedListAsync(int empresaId)
        {
            string cacheKey = GrupolojaCacheKeys.ListFromEmpresaKey(empresaId);
            var grupolojaList = await _distributedCache.GetAsync<List<Grupoloja>>(cacheKey);
            if (grupolojaList == null)
            {
                grupolojaList = await _grupolojaRepository.GetListFromEmpresaAsync(empresaId);
                await _distributedCache.SetAsync(cacheKey, grupolojaList);
            }
            return grupolojaList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}