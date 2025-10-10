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
    public class GetAllReportTemplateQuestionByQuestionCachedQuery : IRequest<Result<List<ReportTemplateQuestionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int questionTemplateId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTemplateQuestionByQuestionCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllReportTemplateQuestionByQuestionCachedQueryHandler : IRequestHandler<GetAllReportTemplateQuestionByQuestionCachedQuery, Result<List<ReportTemplateQuestionCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTemplateQuestionCacheRepository _reportTemplateQuestionCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTemplateQuestionByQuestionCachedQueryHandler(IReportTemplateQuestionCacheRepository reportTemplateQuestionCache, IMapper mapper)
        {
            _reportTemplateQuestionCache = reportTemplateQuestionCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportTemplateQuestionCachedResponse>>> Handle(GetAllReportTemplateQuestionByQuestionCachedQuery request, CancellationToken cancellationToken)
        {
            var reportTemplateQuestionList = await _reportTemplateQuestionCache.GetByQuestionTemplateIdCachedListAsync(request.questionTemplateId);
            var mappedReportTemplateQuestion = _mapper.Map<List<ReportTemplateQuestionCachedResponse>>(reportTemplateQuestionList);
            return Result<List<ReportTemplateQuestionCachedResponse>>.Success(mappedReportTemplateQuestion);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}