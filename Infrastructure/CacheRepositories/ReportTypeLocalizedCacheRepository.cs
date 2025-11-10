using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Reports;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class ReportTypeLocalizedCacheRepository : IReportTypeLocalizedCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IReportTypeLocalizedRepository _tipoOcorrenciaLocalizedRepository;


        //---------------------------------------------------------------------------------------------------


        public ReportTypeLocalizedCacheRepository(IDistributedCache distributedCache, IReportTypeLocalizedRepository tipoOcorrenciaLocalizedRepository)
        {
            _distributedCache = distributedCache;
            _tipoOcorrenciaLocalizedRepository = tipoOcorrenciaLocalizedRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTypeLocalized>> GetCachedListAsync()
        {
            string cacheKey = ReportTypeLocalizedCacheKeys.ListKey;
            var tipoOcorrenciaLocalizedList = await _distributedCache.GetAsync<List<ReportTypeLocalized>>(cacheKey);
            if (tipoOcorrenciaLocalizedList == null)
            {
                tipoOcorrenciaLocalizedList = await _tipoOcorrenciaLocalizedRepository.GetListAsync();
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalizedList);
            }
            return tipoOcorrenciaLocalizedList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportTypeLocalized> GetByIdAsync(int tipoOcorrenciaLocalizedId)
        {
            string cacheKey = ReportTypeLocalizedCacheKeys.GetKey(tipoOcorrenciaLocalizedId);
            var tipoOcorrenciaLocalized = await _distributedCache.GetAsync<ReportTypeLocalized>(cacheKey);
            if (tipoOcorrenciaLocalized == null)
            {
                tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedRepository.GetByIdAsync(tipoOcorrenciaLocalizedId);
                if (tipoOcorrenciaLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalized);
            }
            return tipoOcorrenciaLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportTypeLocalized> GetByNameAsync(string name)
        {
            string cacheKey = ReportTypeLocalizedCacheKeys.GetNameKey(name);
            var tipoOcorrenciaLocalized = await _distributedCache.GetAsync<ReportTypeLocalized>(cacheKey);
            if (tipoOcorrenciaLocalized == null)
            {
                tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedRepository.GetByNameAsync(name);
                if (tipoOcorrenciaLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalized);
            }
            return tipoOcorrenciaLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTypeLocalized>> GetByLanguageAsync(string language)
        {
            string cacheKey = ReportTypeLocalizedCacheKeys.GetListLanguageKey(language);
            var tipoOcorrenciaLocalized = await _distributedCache.GetAsync<List<ReportTypeLocalized>>(cacheKey);
            if (tipoOcorrenciaLocalized == null)
            {
                tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedRepository.GetByLanguageAsync(language);
                if (tipoOcorrenciaLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalized);
            }
            return tipoOcorrenciaLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTypeLocalized>> GetByReportTypeIdAsync(int tipoOcorrenciaId)
        {
            string cacheKey = ReportTypeLocalizedCacheKeys.ListFromReportTypeKey(tipoOcorrenciaId);
            var tipoOcorrenciaLocalizedList = await _distributedCache.GetAsync<List<ReportTypeLocalized>>(cacheKey);
            if (tipoOcorrenciaLocalizedList == null)
            {
                tipoOcorrenciaLocalizedList = await _tipoOcorrenciaLocalizedRepository.GetListFromReportTypeIdAsync(tipoOcorrenciaId);
                if (tipoOcorrenciaLocalizedList == null) return null;
                //await _distributedCache.SetAsync(cacheKey, tipoOcorrenciaLocalizedList);
            }
            return tipoOcorrenciaLocalizedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}