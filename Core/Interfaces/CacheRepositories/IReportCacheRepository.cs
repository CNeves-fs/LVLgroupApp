using Core.Entities.Business;
using Core.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IReportCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Report>> GetCachedAllReportAsync();

        Task<List<Report>> GetCachedListAsync();

        Task<List<Report>> GetByLojaIdCachedListAsync(int lojaId);

        Task<List<Report>> GetByReportTemplateIdCachedListAsync(int reportTemplateId);

        Task<Report> GetByIdAsync(int reportId);


        //---------------------------------------------------------------------------------------------------

    }
}