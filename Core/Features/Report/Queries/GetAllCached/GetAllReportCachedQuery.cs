using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Report.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Report.Queries.GetAllCached
{
    public class GetAllReportCachedQuery : IRequest<Result<List<ReportCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllReportCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllReportCachedQueryHandler : IRequestHandler<GetAllReportCachedQuery, Result<List<ReportCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportCacheRepository _reportCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllReportCachedQueryHandler(IReportCacheRepository reportCache, IMapper mapper)
        {
            _reportCache = reportCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportCachedResponse>>> Handle(GetAllReportCachedQuery request, CancellationToken cancellationToken)
        {
            var reportList = await _reportCache.GetCachedAllReportAsync();
            var mappedReport = _mapper.Map<List<ReportCachedResponse>>(reportList);
            return Result<List<ReportCachedResponse>>.Success(mappedReport);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}