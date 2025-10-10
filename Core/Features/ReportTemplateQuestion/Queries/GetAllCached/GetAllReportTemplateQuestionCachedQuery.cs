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
    public class GetAllReportTemplateQuestionCachedQuery : IRequest<Result<List<ReportTemplateQuestionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllReportTemplateQuestionCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllReportTemplateQuestionCachedQueryHandler : IRequestHandler<GetAllReportTemplateQuestionCachedQuery, Result<List<ReportTemplateQuestionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateQuestionCacheRepository _reportTemplateQuestionCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTemplateQuestionCachedQueryHandler(IReportTemplateQuestionCacheRepository reportTemplateQuestionCache, IMapper mapper)
        {
            _reportTemplateQuestionCache = reportTemplateQuestionCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportTemplateQuestionCachedResponse>>> Handle(GetAllReportTemplateQuestionCachedQuery request, CancellationToken cancellationToken)
        {
            var reportTemplateQuestionList = await _reportTemplateQuestionCache.GetCachedAllReportTemplateQuestionAsync();
            var mappedReportTemplateQuestion = _mapper.Map<List<ReportTemplateQuestionCachedResponse>>(reportTemplateQuestionList);
            return Result<List<ReportTemplateQuestionCachedResponse>>.Success(mappedReportTemplateQuestion);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}