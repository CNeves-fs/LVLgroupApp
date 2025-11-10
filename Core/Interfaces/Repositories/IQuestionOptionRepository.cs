using Core.Entities.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IQuestionOptionRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<QuestionOption> QuestionOptions { get; }

        Task<List<QuestionOption>> GetAllQuestionOptionsListAsync();

        Task<List<QuestionOption>> GetAllActiveQuestionOptionsListAsync();

        Task<List<QuestionOption>> GetListAsync();

        Task<List<QuestionOption>> GetListFromQuestionTemplateIdAsync(int questionTemplateId);

        Task<QuestionOption> GetByIdAsync(int questionOptionId);

        Task<int> InsertAsync(QuestionOption questionOption);

        Task UpdateAsync(QuestionOption questionOption);

        Task DeleteAsync(QuestionOption questionOption);


        //---------------------------------------------------------------------------------------------------

    }
}