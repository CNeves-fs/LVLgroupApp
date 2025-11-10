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
    public class ReportRepository : IReportRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IRepositoryAsync<Report> _repository;

        private readonly IRepositoryAsync<Core.Entities.Reports.QuestionTemplate> _questionTemplateRepository;

        private readonly IRepositoryAsync<Core.Entities.Reports.ReportTemplate> _reportTemplateRepository;

        private readonly IDistributedCache _distributedCache;


        //---------------------------------------------------------------------------------------------------


        public ReportRepository(IDistributedCache distributedCache,
                                                IRepositoryAsync<Report> repository,
                                                IRepositoryAsync<Core.Entities.Reports.ReportTemplate> reportTemplateRepository,
                                                IRepositoryAsync<Core.Entities.Reports.QuestionTemplate> questionTemplateRepository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
            _questionTemplateRepository = questionTemplateRepository;
            _reportTemplateRepository = reportTemplateRepository;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Report> Reports => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Report>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Report>> GetListFromLojaIdAsync(int lojaId)
        {
            return await _repository.Entities.Where(r => r.LojaId == lojaId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Report>> GetListFromReportTemplateIdAsync(int reportTemplateId)
        {
            return await _repository.Entities.Where(r => r.ReportTemplateId == reportTemplateId).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Report> GetByIdAsync(int reportId)
        {
            return await _repository.Entities.Where(r => r.Id == reportId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<int> InsertAsync(Report report)
        {
            await _repository.AddAsync(report);
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.GetKey(report.Id));
            await _distributedCache.RemoveAsync(ReportCacheKeys.AllReportListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListReportByLojaIdKey(report.LojaId));
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListReportByReportTemplateIdKey(report.ReportTemplateId));

            return report.Id;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Report report)
        {
            await _repository.AddAsync(report);
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.GetKey(report.Id));
            await _distributedCache.RemoveAsync(ReportCacheKeys.AllReportListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListReportByLojaIdKey(report.LojaId));
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListReportByReportTemplateIdKey(report.ReportTemplateId));
        }


        //---------------------------------------------------------------------------------------------------
        
        
        public async Task UpdateAsync(Report report)
        {
            await _repository.AddAsync(report);
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.GetKey(report.Id));
            await _distributedCache.RemoveAsync(ReportCacheKeys.AllReportListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.SelectListKey);
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListReportByLojaIdKey(report.LojaId));
            await _distributedCache.RemoveAsync(ReportCacheKeys.ListReportByReportTemplateIdKey(report.ReportTemplateId));
        }


        //---------------------------------------------------------------------------------------------------

    }
}