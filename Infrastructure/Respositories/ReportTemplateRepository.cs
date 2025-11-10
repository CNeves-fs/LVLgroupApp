using AspNetCoreHero.Extensions.Caching;
using Core.Entities.Reports;
using Core.Interfaces.Repositories;
using Infrastructure.CacheKeys;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReportTemplateRepository : IReportTemplateRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<ReportTemplate> _repository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ReportTemplateRepository(IDistributedCache distributedCache, IRepositoryAsync<ReportTemplate> repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<ReportTemplate> ReportTemplates => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplate>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplate>> GetListActiveAsync()
        {
            return await _repository.Entities.Where(rt => rt.IsActive == true).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<ReportTemplate>> GetListFromReportTypeIdAsync(int reportTypeId)
        {
            return await _repository.Entities.Where(rt => rt.ReportTypeId == reportTypeId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<ReportTemplate> GetByIdAsync(int reportTemplateId)
        {
            return await _repository.Entities.Where(o => o.Id == reportTemplateId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(ReportTemplate reportTemplate)
        {
            await _repository.AddAsync(reportTemplate);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.GetKey(reportTemplate.Id));
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.AllReportTemplateListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.AllActiveReportTemplateListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListIsActiveKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListReportTemplateByReportTypeIdKey(reportTemplate.ReportTypeId));

            return reportTemplate.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(ReportTemplate reportTemplate)
        {
            await _repository.DeleteAsync(reportTemplate);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.GetKey(reportTemplate.Id));
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.AllReportTemplateListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.AllActiveReportTemplateListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListIsActiveKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListReportTemplateByReportTypeIdKey(reportTemplate.ReportTypeId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(ReportTemplate reportTemplate)
        {
            await _repository.UpdateAsync(reportTemplate);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.GetKey(reportTemplate.Id));
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.AllReportTemplateListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.AllActiveReportTemplateListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListIsActiveKey);
            await _distributedCache.RemoveAsync(ReportTemplateCacheKeys.ListReportTemplateByReportTypeIdKey(reportTemplate.ReportTypeId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}