using Core.Entities.Claims;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Core.Constants.Permissions;

namespace Infrastructure.Repositories
{
    public class StatusRepository : IStatusRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Status> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public StatusRepository(IDistributedCache distributedCache, IRepositoryAsync<Status> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Status> Status => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Status status)
        {
            await _repository.DeleteAsync(status);
            await _distributedCache.RemoveAsync(StatusCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(StatusCacheKeys.GetKey(status.Id));
            await _distributedCache.RemoveAsync(StatusCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Status> GetByIdAsync(int statusId)
        {
            return await _repository.Entities.Where(s => s.Id == statusId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Status>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Status status)
        {
            await _repository.AddAsync(status);
            await _distributedCache.RemoveAsync(StatusCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(StatusCacheKeys.GetKey(status.Id));
            await _distributedCache.RemoveAsync(StatusCacheKeys.SelectListKey);
            return status.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task UpdateAsync(Status status)
        {
            await _repository.UpdateAsync(status);
            await _distributedCache.RemoveAsync(StatusCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(StatusCacheKeys.GetKey(status.Id));
            await _distributedCache.RemoveAsync(StatusCacheKeys.SelectListKey);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Status>> GetTipoListAsync(int tipo)
        {
            return await _repository.Entities.Where(s => s.Tipo == tipo).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------

    }
}