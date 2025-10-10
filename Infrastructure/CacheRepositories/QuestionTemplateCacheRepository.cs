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
    public class QuestionTemplateCacheRepository : IQuestionTemplateCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IDistributedCache _distributedCache;

        private readonly IQuestionTemplateRepository _questionTemplateRepository;


        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateCacheRepository(IDistributedCache distributedCache, IQuestionTemplateRepository questionTemplateRepository)
        {
            _distributedCache = distributedCache;
            _questionTemplateRepository = questionTemplateRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<QuestionTemplate> GetByIdAsync(int questionTemplateId)
        {
            string cacheKey = QuestionTemplateCacheKeys.GetKey(questionTemplateId);
            var questionTemplate = await _distributedCache.GetAsync<QuestionTemplate>(cacheKey);
            if (questionTemplate == null)
            {
                questionTemplate = await _questionTemplateRepository.GetByIdAsync(questionTemplateId);
                // Throw.Exception.IfNull(questionTemplate, "QuestionTemplate", "QuestionTemplate not Found");
                if (questionTemplate == null) return null;
                await _distributedCache.SetAsync(cacheKey, questionTemplate);
            }
            return questionTemplate;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplate>> GetCachedAllQuestionTemplateAsync()
        {
            string cacheKey = QuestionTemplateCacheKeys.AllQuestionTemplateListKey;
            var questionTemplateList = await _distributedCache.GetAsync<List<QuestionTemplate>>(cacheKey);
            if (questionTemplateList == null)
            {
                questionTemplateList = await _questionTemplateRepository.GetAllQuestionTemplatesListAsync();
                await _distributedCache.SetAsync(cacheKey, questionTemplateList);
            }
            return questionTemplateList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplate>> GetCachedAllActiveQuestionTemplateAsync()
        {
            string cacheKey = QuestionTemplateCacheKeys.AllActiveQuestionTemplateListKey;
            var questionTemplateList = await _distributedCache.GetAsync<List<QuestionTemplate>>(cacheKey);
            if (questionTemplateList == null)
            {
                questionTemplateList = await _questionTemplateRepository.GetAllActiveQuestionTemplatesListAsync();
                await _distributedCache.SetAsync(cacheKey, questionTemplateList);
            }
            return questionTemplateList;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplate>> GetCachedListAsync()
        {
            string cacheKey = QuestionTemplateCacheKeys.ListKey;
            var questionTemplateList = await _distributedCache.GetAsync<List<QuestionTemplate>>(cacheKey);
            if (questionTemplateList == null)
            {
                questionTemplateList = await _questionTemplateRepository.GetListAsync();
                await _distributedCache.SetAsync(cacheKey, questionTemplateList);
            }
            return questionTemplateList;
        }





        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplate>> GetByTypeIdCachedListAsync(int questionTemplateId)
        {
            string cacheKey = QuestionTemplateCacheKeys.ListFromTypeKey(questionTemplateId);
            var questionTemplateList = await _distributedCache.GetAsync<List<QuestionTemplate>>(cacheKey);
            if (questionTemplateList == null)
            {
                questionTemplateList = await _questionTemplateRepository.GetListFromTypeAsync(questionTemplateId);
                await _distributedCache.SetAsync(cacheKey, questionTemplateList);
            }
            return questionTemplateList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}