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
    public class TipoOcorrenciaRepository : ITipoOcorrenciaRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<TipoOcorrencia> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public TipoOcorrenciaRepository(IDistributedCache distributedCache, IRepositoryAsync<TipoOcorrencia> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<TipoOcorrencia> TiposOcorrencias => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrencia>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<TipoOcorrencia> GetByIdAsync(int tipoOcorrenciaId)
        {
            return await _repository.Entities.Where(to => to.Id == tipoOcorrenciaId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<TipoOcorrencia>> GetByCategoriaIdListAsync(int categoriaId)
        {
            return await _repository.Entities.Where(to => to.CategoriaId == categoriaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<TipoOcorrencia> GetByDefaultNameAsync(string defaultName)
        {
            return await _repository.Entities.Where(to => to.DefaultName == defaultName).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(TipoOcorrencia tipoocorrencia)
        {
            await _repository.AddAsync(tipoocorrencia);
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetCategoriaIdKey(tipoocorrencia.CategoriaId));
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetKey(tipoocorrencia.Id));
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetDefaultNameKey(tipoocorrencia.DefaultName));

            return tipoocorrencia.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(TipoOcorrencia tipoocorrencia)
        {
            await _repository.DeleteAsync(tipoocorrencia);
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetCategoriaIdKey(tipoocorrencia.CategoriaId));
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetKey(tipoocorrencia.Id));
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetDefaultNameKey(tipoocorrencia.DefaultName));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(TipoOcorrencia tipoocorrencia)
        {
            await _repository.UpdateAsync(tipoocorrencia);
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetCategoriaIdKey(tipoocorrencia.CategoriaId));
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetKey(tipoocorrencia.Id));
            await _distributedCache.RemoveAsync(TipoOcorrenciaCacheKeys.GetDefaultNameKey(tipoocorrencia.DefaultName));
        }


        //---------------------------------------------------------------------------------------------------

    }
}