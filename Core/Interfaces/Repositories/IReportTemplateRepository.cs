using Core.Entities.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IReportTemplateRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<ReportTemplate> ReportTemplates { get; }

        Task<List<ReportTemplate>> GetListAsync();

        Task<List<ReportTemplate>> GetListFromReportTypeIdAsync(int reportTypeId);

        Task<List<ReportTemplate>> GetListActiveAsync();

        Task<ReportTemplate> GetByIdAsync(int reportTemplateId);

        Task<int> InsertAsync(ReportTemplate reportTemplate);

        Task UpdateAsync(ReportTemplate reportTemplate);

        Task DeleteAsync(ReportTemplate reportTemplate);


        //---------------------------------------------------------------------------------------------------

    }
}