using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTypes.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypes.Queries.GetById
{
    public class GetReportTypeByIdQuery : IRequest<Result<ReportTypeCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetReportTypeByIdQueryHandler : IRequestHandler<GetReportTypeByIdQuery, Result<ReportTypeCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportTypeCacheRepository _reportTypeCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetReportTypeByIdQueryHandler(IReportTypeCacheRepository reportTypeCache, IMapper mapper)
            {
                _reportTypeCache = reportTypeCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ReportTypeCachedResponse>> Handle(GetReportTypeByIdQuery query, CancellationToken cancellationToken)
            {
                var reportType = await _reportTypeCache.GetByIdAsync(query.Id);
                var mappedReportType = _mapper.Map<ReportTypeCachedResponse>(reportType);
                return Result<ReportTypeCachedResponse>.Success(mappedReportType);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}