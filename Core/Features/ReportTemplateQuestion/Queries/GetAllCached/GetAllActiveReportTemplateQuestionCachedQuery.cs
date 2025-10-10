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
    public class GetAllActiveReportTemplateQuestionCachedQuery : IRequest<Result<List<ReportTemplateQuestionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllActiveReportTemplateQuestionCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllActiveReportTemplateQuestionCachedQueryHandler : IRequestHandler<GetAllActiveReportTemplateQuestionCachedQuery, Result<List<ReportTemplateQuestionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateQuestionCacheRepository _reportTemplateQuestionCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllActiveReportTemplateQuestionCachedQueryHandler(IReportTemplateQuestionCacheRepository reportTemplateQuestionCache, IMapper mapper)
        {
            _reportTemplateQuestionCache = reportTemplateQuestionCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportTemplateQuestionCachedResponse>>> Handle(GetAllActiveReportTemplateQuestionCachedQuery request, CancellationToken cancellationToken)
        {
            var reportTemplateQuestionList = await _reportTemplateQuestionCache.GetCachedAllActiveReportTemplateQuestionAsync();
            var mappedReportTemplateQuestion = _mapper.Map<List<ReportTemplateQuestionCachedResponse>>(reportTemplateQuestionList);
            return Result<List<ReportTemplateQuestionCachedResponse>>.Success(mappedReportTemplateQuestion);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}