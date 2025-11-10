using Core.Entities.Business;
using Core.Entities.Reports;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class QuestionTemplateRepository : IQuestionTemplateRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<QuestionTemplate> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateRepository(IDistributedCache distributedCache, IRepositoryAsync<QuestionTemplate> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<QuestionTemplate> QuestionTemplates => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<QuestionTemplate> GetByIdAsync(int questionTemplateId)
        {
            return await _repository.Entities.Where(q => q.Id == questionTemplateId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplate>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplate>> GetAllQuestionTemplatesListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplate>> GetAllActiveQuestionTemplatesListAsync()
        {
            return await _repository.Entities.Where(q => q.IsActive == true).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplate>> GetListFromTypeAsync(int questionTemplateId)
        {
            return await _repository.Entities.Where(q => (q.QuestionTypeId == questionTemplateId)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(QuestionTemplate questionTemplate)
        {
            await _repository.AddAsync(questionTemplate);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.AllQuestionTemplateListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.AllActiveQuestionTemplateListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.GetKey(questionTemplate.Id));
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.ListFromTypeKey(questionTemplate.QuestionTypeId));
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.SelectListFromTypeKey(questionTemplate.QuestionTypeId));
            return questionTemplate.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(QuestionTemplate questionTemplate)
        {
            await _repository.DeleteAsync(questionTemplate);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.AllQuestionTemplateListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.AllActiveQuestionTemplateListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.GetKey(questionTemplate.Id));
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.ListFromTypeKey(questionTemplate.QuestionTypeId));
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.SelectListFromTypeKey(questionTemplate.QuestionTypeId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(QuestionTemplate questionTemplate)
        {
            await _repository.UpdateAsync(questionTemplate);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.AllQuestionTemplateListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.AllActiveQuestionTemplateListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.GetKey(questionTemplate.Id));
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.ListFromTypeKey(questionTemplate.QuestionTypeId));
            await _distributedCache.RemoveAsync(QuestionTemplateCacheKeys.SelectListFromTypeKey(questionTemplate.QuestionTypeId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}