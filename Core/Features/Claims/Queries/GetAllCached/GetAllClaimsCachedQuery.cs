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
    public class GetAllClaimsCachedQuery : IRequest<Result<List<ClaimCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllClaimsCachedQueryHandler : IRequestHandler<GetAllClaimsCachedQuery, Result<List<ClaimCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimCacheRepository _claimCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsCachedQueryHandler(IClaimCacheRepository claimCache, IMapper mapper)
        {
            _claimCache = claimCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ClaimCachedResponse>>> Handle(GetAllClaimsCachedQuery request, CancellationToken cancellationToken)
        {
            var claimList = await _claimCache.GetCachedListAsync();
            var mappedClaims = _mapper.Map<List<ClaimCachedResponse>>(claimList);
            return Result<List<ClaimCachedResponse>>.Success(mappedClaims);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}