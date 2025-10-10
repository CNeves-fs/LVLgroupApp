using Core.Entities.Artigos;
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
    public class GenderRepository : IGenderRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Gender> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public GenderRepository(IDistributedCache distributedCache, IRepositoryAsync<Gender> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Gender> Genders => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Gender gender)
        {
            await _repository.DeleteAsync(gender);
            await _distributedCache.RemoveAsync(GenderCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(GenderCacheKeys.GetKey(gender.Id));
            await _distributedCache.RemoveAsync(GenderCacheKeys.GetNomeKey(gender.Nome));
            await _distributedCache.RemoveAsync(GenderCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Gender> GetByIdAsync(int genderId)
        {
            return await _repository.Entities.Where(g => g.Id == genderId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Gender> GetByNomeAsync(string nome)
        {
            return await _repository.Entities.Where(g => g.Nome == nome).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Gender>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Gender gender)
        {
            await _repository.AddAsync(gender);
            await _distributedCache.RemoveAsync(GenderCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(GenderCacheKeys.GetKey(gender.Id));
            await _distributedCache.RemoveAsync(GenderCacheKeys.GetNomeKey(gender.Nome));
            await _distributedCache.RemoveAsync(GenderCacheKeys.SelectListKey);
            return gender.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Gender gender)
        {
            await _repository.UpdateAsync(gender);
            await _distributedCache.RemoveAsync(GenderCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(GenderCacheKeys.GetKey(gender.Id));
            await _distributedCache.RemoveAsync(GenderCacheKeys.GetNomeKey(gender.Nome));
            await _distributedCache.RemoveAsync(GenderCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------

    }
}