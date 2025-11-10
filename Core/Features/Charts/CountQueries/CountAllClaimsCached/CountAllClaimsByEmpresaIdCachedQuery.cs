using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Charts;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.ChartCacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Charts.CountQueries.CountAllClaimsCached
{
    public class CountAllClaimsByEmpresaIdCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------

        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CountAllClaimsByEmpresaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountAllClaimsByEmpresaIdCachedQueryHandler : IRequestHandler<CountAllClaimsByEmpresaIdCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimChartCacheRepository _claimhartCache;
        private readonly IEmpresaCacheRepository _empresaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountAllClaimsByEmpresaIdCachedQueryHandler(IClaimChartCacheRepository claimhartCache, IEmpresaCacheRepository empresaCache,  IMapper mapper)
        {
            _claimhartCache = claimhartCache;
            _empresaCache = empresaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountAllClaimsByEmpresaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var claimCount = await _claimhartCache.CountClaimsByEmpresaIdAsync(request.empresaId);
            var emp = await _empresaCache.GetByIdAsync(request.empresaId);
            claimCount.Entity = emp.Nome;
            return Result<ChartPoint>.Success(claimCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}