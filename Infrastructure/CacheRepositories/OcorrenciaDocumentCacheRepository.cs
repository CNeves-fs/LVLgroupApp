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
    public class OcorrenciaDocumentCacheRepository : IOcorrenciaDocumentCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IOcorrenciaDocumentRepository _ocorrenciaDocumentRepository;


        //---------------------------------------------------------------------------------------------------


        public OcorrenciaDocumentCacheRepository(IDistributedCache distributedCache, IOcorrenciaDocumentRepository ocorrenciaDocumentRepository)
        {
            _distributedCache = distributedCache;
            _ocorrenciaDocumentRepository = ocorrenciaDocumentRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<OcorrenciaDocument>> GetCachedListAsync()
        {
            string cacheKey = OcorrenciaDocumentCacheKeys.ListKey;
            var ocorrenciaDocumentList = await _distributedCache.GetAsync<List<OcorrenciaDocument>>(cacheKey);
            if (ocorrenciaDocumentList == null)
            {
                ocorrenciaDocumentList = await _ocorrenciaDocumentRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, ocorrenciaDocumentList);
            }
            return ocorrenciaDocumentList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<OcorrenciaDocument> GetByIdAsync(int ocorrenciaDocumentId)
        {
            string cacheKey = OcorrenciaDocumentCacheKeys.GetKey(ocorrenciaDocumentId);
            var ocorrenciaDocument = await _distributedCache.GetAsync<OcorrenciaDocument>(cacheKey);
            if (ocorrenciaDocument == null)
            {
                ocorrenciaDocument = await _ocorrenciaDocumentRepository.GetByIdAsync(ocorrenciaDocumentId);
                if (ocorrenciaDocument == null) return null;
                await _distributedCache.SetAsync(cacheKey, ocorrenciaDocument);
            }
            return ocorrenciaDocument;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<OcorrenciaDocument> GetByFileNameAsync(string fileName)
        {
            string cacheKey = OcorrenciaDocumentCacheKeys.GetFileNameKey(fileName);
            var ocorrenciaDocument = await _distributedCache.GetAsync<OcorrenciaDocument>(cacheKey);
            if (ocorrenciaDocument == null)
            {
                ocorrenciaDocument = await _ocorrenciaDocumentRepository.GetByFileNameAsync(fileName);
                if (ocorrenciaDocument == null) return null;
                await _distributedCache.SetAsync(cacheKey, ocorrenciaDocument);
            }
            return ocorrenciaDocument;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<OcorrenciaDocument>> GetByOcorrenciaIdAsync(int? ocorrenciaId)
        {
            string cacheKey = OcorrenciaDocumentCacheKeys.ListFromOcorrenciaKey((int)ocorrenciaId);
            var ocorrenciaDocumentList = await _distributedCache.GetAsync<List<OcorrenciaDocument>>(cacheKey);
            if (ocorrenciaDocumentList == null)
            {
                ocorrenciaDocumentList = await _ocorrenciaDocumentRepository.GetListFromOcorrenciaIdAsync(ocorrenciaId);
                if (ocorrenciaDocumentList == null) return null;
                await _distributedCache.SetAsync(cacheKey, ocorrenciaDocumentList);
            }
            return ocorrenciaDocumentList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<OcorrenciaDocument>> GetByFolderAsync(string folder)
        {
            string cacheKey = OcorrenciaDocumentCacheKeys.ListFromFolderKey(folder);
            var ocorrenciaDocumentList = await _distributedCache.GetAsync<List<OcorrenciaDocument>>(cacheKey);
            if (ocorrenciaDocumentList == null)
            {
                ocorrenciaDocumentList = await _ocorrenciaDocumentRepository.GetListFromFolderAsync(folder);
                if (ocorrenciaDocumentList == null) return null;
                await _distributedCache.SetAsync(cacheKey, ocorrenciaDocumentList);
            }
            return ocorrenciaDocumentList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}