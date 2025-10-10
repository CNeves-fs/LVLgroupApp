using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Report.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportByLojaId.Queries.GetAllCached
{
    public class GetAllReportByLojaIdCachedQuery : IRequest<Result<List<ReportCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int lojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllReportByLojaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllReportByLojaIdCachedQueryHandler : IRequestHandler<GetAllReportByLojaIdCachedQuery, Result<List<ReportCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportCacheRepository _reportCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllReportByLojaIdCachedQueryHandler(IReportCacheRepository reportCache, IMapper mapper)
        {
            _reportCache = reportCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportCachedResponse>>> Handle(GetAllReportByLojaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var reportList = await _reportCache.GetByLojaIdCachedListAsync(request.lojaId);
            var mappedReportList = _mapper.Map<List<ReportCachedResponse>>(reportList);
            return Result<List<ReportCachedResponse>>.Success(mappedReportList);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}