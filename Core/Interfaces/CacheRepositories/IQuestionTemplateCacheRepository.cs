using Core.Entities.Business;
using Core.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IQuestionTemplateCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<QuestionTemplate>> GetCachedAllQuestionTemplateAsync();

        Task<List<QuestionTemplate>> GetCachedAllActiveQuestionTemplateAsync();

        Task<List<QuestionTemplate>> GetCachedListAsync();

        Task<List<QuestionTemplate>> GetByTypeIdCachedListAsync(int questionTypeId);

        Task<QuestionTemplate> GetByIdAsync(int questionTemplateId);


        //---------------------------------------------------------------------------------------------------

    }
}