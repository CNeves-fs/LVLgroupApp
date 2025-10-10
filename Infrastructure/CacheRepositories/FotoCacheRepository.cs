using AspNetCoreHero.Extensions.Caching;
using AspNetCoreHero.ThrowR;
using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class FotoCacheRepository : IFotoCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IFotoRepository _fotoRepository;


        //---------------------------------------------------------------------------------------------------


        public FotoCacheRepository(IDistributedCache distributedCache, IFotoRepository fotoRepository)
        {
            _distributedCache = distributedCache;
            _fotoRepository = fotoRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Foto> GetByIdAsync(int fotoId)
        {
            string cacheKey = FotoCacheKeys.GetKey(fotoId);
            var foto = await _distributedCache.GetAsync<Foto>(cacheKey);
            if (foto == null)
            {
                foto = await _fotoRepository.GetByIdAsync(fotoId);
                // Throw.Exception.IfNull(foto, "Foto", "Foto not Found");
                if (foto == null) return null;
                await _distributedCache.SetAsync(cacheKey, foto);
            }
            return foto;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Foto>> GetCachedListAsync()
        {
            string cacheKey = FotoCacheKeys.ListKey;
            var fotoList = await _distributedCache.GetAsync<List<Foto>>(cacheKey);
            if (fotoList == null)
            {
                fotoList = await _fotoRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, fotoList);
            }
            return fotoList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Foto>> GetByClaimCachedListAsync(int claimId)
        {
            string cacheKey = FotoCacheKeys.ListFromClaimKey(claimId);
            var fotoList = await _distributedCache.GetAsync<List<Foto>>(cacheKey);
            if (fotoList == null)
            {
                fotoList = await _fotoRepository.GetListFromClaimAsync(claimId);
                await _distributedCache.SetAsync(cacheKey, fotoList);
            }
            return fotoList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Foto>> GetAllTempFolderCachedListAsync()
        {
            string cacheKey = FotoCacheKeys.ListFromAllTempFolderKey;
            var fotoList = await _distributedCache.GetAsync<List<Foto>>(cacheKey);
            if (fotoList == null)
            {
                fotoList = await _fotoRepository.GetListFromAllTempFolderAsync();
                await _distributedCache.SetAsync(cacheKey, fotoList);
            }
            return fotoList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Foto>> GetByTempFolderCachedListAsync(string tempFolder)
        {
            string cacheKey = FotoCacheKeys.ListFromTempFolderKey(tempFolder);
            var fotoList = await _distributedCache.GetAsync<List<Foto>>(cacheKey);
            if (fotoList == null)
            {
                fotoList = await _fotoRepository.GetListFromTempFolderAsync(tempFolder);
                await _distributedCache.SetAsync(cacheKey, fotoList);
            }
            return fotoList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}