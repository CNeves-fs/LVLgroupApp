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
    public class ReportTemplateQuestionCacheRepository : IReportTemplateQuestionCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IReportTemplateQuestionRepository _reportTemplateQuestionRepository;


        //---------------------------------------------------------------------------------------------------


        public ReportTemplateQuestionCacheRepository(IDistributedCache distributedCache, IReportTemplateQuestionRepository reportTemplateQuestionRepository)
        {
            _distributedCache = distributedCache;
            _reportTemplateQuestionRepository = reportTemplateQuestionRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplateQuestion>> GetCachedListAsync()
        {
            string cacheKey = ReportTemplateQuestionCacheKeys.ListKey;
            var reportTemplateQuestionList = await _distributedCache.GetAsync<List<ReportTemplateQuestion>>(cacheKey);
            if (reportTemplateQuestionList == null)
            {
                reportTemplateQuestionList = await _reportTemplateQuestionRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, reportTemplateQuestionList);
            }
            return reportTemplateQuestionList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplateQuestion>> GetCachedAllReportTemplateQuestionAsync()
        {
            string cacheKey = ReportTemplateQuestionCacheKeys.AllReportTemplateQuestionListKey;
            var reportTemplateQuestionList = await _distributedCache.GetAsync<List<ReportTemplateQuestion>>(cacheKey);
            if (reportTemplateQuestionList == null)
            {
                reportTemplateQuestionList = await _reportTemplateQuestionRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, reportTemplateQuestionList);
            }
            return reportTemplateQuestionList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplateQuestion>> GetCachedAllActiveReportTemplateQuestionAsync()
        {
            string cacheKey = ReportTemplateQuestionCacheKeys.AllActiveReportTemplateQuestionListKey;
            var reportTemplateQuestionList = await _distributedCache.GetAsync<List<ReportTemplateQuestion>>(cacheKey);
            if (reportTemplateQuestionList == null)
            {
                reportTemplateQuestionList = await _reportTemplateQuestionRepository.GetListActiveAsync();
                await _distributedCache.SetAsync(cacheKey, reportTemplateQuestionList);
            }
            return reportTemplateQuestionList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<ReportTemplateQuestion>> GetByQuestionTemplateIdCachedListAsync(int questionTemplateId)
        {
            string cacheKey = ReportTemplateQuestionCacheKeys.ListReportTemplateQuestionByQuestionTemplateIdKey(questionTemplateId);
            var reportTemplateQuestionList = await _distributedCache.GetAsync<List<ReportTemplateQuestion>>(cacheKey);
            if (reportTemplateQuestionList == null)
            {
                reportTemplateQuestionList = await _reportTemplateQuestionRepository.GetListFromQuestionTemplateIdAsync(questionTemplateId);
                await _distributedCache.SetAsync(cacheKey, reportTemplateQuestionList);
            }
            return reportTemplateQuestionList;
        }


        //---------------------------------------------------------------------------------------------------

        public async Task<List<ReportTemplateQuestion>> GetByReportTemplateIdCachedListAsync(int reportTemplateId)
        {
            string cacheKey = ReportTemplateQuestionCacheKeys.ListReportTemplateQuestionByReportTemplateIdKey(reportTemplateId);
            var reportTemplateQuestionList = await _distributedCache.GetAsync<List<ReportTemplateQuestion>>(cacheKey);
            if (reportTemplateQuestionList == null)
            {
                reportTemplateQuestionList = await _reportTemplateQuestionRepository.GetListFromReportTemplateIdAsync(reportTemplateId);
                await _distributedCache.SetAsync(cacheKey, reportTemplateQuestionList);
            }
            return reportTemplateQuestionList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportTemplateQuestion> GetByIdAsync(int reportTemplateQuestionId)
        {
            string cacheKey = ReportTemplateQuestionCacheKeys.GetKey(reportTemplateQuestionId);
            var reportTemplateQuestion = await _distributedCache.GetAsync<ReportTemplateQuestion>(cacheKey);
            if (reportTemplateQuestion == null)
            {
                reportTemplateQuestion = await _reportTemplateQuestionRepository.GetByIdAsync(reportTemplateQuestionId);
                if (reportTemplateQuestion == null) return null;
                await _distributedCache.SetAsync(cacheKey, reportTemplateQuestion);
            }
            return reportTemplateQuestion;
        }


        //---------------------------------------------------------------------------------------------------

    }
}