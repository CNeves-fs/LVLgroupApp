using Core.Entities.Business;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MercadoRepository : IMercadoRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Mercado> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public MercadoRepository(IDistributedCache distributedCache, IRepositoryAsync<Mercado> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Mercado> Mercados => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Mercado mercado)
        {
            await _repository.DeleteAsync(mercado);
            await _distributedCache.RemoveAsync(MercadoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(MercadoCacheKeys.GetKey(mercado.Id));
            await _distributedCache.RemoveAsync(MercadoCacheKeys.GetNomeKey(mercado.Nome));
            await _distributedCache.RemoveAsync(MercadoCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Mercado> GetByIdAsync(int mercadoId)
        {
            return await _repository.Entities.Where(e => e.Id == mercadoId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Mercado> GetByNomeAsync(string nome)
        {
            return await _repository.Entities.Where(e => e.Nome == nome).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Mercado>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Mercado mercado)
        {
            await _repository.AddAsync(mercado);
            await _distributedCache.RemoveAsync(MercadoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(MercadoCacheKeys.GetKey(mercado.Id));
            await _distributedCache.RemoveAsync(MercadoCacheKeys.GetNomeKey(mercado.Nome));
            await _distributedCache.RemoveAsync(MercadoCacheKeys.SelectListKey);
            return mercado.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Mercado mercado)
        {
            await _repository.UpdateAsync(mercado);
            await _distributedCache.RemoveAsync(MercadoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(MercadoCacheKeys.GetKey(mercado.Id));
            await _distributedCache.RemoveAsync(MercadoCacheKeys.GetNomeKey(mercado.Nome));
            await _distributedCache.RemoveAsync(MercadoCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------

    }
}