using Core.Entities.Business;
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
    public class FototagRepository : IFototagRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Fototag> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public FototagRepository(IDistributedCache distributedCache, IRepositoryAsync<Fototag> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Fototag> Fototags => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Fototag fototag)
        {
            await _repository.DeleteAsync(fototag);
            await _distributedCache.RemoveAsync(FototagCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(FototagCacheKeys.GetKey(fototag.Id));
            await _distributedCache.RemoveAsync(FototagCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Fototag> GetByIdAsync(int fototagId)
        {
            return await _repository.Entities.Where(f => f.Id == fototagId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Fototag>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Fototag fototag)
        {
            await _repository.AddAsync(fototag);
            await _distributedCache.RemoveAsync(FototagCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(FototagCacheKeys.GetKey(fototag.Id));
            await _distributedCache.RemoveAsync(FototagCacheKeys.SelectListKey);
            return fototag.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Fototag fototag)
        {
            await _repository.UpdateAsync(fototag);
            await _distributedCache.RemoveAsync(FototagCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(FototagCacheKeys.GetKey(fototag.Id));
            await _distributedCache.RemoveAsync(FototagCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------

    }
}