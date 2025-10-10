using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTypes.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypes.Queries.GetAllCached
{
    public class GetAllReportTypesCachedQuery : IRequest<Result<List<ReportTypeCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllReportTypesCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllReportTypesCachedQueryHandler : IRequestHandler<GetAllReportTypesCachedQuery, Result<List<ReportTypeCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTypeCacheRepository _reportTypeCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTypesCachedQueryHandler(IReportTypeCacheRepository reportTypeCache, IMapper mapper)
        {
            _reportTypeCache = reportTypeCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportTypeCachedResponse>>> Handle(GetAllReportTypesCachedQuery request, CancellationToken cancellationToken)
        {
            var reportTypeList = await _reportTypeCache.GetCachedListAsync();
            var mappedReportTypes = _mapper.Map<List<ReportTypeCachedResponse>>(reportTypeList);
            return Result<List<ReportTypeCachedResponse>>.Success(mappedReportTypes);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}