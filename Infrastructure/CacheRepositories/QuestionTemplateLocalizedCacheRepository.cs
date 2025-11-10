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
    public class QuestionTemplateLocalizedCacheRepository : IQuestionTemplateLocalizedCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IQuestionTemplateLocalizedRepository _questionTemplateLocalizedRepository;


        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateLocalizedCacheRepository(IDistributedCache distributedCache, IQuestionTemplateLocalizedRepository questionTemplateLocalizedRepository)
        {
            _distributedCache = distributedCache;
            _questionTemplateLocalizedRepository = questionTemplateLocalizedRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplateLocalized>> GetCachedListAsync()
        {
            string cacheKey = QuestionTemplateLocalizedCacheKeys.ListKey;
            var questionTemplateLocalizedList = await _distributedCache.GetAsync<List<QuestionTemplateLocalized>>(cacheKey);
            if (questionTemplateLocalizedList == null)
            {
                questionTemplateLocalizedList = await _questionTemplateLocalizedRepository.GetListAsync();
                //await _distributedCache.SetAsync(cacheKey, questionTemplateLocalizedList);
            }
            return questionTemplateLocalizedList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<QuestionTemplateLocalized> GetByIdAsync(int questionTemplateLocalizedId)
        {
            string cacheKey = QuestionTemplateLocalizedCacheKeys.GetKey(questionTemplateLocalizedId);
            var questionTemplateLocalized = await _distributedCache.GetAsync<QuestionTemplateLocalized>(cacheKey);
            if (questionTemplateLocalized == null)
            {
                questionTemplateLocalized = await _questionTemplateLocalizedRepository.GetByIdAsync(questionTemplateLocalizedId);
                if (questionTemplateLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, questionTemplateLocalized);
            }
            return questionTemplateLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<QuestionTemplateLocalized> GetByTextAsync(string text)
        {
            string cacheKey = QuestionTemplateLocalizedCacheKeys.GetTextKey(text);
            var questionTemplateLocalized = await _distributedCache.GetAsync<QuestionTemplateLocalized>(cacheKey);
            if (questionTemplateLocalized == null)
            {
                questionTemplateLocalized = await _questionTemplateLocalizedRepository.GetByTextAsync(text);
                if (questionTemplateLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, questionTemplateLocalized);
            }
            return questionTemplateLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplateLocalized>> GetByLanguageAsync(string language)
        {
            string cacheKey = QuestionTemplateLocalizedCacheKeys.GetLanguageKey(language);
            var questionTemplateLocalized = await _distributedCache.GetAsync<List<QuestionTemplateLocalized>>(cacheKey);
            if (questionTemplateLocalized == null)
            {
                questionTemplateLocalized = await _questionTemplateLocalizedRepository.GetByLanguageAsync(language);
                if (questionTemplateLocalized == null) return null;
                //await _distributedCache.SetAsync(cacheKey, questionTemplateLocalized);
            }
            return questionTemplateLocalized;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplateLocalized>> GetByQuestionTemplateIdAsync(int questionTemplateId)
        {
            string cacheKey = QuestionTemplateLocalizedCacheKeys.ListFromQuestionTemplateKey(questionTemplateId);
            var questionTemplateLocalizedList = await _distributedCache.GetAsync<List<QuestionTemplateLocalized>>(cacheKey);
            if (questionTemplateLocalizedList == null)
            {
                questionTemplateLocalizedList = await _questionTemplateLocalizedRepository.GetListFromQuestionTemplateIdAsync(questionTemplateId);
                if (questionTemplateLocalizedList == null) return null;
                //await _distributedCache.SetAsync(cacheKey, questionTemplateLocalizedList);
            }
            return questionTemplateLocalizedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}