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
    public class TipoOcorrenciaLocalizedRepository : ITipoOcorrenciaLocalizedRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<TipoOcorrenciaLocalized> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaLocalizedRepository(IDistributedCache distributedCache, IRepositoryAsync<TipoOcorrenciaLocalized> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<TipoOcorrenciaLocalized> TiposOcorrenciasLocalized => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrenciaLocalized>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<TipoOcorrenciaLocalized> GetByIdAsync(int tipoOcorrenciaLocalizedId)
        {
            return await _repository.Entities.Where(to => to.Id == tipoOcorrenciaLocalizedId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrenciaLocalized>> GetListFromTipoOcorrenciaIdAsync(int tipoocorrenciaId)
        {
            return await _repository.Entities.Where(to => to.TipoOcorrenciaId == tipoocorrenciaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrenciaLocalized>> GetByLanguageAsync(string language)
        {
            return await _repository.Entities.Where(to => to.Language == language).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<TipoOcorrenciaLocalized> GetByNameAsync(string name)
        {
            return await _repository.Entities.Where(to => to.Name == name).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(TipoOcorrenciaLocalized tipoOcorrenciaLocalized)
        {
            await _repository.AddAsync(tipoOcorrenciaLocalized);
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetKey(tipoOcorrenciaLocalized.Id));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetNameKey(tipoOcorrenciaLocalized.Name));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetLanguageKey(tipoOcorrenciaLocalized.Language));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.ListFromTipoOcorrenciaKey(tipoOcorrenciaLocalized.TipoOcorrenciaId));

            return tipoOcorrenciaLocalized.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(TipoOcorrenciaLocalized tipoOcorrenciaLocalized)
        {
            await _repository.DeleteAsync(tipoOcorrenciaLocalized);
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetKey(tipoOcorrenciaLocalized.Id));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetNameKey(tipoOcorrenciaLocalized.Name));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetLanguageKey(tipoOcorrenciaLocalized.Language));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.ListFromTipoOcorrenciaKey(tipoOcorrenciaLocalized.TipoOcorrenciaId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(TipoOcorrenciaLocalized tipoOcorrenciaLocalized)
        {
            await _repository.UpdateAsync(tipoOcorrenciaLocalized);
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetKey(tipoOcorrenciaLocalized.Id));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetNameKey(tipoOcorrenciaLocalized.Name));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.GetLanguageKey(tipoOcorrenciaLocalized.Language));
            await _distributedCache.RemoveAsync(TipoOcorrenciaLocalizedCacheKeys.ListFromTipoOcorrenciaKey(tipoOcorrenciaLocalized.TipoOcorrenciaId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}