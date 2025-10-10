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
    public class ReportTemplateQuestionRepository : IReportTemplateQuestionRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<ReportTemplateQuestion> _repository;

        private readonly IRepositoryAsync<Core.Entities.Reports.QuestionTemplate> _questionTemplateRepository;

        private readonly IRepositoryAsync<Core.Entities.Reports.ReportTemplate> _reportTemplateRepository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ReportTemplateQuestionRepository(IDistributedCache distributedCache,
                                                IRepositoryAsync<ReportTemplateQuestion> repository,
                                                IRepositoryAsync<Core.Entities.Reports.ReportTemplate> reportTemplateRepository,
                                                IRepositoryAsync<Core.Entities.Reports.QuestionTemplate> questionTemplateRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _questionTemplateRepository = questionTemplateRepository;
            _reportTemplateRepository = reportTemplateRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<ReportTemplateQuestion> ReportTemplateQuestions => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplateQuestion>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplateQuestion>> GetListActiveAsync()
        {
            var questionTemplates = await _questionTemplateRepository.Entities.Where(qt => qt.IsActive == true).Select(qt => qt.Id).ToListAsync();
            var reportTemplates = await _reportTemplateRepository.Entities.Where(rt => rt.IsActive == true).Select(rt => rt.Id).ToListAsync();
            return await _repository.Entities.Where(rtq => questionTemplates.Contains(rtq.QuestionTemplateId) && reportTemplates.Contains(rtq.ReportTemplateId)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplateQuestion>> GetListFromQuestionTemplateIdAsync(int questionTemplateId)
        {
            return await _repository.Entities.Where(rtq => rtq.QuestionTemplateId == questionTemplateId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplateQuestion>> GetListFromReportTemplateIdAsync(int reportTemplateId)
        {
            return await _repository.Entities.Where(rtq => rtq.ReportTemplateId == reportTemplateId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportTemplateQuestion> GetByIdAsync(int reportTemplateQuestionId)
        {
            return await _repository.Entities.Where(rtq => rtq.Id == reportTemplateQuestionId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(ReportTemplateQuestion reportTemplateQuestion)
        {
            await _repository.AddAsync(reportTemplateQuestion);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.GetKey(reportTemplateQuestion.Id));
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.AllReportTemplateQuestionListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.AllActiveReportTemplateQuestionListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListReportTemplateQuestionByQuestionTemplateIdKey(reportTemplateQuestion.QuestionTemplateId));
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListReportTemplateQuestionByReportTemplateIdKey(reportTemplateQuestion.ReportTemplateId));

            return reportTemplateQuestion.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(ReportTemplateQuestion reportTemplateQuestion)
        {
            await _repository.AddAsync(reportTemplateQuestion);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.GetKey(reportTemplateQuestion.Id));
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.AllReportTemplateQuestionListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.AllActiveReportTemplateQuestionListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListReportTemplateQuestionByQuestionTemplateIdKey(reportTemplateQuestion.QuestionTemplateId));
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListReportTemplateQuestionByReportTemplateIdKey(reportTemplateQuestion.ReportTemplateId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(ReportTemplateQuestion reportTemplateQuestion)
        {
            await _repository.AddAsync(reportTemplateQuestion);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.GetKey(reportTemplateQuestion.Id));
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.AllReportTemplateQuestionListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.AllActiveReportTemplateQuestionListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListReportTemplateQuestionByQuestionTemplateIdKey(reportTemplateQuestion.QuestionTemplateId));
            await _distributedCache.RemoveAsync(ReportTemplateQuestionCacheKeys.ListReportTemplateQuestionByReportTemplateIdKey(reportTemplateQuestion.ReportTemplateId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}