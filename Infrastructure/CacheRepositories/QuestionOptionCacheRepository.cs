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
    public class QuestionOptionCacheRepository : IQuestionOptionCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IQuestionOptionRepository _questionOptionRepository;


        //---------------------------------------------------------------------------------------------------


        public QuestionOptionCacheRepository(IDistributedCache distributedCache, IQuestionOptionRepository questionOptionRepository)
        {
            _distributedCache = distributedCache;
            _questionOptionRepository = questionOptionRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<QuestionOption> GetByIdAsync(int questionOptionId)
        {
            string cacheKey = QuestionOptionCacheKeys.GetKey(questionOptionId);
            var questionOption = await _distributedCache.GetAsync<QuestionOption>(cacheKey);
            if (questionOption == null)
            {
                questionOption = await _questionOptionRepository.GetByIdAsync(questionOptionId);
                // Throw.Exception.IfNull(questionOption, "QuestionOption", "QuestionOption not Found");
                if (questionOption == null) return null;
                await _distributedCache.SetAsync(cacheKey, questionOption);
            }
            return questionOption;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionOption>> GetCachedAllQuestionOptionAsync()
        {
            string cacheKey = QuestionOptionCacheKeys.AllQuestionOptionListKey;
            var questionOptionList = await _distributedCache.GetAsync<List<QuestionOption>>(cacheKey);
            if (questionOptionList == null)
            {
                questionOptionList = await _questionOptionRepository.GetAllQuestionOptionsListAsync();
                await _distributedCache.SetAsync(cacheKey, questionOptionList);
            }
            return questionOptionList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionOption>> GetCachedAllActiveQuestionOptionAsync()
        {
            string cacheKey = QuestionOptionCacheKeys.AllActiveQuestionOptionListKey;
            var questionOptionList = await _distributedCache.GetAsync<List<QuestionOption>>(cacheKey);
            if (questionOptionList == null)
            {
                questionOptionList = await _questionOptionRepository.GetAllActiveQuestionOptionsListAsync();
                await _distributedCache.SetAsync(cacheKey, questionOptionList);
            }
            return questionOptionList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionOption>> GetCachedListAsync()
        {
            string cacheKey = QuestionOptionCacheKeys.ListKey;
            var questionOptionList = await _distributedCache.GetAsync<List<QuestionOption>>(cacheKey);
            if (questionOptionList == null)
            {
                questionOptionList = await _questionOptionRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, questionOptionList);
            }
            return questionOptionList;
        }





        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionOption>> GetByQuestionTemplateIdCachedListAsync(int questionTemplateId)
        {
            string cacheKey = QuestionOptionCacheKeys.ListFromQuestionTemplateIdKey(questionTemplateId);
            var questionOptionList = await _distributedCache.GetAsync<List<QuestionOption>>(cacheKey);
            if (questionOptionList == null)
            {
                questionOptionList = await _questionOptionRepository.GetListFromQuestionTemplateIdAsync(questionTemplateId);
                await _distributedCache.SetAsync(cacheKey, questionOptionList);
            }
            return questionOptionList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}