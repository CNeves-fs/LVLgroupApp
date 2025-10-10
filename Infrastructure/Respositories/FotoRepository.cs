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
    public class FotoRepository : IFotoRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Foto> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public FotoRepository(IDistributedCache distributedCache, IRepositoryAsync<Foto> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Foto> Fotos => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Foto foto)
        {
            await _repository.DeleteAsync(foto);
            await _distributedCache.RemoveAsync(FotoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(FotoCacheKeys.GetKey(foto.Id));
            if (foto.ClaimId != null)
            {
                await _distributedCache.RemoveAsync(FotoCacheKeys.ListFromClaimKey((int)foto.ClaimId));
            }
            if (!string.IsNullOrEmpty(foto.TempFolderGuid))
            {
                await _distributedCache.RemoveAsync(FotoCacheKeys.ListFromTempFolderKey(foto.TempFolderGuid));
            }
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Foto> GetByIdAsync(int fotoId)
        {
            return await _repository.Entities.Where(f => f.Id == fotoId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Foto>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Foto>> GetListFromClaimAsync(int claimId)
        {
            return await _repository.Entities.Where(f => (f.ClaimId == claimId)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Foto>> GetListFromAllTempFolderAsync()
        {
            return await _repository.Entities.Where(f => (!string.IsNullOrEmpty(f.TempFolderGuid))).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Foto>> GetListFromTempFolderAsync(string tempFolder)
        {
            return await _repository.Entities.Where(f => (f.TempFolderGuid == tempFolder)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Foto foto)
        {
            await _repository.AddAsync(foto);
            await _distributedCache.RemoveAsync(FotoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(FotoCacheKeys.GetKey(foto.Id));
            if (foto.ClaimId != null)
            {
                await _distributedCache.RemoveAsync(FotoCacheKeys.ListFromClaimKey((int)foto.ClaimId));
            }         
            if (!string.IsNullOrEmpty(foto.TempFolderGuid))
            {
                await _distributedCache.RemoveAsync(FotoCacheKeys.ListFromTempFolderKey(foto.TempFolderGuid));
            }
            return foto.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Foto foto)
        {
            await _repository.UpdateAsync(foto);
            await _distributedCache.RemoveAsync(FotoCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(FotoCacheKeys.GetKey(foto.Id));
            if (foto.ClaimId != null)
            {
                await _distributedCache.RemoveAsync(FotoCacheKeys.ListFromClaimKey((int)foto.ClaimId));
            }
            if (string.IsNullOrEmpty(foto.TempFolderGuid))
            {
                await _distributedCache.RemoveAsync(FotoCacheKeys.ListFromTempFolderKey(foto.TempFolderGuid));
            }
        }


        //---------------------------------------------------------------------------------------------------

    }
}