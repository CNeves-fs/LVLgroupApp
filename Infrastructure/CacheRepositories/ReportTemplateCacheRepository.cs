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
    public class ReportTemplateCacheRepository : IReportTemplateCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IReportTemplateRepository _reportTemplateRepository;


        //---------------------------------------------------------------------------------------------------


        public ReportTemplateCacheRepository(IDistributedCache distributedCache, IReportTemplateRepository reportTemplateRepository)
        {
            _distributedCache = distributedCache;
            _reportTemplateRepository = reportTemplateRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplate>> GetCachedListAsync()
        {
            string cacheKey = ReportTemplateCacheKeys.ListKey;
            var reportTemplateList = await _distributedCache.GetAsync<List<ReportTemplate>>(cacheKey);
            if (reportTemplateList == null)
            {
                reportTemplateList = await _reportTemplateRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, reportTemplateList);
            }
            return reportTemplateList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplate>> GetCachedAllReportTemplateAsync()
        {
            string cacheKey = ReportTemplateCacheKeys.AllReportTemplateListKey;
            var reportTemplateList = await _distributedCache.GetAsync<List<ReportTemplate>>(cacheKey);
            if (reportTemplateList == null)
            {
                reportTemplateList = await _reportTemplateRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, reportTemplateList);
            }
            return reportTemplateList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplate>> GetCachedAllActiveReportTemplateAsync()
        {
            string cacheKey = ReportTemplateCacheKeys.AllActiveReportTemplateListKey;
            var reportTemplateList = await _distributedCache.GetAsync<List<ReportTemplate>>(cacheKey);
            if (reportTemplateList == null)
            {
                reportTemplateList = await _reportTemplateRepository.GetListActiveAsync();
                await _distributedCache.SetAsync(cacheKey, reportTemplateList);
            }
            return reportTemplateList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<ReportTemplate>> GetByReportTypeIdCachedListAsync(int reportTypeId)
        {
            string cacheKey = ReportTemplateCacheKeys.ListReportTemplateByReportTypeIdKey(reportTypeId);
            var reportTemplateList = await _distributedCache.GetAsync<List<ReportTemplate>>(cacheKey);
            if (reportTemplateList == null)
            {
                reportTemplateList = await _reportTemplateRepository.GetListFromReportTypeIdAsync(reportTypeId);
                await _distributedCache.SetAsync(cacheKey, reportTemplateList);
            }
            return reportTemplateList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportTemplate> GetByIdAsync(int reportTemplateId)
        {
            string cacheKey = ReportTemplateCacheKeys.GetKey(reportTemplateId);
            var reportTemplate = await _distributedCache.GetAsync<ReportTemplate>(cacheKey);
            if (reportTemplate == null)
            {
                reportTemplate = await _reportTemplateRepository.GetByIdAsync(reportTemplateId);
                if (reportTemplate == null) return null;
                await _distributedCache.SetAsync(cacheKey, reportTemplate);
            }
            return reportTemplate;
        }


        //---------------------------------------------------------------------------------------------------

    }
}