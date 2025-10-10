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
    public class ReportTypeLocalizedRepository : IReportTypeLocalizedRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<ReportTypeLocalized> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ReportTypeLocalizedRepository(IDistributedCache distributedCache, IRepositoryAsync<ReportTypeLocalized> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<ReportTypeLocalized> ReportTypesLocalized => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTypeLocalized>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportTypeLocalized> GetByIdAsync(int reportTypeLocalizedId)
        {
            return await _repository.Entities.Where(rtl => rtl.Id == reportTypeLocalizedId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTypeLocalized>> GetListFromReportTypeIdAsync(int reportTypeId)
        {
            return await _repository.Entities.Where(rtl => rtl.ReportTypeId == reportTypeId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTypeLocalized>> GetByLanguageAsync(string language)
        {
            return await _repository.Entities.Where(rtl => rtl.Language == language).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportTypeLocalized> GetByNameAsync(string name)
        {
            return await _repository.Entities.Where(rtl => rtl.Name == name).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(ReportTypeLocalized reportTypeLocalized)
        {
            await _repository.AddAsync(reportTypeLocalized);
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetKey(reportTypeLocalized.Id));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetNameKey(reportTypeLocalized.Name));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetListLanguageKey(reportTypeLocalized.Language));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.ListFromReportTypeKey(reportTypeLocalized.ReportTypeId));

            return reportTypeLocalized.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(ReportTypeLocalized reportTypeLocalized)
        {
            await _repository.DeleteAsync(reportTypeLocalized);
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetKey(reportTypeLocalized.Id));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetNameKey(reportTypeLocalized.Name));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetListLanguageKey(reportTypeLocalized.Language));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.ListFromReportTypeKey(reportTypeLocalized.ReportTypeId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(ReportTypeLocalized reportTypeLocalized)
        {
            await _repository.UpdateAsync(reportTypeLocalized);
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetKey(reportTypeLocalized.Id));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetNameKey(reportTypeLocalized.Name));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.GetListLanguageKey(reportTypeLocalized.Language));
            await _distributedCache.RemoveAsync(ReportTypeLocalizedCacheKeys.ListFromReportTypeKey(reportTypeLocalized.ReportTypeId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}