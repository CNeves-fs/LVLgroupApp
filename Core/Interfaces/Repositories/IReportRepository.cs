using Core.Entities.Reports;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IReportRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Report> Reports { get; }

        Task<List<Report>> GetListAsync();

        Task<List<Report>> GetListFromReportTemplateIdAsync(int reportTemplateId);

        Task<List<Report>> GetListFromLojaIdAsync(int lojaId);

        Task<Report> GetByIdAsync(int reportId);

        Task<int> InsertAsync(Report report);

        Task UpdateAsync(Report report);

        Task DeleteAsync(Report report);


        //---------------------------------------------------------------------------------------------------

    }
}