using Core.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IQuestionOptionCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<QuestionOption>> GetCachedAllQuestionOptionAsync();

        Task<List<QuestionOption>> GetCachedAllActiveQuestionOptionAsync();

        Task<List<QuestionOption>> GetCachedListAsync();

        Task<List<QuestionOption>> GetByQuestionTemplateIdCachedListAsync(int questionTemplateId);

        Task<QuestionOption> GetByIdAsync(int questionOptionId);


        //---------------------------------------------------------------------------------------------------

    }
}