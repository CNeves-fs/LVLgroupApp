using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTemplate.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTemplate.Queries.GetById
{
    public class GetReportTemplateByIdQuery : IRequest<Result<ReportTemplateCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetReportTemplateByIdQueryHandler : IRequestHandler<GetReportTemplateByIdQuery, Result<ReportTemplateCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportTemplateCacheRepository _reportTemplateCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetReportTemplateByIdQueryHandler(IReportTemplateCacheRepository reportTemplateCache, IMapper mapper)
            {
                _reportTemplateCache = reportTemplateCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ReportTemplateCachedResponse>> Handle(GetReportTemplateByIdQuery query, CancellationToken cancellationToken)
            {
                var reportTemplate = await _reportTemplateCache.GetByIdAsync(query.Id);
                var mappedReportTemplate = _mapper.Map<ReportTemplateCachedResponse>(reportTemplate);
                return Result<ReportTemplateCachedResponse>.Success(mappedReportTemplate);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}