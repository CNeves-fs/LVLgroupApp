using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Reports;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.CacheRepositories
{
    public class ReportTypeCacheRepository : IReportTypeCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IReportTypeRepository _reportTypeRepository;


        //---------------------------------------------------------------------------------------------------


        public ReportTypeCacheRepository(IDistributedCache distributedCache, IReportTypeRepository reportTypeRepository)
        {
            _distributedCache = distributedCache;
            _reportTypeRepository = reportTypeRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportType>> GetCachedListAsync()
        {
            string cacheKey = ReportTypeCacheKeys.ListKey;
            var reportTypeList = await _distributedCache.GetAsync<List<ReportType>>(cacheKey);
            if (reportTypeList == null)
            {
                reportTypeList = await _reportTypeRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, reportTypeList);
            }
            return reportTypeList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportType>> GetCachedAllReportTypeAsync()
        {
            string cacheKey = ReportTypeCacheKeys.ListKey;
            var reportTypeList = await _distributedCache.GetAsync<List<ReportType>>(cacheKey);
            if (reportTypeList == null)
            {
                reportTypeList = await _reportTypeRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, reportTypeList);
            }
            return reportTypeList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportType> GetByIdAsync(int reportTypeId)
        {
            string cacheKey = ReportTypeCacheKeys.GetKey(reportTypeId);
            var reportType = await _distributedCache.GetAsync<ReportType>(cacheKey);
            if (reportType == null)
            {
                reportType = await _reportTypeRepository.GetByIdAsync(reportTypeId);
                if (reportType == null) return null;
                await _distributedCache.SetAsync(cacheKey, reportType);
            }
            return reportType;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportType> GetByNameAsync(string defaultname)
        {
            string cacheKey = ReportTypeCacheKeys.GetDefaultNameKey(defaultname);
            var reportType = await _distributedCache.GetAsync<ReportType>(cacheKey);
            if (reportType == null)
            {
                reportType = await _reportTypeRepository.GetByDefaultNameAsync(defaultname);
                if (reportType == null) return null;
                await _distributedCache.SetAsync(cacheKey, reportType);
            }
            return reportType;
        }


        //---------------------------------------------------------------------------------------------------

    }
}