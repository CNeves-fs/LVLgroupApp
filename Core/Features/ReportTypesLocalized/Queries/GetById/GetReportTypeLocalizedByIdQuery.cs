using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.ReportTypesLocalized.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypesLocalized.Queries.GetById
{
    public class GetReportTypeLocalizedByIdQuery : IRequest<Result<ReportTypeLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetReportTypeLocalizedByIdQueryHandler : IRequestHandler<GetReportTypeLocalizedByIdQuery, Result<ReportTypeLocalizedCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IReportTypeLocalizedCacheRepository _tipoOcorrenciaLocalizedCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetReportTypeLocalizedByIdQueryHandler(IReportTypeLocalizedCacheRepository tipoOcorrenciaLocalizedCache, IMapper mapper)
            {
                _tipoOcorrenciaLocalizedCache = tipoOcorrenciaLocalizedCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ReportTypeLocalizedCachedResponse>> Handle(GetReportTypeLocalizedByIdQuery query, CancellationToken cancellationToken)
            {
                var tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedCache.GetByIdAsync(query.Id);
                var mappedReportTypeLocalized = _mapper.Map<ReportTypeLocalizedCachedResponse>(tipoOcorrenciaLocalized);
                return Result<ReportTypeLocalizedCachedResponse>.Success(mappedReportTypeLocalized);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}