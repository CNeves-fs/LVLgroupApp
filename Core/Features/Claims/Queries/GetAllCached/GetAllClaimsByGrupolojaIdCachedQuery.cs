using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Claims.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Claims.Queries.GetAllCached
{
    public class GetAllClaimsByGrupolojaIdCachedQuery : IRequest<Result<List<ClaimCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int grupolojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsByGrupolojaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllClaimsByGrupolojaIdCachedQueryHandler : IRequestHandler<GetAllClaimsByGrupolojaIdCachedQuery, Result<List<ClaimCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimCacheRepository _claimCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsByGrupolojaIdCachedQueryHandler(IClaimCacheRepository claimCache, IMapper mapper)
        {
            _claimCache = claimCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ClaimCachedResponse>>> Handle(GetAllClaimsByGrupolojaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var claimList = await _claimCache.GetByGrupolojaIdAsync(request.grupolojaId);
            var mappedClaims = _mapper.Map<List<ClaimCachedResponse>>(claimList);
            return Result<List<ClaimCachedResponse>>.Success(mappedClaims);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}