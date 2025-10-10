using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTemplateQuestion.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplateQuestion.Queries.GetById
{
    public class GetReportTemplateQuestionByIdQuery : IRequest<Result<ReportTemplateQuestionCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetReportTemplateQuestionByIdQueryHandler : IRequestHandler<GetReportTemplateQuestionByIdQuery, Result<ReportTemplateQuestionCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportTemplateQuestionCacheRepository _reportTemplateQuestionCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetReportTemplateQuestionByIdQueryHandler(IReportTemplateQuestionCacheRepository reportTemplateQuestionCache, IMapper mapper)
            {
                _reportTemplateQuestionCache = reportTemplateQuestionCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ReportTemplateQuestionCachedResponse>> Handle(GetReportTemplateQuestionByIdQuery query, CancellationToken cancellationToken)
            {
                var reportTemplateQuestion = await _reportTemplateQuestionCache.GetByIdAsync(query.Id);
                var mappedReportTemplateQuestion = _mapper.Map<ReportTemplateQuestionCachedResponse>(reportTemplateQuestion);
                return Result<ReportTemplateQuestionCachedResponse>.Success(mappedReportTemplateQuestion);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}