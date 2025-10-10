using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Charts;
using Core.Features.Claims.Response;
using Core.Interfaces.CacheRepositories;
using Core.Interfaces.ChartCacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Charts.CountQueries.CountAllClaimsCached
{
    public class CountAllClaimsByGrupolojaIdCachedQuery : IRequest<Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------

        public int grupolojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public CountAllClaimsByGrupolojaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class CountAllClaimsByGrupolojaIdCachedQueryHandler : IRequestHandler<CountAllClaimsByGrupolojaIdCachedQuery, Result<ChartPoint>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimChartCacheRepository _claimhartCache;
        private readonly IGrupolojaCacheRepository _grupolojaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public CountAllClaimsByGrupolojaIdCachedQueryHandler(IClaimChartCacheRepository claimhartCache, IGrupolojaCacheRepository grupolojaCache, IMapper mapper)
        {
            _claimhartCache = claimhartCache;
            _grupolojaCache = grupolojaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<ChartPoint>> Handle(CountAllClaimsByGrupolojaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var claimCount = await _claimhartCache.CountClaimsByGrupolojaIdAsync(request.grupolojaId);
            var gl = await _grupolojaCache.GetByIdAsync(request.grupolojaId);
            claimCount.Entity = gl.Nome;
            return Result<ChartPoint>.Success(claimCount);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}