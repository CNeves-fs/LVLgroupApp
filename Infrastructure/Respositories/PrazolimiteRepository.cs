using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PrazolimiteRepository : IPrazolimiteRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Prazolimite> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public PrazolimiteRepository(IDistributedCache distributedCache, IRepositoryAsync<Prazolimite> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Prazolimite> Prazoslimite => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Prazolimite prazolimite)
        {
            await _repository.DeleteAsync(prazolimite);
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.GetKey(prazolimite.Id));
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Prazolimite> GetByIdAsync(int prazolimiteId)
        {
            return await _repository.Entities.Where(s => s.Id == prazolimiteId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Prazolimite>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Prazolimite prazolimite)
        {
            await _repository.AddAsync(prazolimite);
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.GetKey(prazolimite.Id));
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.SelectListKey);
            return prazolimite.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Prazolimite prazolimite)
        {
            await _repository.UpdateAsync(prazolimite);
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.GetKey(prazolimite.Id));
            await _distributedCache.RemoveAsync(PrazolimiteCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Prazolimite> GetByLimiteAsync(int prazo)
        {
            return await _repository.Entities.Where(p => p.LimiteMax >= prazo && p.LimiteMin <= prazo).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------

    }
}