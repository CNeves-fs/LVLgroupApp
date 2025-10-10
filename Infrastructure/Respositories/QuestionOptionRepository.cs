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
    public class QuestionOptionRepository : IQuestionOptionRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<QuestionOption> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public QuestionOptionRepository(IDistributedCache distributedCache, IRepositoryAsync<QuestionOption> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<QuestionOption> QuestionOptions => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<QuestionOption> GetByIdAsync(int questionOptionId)
        {
            return await _repository.Entities.Where(q => q.Id == questionOptionId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionOption>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionOption>> GetAllQuestionOptionsListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionOption>> GetAllActiveQuestionOptionsListAsync()
        {
            return await _repository.Entities.Where(q => q.IsActive == true).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionOption>> GetListFromQuestionTemplateIdAsync(int questionTemplateId)
        {
            return await _repository.Entities.Where(q => (q.QuestionTemplateId == questionTemplateId)).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(QuestionOption questionOption)
        {
            await _repository.AddAsync(questionOption);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.AllQuestionOptionListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.AllActiveQuestionOptionListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.GetKey(questionOption.Id));
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.ListFromQuestionTemplateIdKey(questionOption.QuestionTemplateId));
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.SelectListFromQuestionTemplateIdKey(questionOption.QuestionTemplateId));
            return questionOption.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(QuestionOption questionOption)
        {
            await _repository.DeleteAsync(questionOption);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.AllQuestionOptionListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.AllActiveQuestionOptionListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.GetKey(questionOption.Id));
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.ListFromQuestionTemplateIdKey(questionOption.QuestionTemplateId));
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.SelectListFromQuestionTemplateIdKey(questionOption.QuestionTemplateId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(QuestionOption questionOption)
        {
            await _repository.UpdateAsync(questionOption);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.AllQuestionOptionListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.AllActiveQuestionOptionListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.GetKey(questionOption.Id));
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.ListFromQuestionTemplateIdKey(questionOption.QuestionTemplateId));
            await _distributedCache.RemoveAsync(QuestionOptionCacheKeys.SelectListFromQuestionTemplateIdKey(questionOption.QuestionTemplateId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}