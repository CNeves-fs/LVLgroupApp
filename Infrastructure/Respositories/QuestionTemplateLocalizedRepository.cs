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
    public class QuestionTemplateLocalizedRepository : IQuestionTemplateLocalizedRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<QuestionTemplateLocalized> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public QuestionTemplateLocalizedRepository(IDistributedCache distributedCache, IRepositoryAsync<QuestionTemplateLocalized> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<QuestionTemplateLocalized> QuestionTemplatesLocalized => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplateLocalized>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<QuestionTemplateLocalized> GetByIdAsync(int questionTemplateLocalizedId)
        {
            return await _repository.Entities.Where(qtl => qtl.Id == questionTemplateLocalizedId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplateLocalized>> GetListFromQuestionTemplateIdAsync(int questionTemplateId)
        {
            return await _repository.Entities.Where(qtl => qtl.QuestionTemplateId == questionTemplateId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<QuestionTemplateLocalized>> GetByLanguageAsync(string language)
        {
            return await _repository.Entities.Where(qtl => qtl.Language == language).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<QuestionTemplateLocalized> GetByTextAsync(string text)
        {
            return await _repository.Entities.Where(qtl => qtl.QuestionText == text).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(QuestionTemplateLocalized questionTemplateLocalized)
        {
            await _repository.AddAsync(questionTemplateLocalized);
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetKey(questionTemplateLocalized.Id));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetTextKey(questionTemplateLocalized.QuestionText));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetLanguageKey(questionTemplateLocalized.Language));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.ListFromQuestionTemplateKey(questionTemplateLocalized.QuestionTemplateId));

            return questionTemplateLocalized.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(QuestionTemplateLocalized questionTemplateLocalized)
        {
            await _repository.DeleteAsync(questionTemplateLocalized);
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetKey(questionTemplateLocalized.Id));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetTextKey(questionTemplateLocalized.QuestionText));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetLanguageKey(questionTemplateLocalized.Language));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.ListFromQuestionTemplateKey(questionTemplateLocalized.QuestionTemplateId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(QuestionTemplateLocalized questionTemplateLocalized)
        {
            await _repository.UpdateAsync(questionTemplateLocalized);
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetKey(questionTemplateLocalized.Id));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetTextKey(questionTemplateLocalized.QuestionText));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.GetLanguageKey(questionTemplateLocalized.Language));
            await _distributedCache.RemoveAsync(QuestionTemplateLocalizedCacheKeys.ListFromQuestionTemplateKey(questionTemplateLocalized.QuestionTemplateId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}