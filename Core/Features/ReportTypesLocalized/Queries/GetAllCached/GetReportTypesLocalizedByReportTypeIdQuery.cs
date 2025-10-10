using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTypesLocalized.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypesLocalized.Queries.GetAllCached
{
    public class GetReportTypesLocalizedByReportTypeIdQuery : IRequest<Result<List<ReportTypeLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int ReportTypeId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetReportTypesLocalizedByReportTypeIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetReportTypesLocalizedByReportTypeIdQueryHandler : IRequestHandler<GetReportTypesLocalizedByReportTypeIdQuery, Result<List<ReportTypeLocalizedCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTypeLocalizedCacheRepository _reportTypeLocalizedCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetReportTypesLocalizedByReportTypeIdQueryHandler(IReportTypeLocalizedCacheRepository reportTypeLocalizedCache, IMapper mapper)
        {
            _reportTypeLocalizedCache = reportTypeLocalizedCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ReportTypeLocalizedCachedResponse>>> Handle(GetReportTypesLocalizedByReportTypeIdQuery query, CancellationToken cancellationToken)
        {
            var tiposOcorrenciasLocalizedList = await _reportTypeLocalizedCache.GetByReportTypeIdAsync(query.ReportTypeId);
            var mappedReportTypesLocalized = _mapper.Map<List<ReportTypeLocalizedCachedResponse>>(tiposOcorrenciasLocalizedList);
            return Result<List<ReportTypeLocalizedCachedResponse>>.Success(mappedReportTypesLocalized);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}