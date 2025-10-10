using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Artigos;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class GenderCacheRepository : IGenderCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IGenderRepository _genderRepository;


        //---------------------------------------------------------------------------------------------------


        public GenderCacheRepository(IDistributedCache distributedCache, IGenderRepository genderRepository)
        {
            _distributedCache = distributedCache;
            _genderRepository = genderRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Gender> GetByIdAsync(int genderId)
        {
            string cacheKey = GenderCacheKeys.GetKey(genderId);
            var gender = await _distributedCache.GetAsync<Gender>(cacheKey);
            if (gender == null)
            {
                gender = await _genderRepository.GetByIdAsync(genderId);
                // Throw.Exception.IfNull(gender, "Gender", "Gender not Found");
                if (gender == null) return null;
                await _distributedCache.SetAsync(cacheKey, gender);
            }
            return gender;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Gender> GetByNomeAsync(string nome)
        {
            string cacheKey = GenderCacheKeys.GetNomeKey(nome);
            var gender = await _distributedCache.GetAsync<Gender>(cacheKey);
            if (gender == null)
            {
                gender = await _genderRepository.GetByNomeAsync(nome);
                //Throw.Exception.IfNull(gender, "Gender", "Gender not Found");
                if (gender == null) return null;
                await _distributedCache.SetAsync(cacheKey, gender);
            }
            return gender;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Gender>> GetCachedListAsync()
        {
            string cacheKey = GenderCacheKeys.ListKey;
            var genderList = await _distributedCache.GetAsync<List<Gender>>(cacheKey);
            if (genderList == null)
            {
                genderList = await _genderRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, genderList);
            }
            return genderList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}