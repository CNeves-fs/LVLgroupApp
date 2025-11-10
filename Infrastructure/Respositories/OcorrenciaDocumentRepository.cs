using Core.Entities.Ocorrencias;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OcorrenciaDocumentRepository : IOcorrenciaDocumentRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<OcorrenciaDocument> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public OcorrenciaDocumentRepository(IDistributedCache distributedCache, IRepositoryAsync<OcorrenciaDocument> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<OcorrenciaDocument> OcorrenciaDocuments => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<OcorrenciaDocument>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<OcorrenciaDocument> GetByIdAsync(int ocorrenciaDocumentId)
        {
            return await _repository.Entities.Where(od => od.Id == ocorrenciaDocumentId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<OcorrenciaDocument>> GetListFromOcorrenciaIdAsync(int? ocorrenciaId)
        {
            if (ocorrenciaId == null) return new List<OcorrenciaDocument>();
            return await _repository.Entities.Where(od => od.OcorrenciaId == ocorrenciaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<OcorrenciaDocument>> GetListFromFolderAsync(string folder)
        {
            return await _repository.Entities.Where(od => od.OcorrenciaFolder == folder).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<OcorrenciaDocument> GetByFileNameAsync(string filename)
        {
            return await _repository.Entities.Where(od => od.FileName == filename).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(OcorrenciaDocument ocorrenciaDocument)
        {
            await _repository.AddAsync(ocorrenciaDocument);
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.GetKey(ocorrenciaDocument.Id));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListFromOcorrenciaKey(ocorrenciaDocument.OcorrenciaId));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListFromFolderKey(ocorrenciaDocument.OcorrenciaFolder));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.GetFileNameKey(ocorrenciaDocument.FileName));

            return ocorrenciaDocument.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(OcorrenciaDocument ocorrenciaDocument)
        {
            await _repository.DeleteAsync(ocorrenciaDocument);
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.GetKey(ocorrenciaDocument.Id));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListFromOcorrenciaKey(ocorrenciaDocument.OcorrenciaId));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListFromFolderKey(ocorrenciaDocument.OcorrenciaFolder));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.GetFileNameKey(ocorrenciaDocument.FileName));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(OcorrenciaDocument ocorrenciaDocument)
        {
            await _repository.UpdateAsync(ocorrenciaDocument);
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.GetKey(ocorrenciaDocument.Id));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListFromOcorrenciaKey(ocorrenciaDocument.OcorrenciaId));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.ListFromFolderKey(ocorrenciaDocument.OcorrenciaFolder));
            await _distributedCache.RemoveAsync(OcorrenciaDocumentCacheKeys.GetFileNameKey(ocorrenciaDocument.FileName));
        }


        //---------------------------------------------------------------------------------------------------

    }
}