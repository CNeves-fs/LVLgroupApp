using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Report.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Report.Queries.GetById
{
    public class GetReportByIdQuery : IRequest<Result<ReportCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, Result<ReportCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportCacheRepository _reportCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetReportByIdQueryHandler(IReportCacheRepository reportCache, IMapper mapper)
            {
                _reportCache = reportCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ReportCachedResponse>> Handle(GetReportByIdQuery query, CancellationToken cancellationToken)
            {
                var report = await _reportCache.GetByIdAsync(query.Id);
                var mappedReport = _mapper.Map<ReportCachedResponse>(report);
                return Result<ReportCachedResponse>.Success(mappedReport);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}