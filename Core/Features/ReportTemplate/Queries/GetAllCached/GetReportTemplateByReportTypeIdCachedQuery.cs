using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTemplate.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplate.Queries.GetAllCached
{
    public class GetReportTemplateByReportTypeIdCachedQuery : IRequest<Result<List<ReportTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int reportTypeId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetReportTemplateByReportTypeIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetReportTemplateByReportTypeIdCachedQueryHandler : IRequestHandler<GetReportTemplateByReportTypeIdCachedQuery, Result<List<ReportTemplateCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateCacheRepository _reportTemplateCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetReportTemplateByReportTypeIdCachedQueryHandler(IReportTemplateCacheRepository reportTemplateCache, IMapper mapper)
        {
            _reportTemplateCache = reportTemplateCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportTemplateCachedResponse>>> Handle(GetReportTemplateByReportTypeIdCachedQuery request, CancellationToken cancellationToken)
        {
            var reportTemplateList = await _reportTemplateCache.GetByReportTypeIdCachedListAsync(request.reportTypeId);
            var mappedReportTemplate = _mapper.Map<List<ReportTemplateCachedResponse>>(reportTemplateList);
            return Result<List<ReportTemplateCachedResponse>>.Success(mappedReportTemplate);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}