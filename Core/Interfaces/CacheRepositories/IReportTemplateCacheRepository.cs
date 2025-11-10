using Core.Entities.Business;
using Core.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IReportTemplateCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<ReportTemplate>> GetCachedAllReportTemplateAsync();

        Task<List<ReportTemplate>> GetCachedAllActiveReportTemplateAsync();

        Task<List<ReportTemplate>> GetCachedListAsync();

        Task<List<ReportTemplate>> GetByReportTypeIdCachedListAsync(int reportTypeId);

        Task<ReportTemplate> GetByIdAsync(int questionTypeId);


        //---------------------------------------------------------------------------------------------------

    }
}