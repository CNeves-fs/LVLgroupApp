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
    public class ReportCacheRepository : IReportCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IReportRepository _reportRepository;


        //---------------------------------------------------------------------------------------------------


        public ReportCacheRepository(IDistributedCache distributedCache, IReportRepository reportRepository)
        {
            _distributedCache = distributedCache;
            _reportRepository = reportRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Report>> GetCachedListAsync()
        {
            string cacheKey = ReportCacheKeys.ListKey;
            var reportList = await _distributedCache.GetAsync<List<Report>>(cacheKey);
            if (reportList == null)
            {
                reportList = await _reportRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, reportList);
            }
            return reportList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Report>> GetCachedAllReportAsync()
        {
            string cacheKey = ReportCacheKeys.AllReportListKey;
            var reportList = await _distributedCache.GetAsync<List<Report>>(cacheKey);
            if (reportList == null)
            {
                reportList = await _reportRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, reportList);
            }
            return reportList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<Report>> GetByLojaIdCachedListAsync(int lojaId)
        {
            string cacheKey = ReportCacheKeys.ListReportByLojaIdKey(lojaId);
            var reportList = await _distributedCache.GetAsync<List<Report>>(cacheKey);
            if (reportList == null)
            {
                reportList = await _reportRepository.GetListFromLojaIdAsync(lojaId);
                await _distributedCache.SetAsync(cacheKey, reportList);
            }
            return reportList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<Report>> GetByReportTemplateIdCachedListAsync(int reportTemplateId)
        {
            string cacheKey = ReportCacheKeys.ListReportByReportTemplateIdKey(reportTemplateId);
            var reportList = await _distributedCache.GetAsync<List<Report>>(cacheKey);
            if (reportList == null)
            {
                reportList = await _reportRepository.GetListFromReportTemplateIdAsync(reportTemplateId);
                await _distributedCache.SetAsync(cacheKey, reportList);
            }
            return reportList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Report> GetByIdAsync(int reportId)
        {
            string cacheKey = ReportCacheKeys.GetKey(reportId);
            var report = await _distributedCache.GetAsync<Report>(cacheKey);
            if (report == null)
            {
                report = await _reportRepository.GetByIdAsync(reportId);
                if (report == null) return null;
                await _distributedCache.SetAsync(cacheKey, report);
            }
            return report;
        }


        //---------------------------------------------------------------------------------------------------

    }
}