using Core.Entities.Ocorrencias;
using Core.Entities.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IReportTypeRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<ReportType> ReportTypes { get; }

        Task<List<ReportType>> GetListAsync();

        Task<ReportType> GetByIdAsync(int reportTypeId);

        Task<ReportType> GetByDefaultNameAsync(string defaultName);

        Task<int> InsertAsync(ReportType reportType);

        Task UpdateAsync(ReportType reportType);

        Task DeleteAsync(ReportType reportType);


        //---------------------------------------------------------------------------------------------------

    }
}