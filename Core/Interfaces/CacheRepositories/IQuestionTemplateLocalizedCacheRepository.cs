using Core.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IQuestionTemplateLocalizedCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<QuestionTemplateLocalized>> GetCachedListAsync();

        Task<List<QuestionTemplateLocalized>> GetByQuestionTemplateIdAsync(int questionTemplateId);

        Task<List<QuestionTemplateLocalized>> GetByLanguageAsync(string language);

        Task<QuestionTemplateLocalized> GetByIdAsync(int questionTemplatelocalizedId);

        Task<QuestionTemplateLocalized> GetByTextAsync(string text);


        //---------------------------------------------------------------------------------------------------

    }
}