using Core.Entities.Ocorrencias;
using Core.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IReportTypeCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<ReportType>> GetCachedAllReportTypeAsync();

        Task<List<ReportType>> GetCachedListAsync();

        Task<ReportType> GetByNameAsync(string defaultname);

        Task<ReportType> GetByIdAsync(int reportTypeId);


        //---------------------------------------------------------------------------------------------------

    }
}