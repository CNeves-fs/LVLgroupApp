using Core.Entities.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IQuestionTemplateLocalizedRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<QuestionTemplateLocalized> QuestionTemplatesLocalized { get; }

        Task<List<QuestionTemplateLocalized>> GetListAsync();

        Task<List<QuestionTemplateLocalized>> GetListFromQuestionTemplateIdAsync(int questionTemplateId);

        Task<List<QuestionTemplateLocalized>> GetByLanguageAsync(string language);

        Task<QuestionTemplateLocalized> GetByIdAsync(int questionTemplatelocalizedId);

        Task<QuestionTemplateLocalized> GetByTextAsync(string text);

        Task<int> InsertAsync(QuestionTemplateLocalized questionTemplatelocalized);

        Task UpdateAsync(QuestionTemplateLocalized questionTemplatelocalized);

        Task DeleteAsync(QuestionTemplateLocalized questionTemplatelocalized);


        //---------------------------------------------------------------------------------------------------

    }
}