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
    public class ParecerRepository : IParecerRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Parecer> _repository;

        private readonly IRepositoryAsync<Claim> _repositoryClaim;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ParecerRepository(IDistributedCache distributedCache, IRepositoryAsync<Parecer> repository, IRepositoryAsync<Claim> repositoryClaim)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _repositoryClaim = repositoryClaim;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Parecer> Pareceres => _repository.Entities;

        public IQueryable<Claim> Claims => _repositoryClaim.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<Parecer> GetByIdAsync(int parecerId)
        {
            return await _repository.Entities.Where(p => p.Id == parecerId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Parecer>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Parecer parecer)
        {
            await _repository.AddAsync(parecer);
            await _distributedCache.RemoveAsync(ParecerCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ParecerCacheKeys.GetKey(parecer.Id));
            await _distributedCache.RemoveAsync(ParecerCacheKeys.SelectListKey);
            return parecer.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Parecer parecer)
        {
            await _repository.DeleteAsync(parecer);
            await _distributedCache.RemoveAsync(ParecerCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ParecerCacheKeys.GetKey(parecer.Id));
            await _distributedCache.RemoveAsync(ParecerCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(Parecer parecer)
        {
            await _repository.UpdateAsync(parecer);
            await _distributedCache.RemoveAsync(ParecerCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ParecerCacheKeys.GetKey(parecer.Id));
            await _distributedCache.RemoveAsync(ParecerCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------

    }
}