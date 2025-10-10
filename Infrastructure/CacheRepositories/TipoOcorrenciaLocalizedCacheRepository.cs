using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Ocorrencias;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class TipoOcorrenciaLocalizedCacheRepository : ITipoOcorrenciaLocalizedCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly ITipoOcorrenciaLocalizedRepository _tipoOcorrenciaLocalizedRepository;


        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaLocalizedCacheRepository(IDistributedCache distributedCache, ITipoOcorrenciaLocalizedRepository tipoOcorrenciaLocalizedRepository)
        {
            _distributedCache = distributedCache;
            _tipoOcorrenciaLocalizedRepository = tipoOcorrenciaLocalizedRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrenciaLocalized>> GetCachedListAsync()
        {
            string cacheKey = TipoOcorrenciaLocalizedCacheKeys.ListKey;
            var tipoOcorrenciaLocalizedList = await _distributedCache.GetAsync<List<TipoOcorrenciaLocalized>>(cacheKey);
            if (tipoOcorrenciaLocalizedList == null)
            {
                tipoOcorrenciaLocalizedList = await _tipoOcorrenciaLocalizedRepository.GetListAsync();
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalizedList);
            }
            return tipoOcorrenciaLocalizedList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<TipoOcorrenciaLocalized> GetByIdAsync(int tipoOcorrenciaLocalizedId)
        {
            string cacheKey = TipoOcorrenciaLocalizedCacheKeys.GetKey(tipoOcorrenciaLocalizedId);
            var tipoOcorrenciaLocalized = await _distributedCache.GetAsync<TipoOcorrenciaLocalized>(cacheKey);
            if (tipoOcorrenciaLocalized == null)
            {
                tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedRepository.GetByIdAsync(tipoOcorrenciaLocalizedId);
                if (tipoOcorrenciaLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalized);
            }
            return tipoOcorrenciaLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<TipoOcorrenciaLocalized> GetByNameAsync(string name)
        {
            string cacheKey = TipoOcorrenciaLocalizedCacheKeys.GetNameKey(name);
            var tipoOcorrenciaLocalized = await _distributedCache.GetAsync<TipoOcorrenciaLocalized>(cacheKey);
            if (tipoOcorrenciaLocalized == null)
            {
                tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedRepository.GetByNameAsync(name);
                if (tipoOcorrenciaLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalized);
            }
            return tipoOcorrenciaLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrenciaLocalized>> GetByLanguageAsync(string language)
        {
            string cacheKey = TipoOcorrenciaLocalizedCacheKeys.GetLanguageKey(language);
            var tipoOcorrenciaLocalized = await _distributedCache.GetAsync<List<TipoOcorrenciaLocalized>>(cacheKey);
            if (tipoOcorrenciaLocalized == null)
            {
                tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedRepository.GetByLanguageAsync(language);
                if (tipoOcorrenciaLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalized);
            }
            return tipoOcorrenciaLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrenciaLocalized>> GetByTipoOcorrenciaIdAsync(int tipoOcorrenciaId)
        {
            string cacheKey = TipoOcorrenciaLocalizedCacheKeys.ListFromTipoOcorrenciaKey(tipoOcorrenciaId);
            var tipoOcorrenciaLocalizedList = await _distributedCache.GetAsync<List<TipoOcorrenciaLocalized>>(cacheKey);
            if (tipoOcorrenciaLocalizedList == null)
            {
                tipoOcorrenciaLocalizedList = await _tipoOcorrenciaLocalizedRepository.GetListFromTipoOcorrenciaIdAsync(tipoOcorrenciaId);
                if (tipoOcorrenciaLocalizedList == null) return null;
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalizedList);
            }
            return tipoOcorrenciaLocalizedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}