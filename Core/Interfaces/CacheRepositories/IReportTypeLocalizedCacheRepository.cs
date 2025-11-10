using Core.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IReportTypeLocalizedCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<ReportTypeLocalized>> GetCachedListAsync();

        Task<List<ReportTypeLocalized>> GetByReportTypeIdAsync(int reportTypeId);

        Task<List<ReportTypeLocalized>> GetByLanguageAsync(string language);

        Task<ReportTypeLocalized> GetByIdAsync(int reportTypeLocalizedId);

        Task<ReportTypeLocalized> GetByNameAsync(string name);


        //---------------------------------------------------------------------------------------------------

    }
}