using Core.Entities.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IQuestionTemplateRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<QuestionTemplate> QuestionTemplates { get; }

        Task<List<QuestionTemplate>> GetAllQuestionTemplatesListAsync();

        Task<List<QuestionTemplate>> GetAllActiveQuestionTemplatesListAsync();

        Task<List<QuestionTemplate>> GetListAsync();

        Task<List<QuestionTemplate>> GetListFromTypeAsync(int questionTypeId);

        Task<QuestionTemplate> GetByIdAsync(int questionTemplateId);

        Task<int> InsertAsync(QuestionTemplate questionTemplate);

        Task UpdateAsync(QuestionTemplate questionTemplate);

        Task DeleteAsync(QuestionTemplate questionTemplate);


        //---------------------------------------------------------------------------------------------------

    }
}