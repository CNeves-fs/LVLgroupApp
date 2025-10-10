using Core.Entities.Business;
using Core.Entities.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IReportTemplateQuestionCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<ReportTemplateQuestion>> GetCachedAllReportTemplateQuestionAsync();

        Task<List<ReportTemplateQuestion>> GetCachedAllActiveReportTemplateQuestionAsync();

        Task<List<ReportTemplateQuestion>> GetCachedListAsync();

        Task<List<ReportTemplateQuestion>> GetByQuestionTemplateIdCachedListAsync(int questionTemplateId);

        Task<List<ReportTemplateQuestion>> GetByReportTemplateIdCachedListAsync(int reportTemplateId);

        Task<ReportTemplateQuestion> GetByIdAsync(int reportTemplateQuestionId);


        //---------------------------------------------------------------------------------------------------

    }
}