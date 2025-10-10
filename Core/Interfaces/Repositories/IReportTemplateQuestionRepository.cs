using Core.Entities.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IReportTemplateQuestionRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<ReportTemplateQuestion> ReportTemplateQuestions { get; }

        Task<List<ReportTemplateQuestion>> GetListAsync();

        Task<List<ReportTemplateQuestion>> GetListFromReportTemplateIdAsync(int reportTemplateId);

        Task<List<ReportTemplateQuestion>> GetListFromQuestionTemplateIdAsync(int questionTemplateId);

        Task<List<ReportTemplateQuestion>> GetListActiveAsync();

        Task<ReportTemplateQuestion> GetByIdAsync(int reportTemplateQuestionId);

        Task<int> InsertAsync(ReportTemplateQuestion reportTemplateQuestion);

        Task UpdateAsync(ReportTemplateQuestion reportTemplateQuestion);

        Task DeleteAsync(ReportTemplateQuestion reportTemplateQuestion);


        //---------------------------------------------------------------------------------------------------

    }
}