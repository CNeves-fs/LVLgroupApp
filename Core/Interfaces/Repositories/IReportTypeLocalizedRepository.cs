using Core.Entities.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IReportTypeLocalizedRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<ReportTypeLocalized> ReportTypesLocalized { get; }

        Task<List<ReportTypeLocalized>> GetListAsync();

        Task<List<ReportTypeLocalized>> GetListFromReportTypeIdAsync(int reportTypeId);

        Task<List<ReportTypeLocalized>> GetByLanguageAsync(string language);

        Task<ReportTypeLocalized> GetByIdAsync(int reportTypeLocalizedId);

        Task<ReportTypeLocalized> GetByNameAsync(string name);

        Task<int> InsertAsync(ReportTypeLocalized reportTypeLocalized);

        Task UpdateAsync(ReportTypeLocalized reportTypeLocalized);

        Task DeleteAsync(ReportTypeLocalized reportTypeLocalized);


        //---------------------------------------------------------------------------------------------------

    }
}