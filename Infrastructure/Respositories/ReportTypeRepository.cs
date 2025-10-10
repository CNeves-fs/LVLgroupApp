using Core.Entities.Reports;
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
    public class ReportTypeRepository : IReportTypeRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<ReportType> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ReportTypeRepository(IDistributedCache distributedCache, IRepositoryAsync<ReportType> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<ReportType> ReportTypes => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportType>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportType> GetByIdAsync(int reportTypeId)
        {
            return await _repository.Entities.Where(rt => rt.Id == reportTypeId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportType> GetByDefaultNameAsync(string defaultName)
        {
            return await _repository.Entities.Where(rt => rt.DefaultName == defaultName).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(ReportType reportType)
        {
            await _repository.AddAsync(reportType);
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.GetKey(reportType.Id));
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.GetDefaultNameKey(reportType.DefaultName));

            return reportType.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(ReportType reportType)
        {
            await _repository.DeleteAsync(reportType);
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.GetKey(reportType.Id));
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.GetDefaultNameKey(reportType.DefaultName));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(ReportType reportType)
        {
            await _repository.UpdateAsync(reportType);
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.GetKey(reportType.Id));
            await _distributedCache.RemoveAsync(ReportTypeCacheKeys.GetDefaultNameKey(reportType.DefaultName));
        }


        //---------------------------------------------------------------------------------------------------

    }
}