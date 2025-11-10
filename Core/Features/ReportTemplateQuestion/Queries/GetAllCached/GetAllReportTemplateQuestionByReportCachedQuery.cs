using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTemplateQuestion.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplateQuestion.Queries.GetAllCached
{
    public class GetAllReportTemplateQuestionByReportCachedQuery : IRequest<Result<List<ReportTemplateQuestionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int reportTemplateId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTemplateQuestionByReportCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllReportTemplateQuestionByReportCachedQueryHandler : IRequestHandler<GetAllReportTemplateQuestionByReportCachedQuery, Result<List<ReportTemplateQuestionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateQuestionCacheRepository _reportTemplateQuestionCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTemplateQuestionByReportCachedQueryHandler(IReportTemplateQuestionCacheRepository reportTemplateQuestionCache, IMapper mapper)
        {
            _reportTemplateQuestionCache = reportTemplateQuestionCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportTemplateQuestionCachedResponse>>> Handle(GetAllReportTemplateQuestionByReportCachedQuery request, CancellationToken cancellationToken)
        {
            var reportTemplateQuestionList = await _reportTemplateQuestionCache.GetByReportTemplateIdCachedListAsync(request.reportTemplateId);
            var mappedReportTemplateQuestionList = _mapper.Map<List<ReportTemplateQuestionCachedResponse>>(reportTemplateQuestionList);
            return Result<List<ReportTemplateQuestionCachedResponse>>.Success(mappedReportTemplateQuestionList);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}