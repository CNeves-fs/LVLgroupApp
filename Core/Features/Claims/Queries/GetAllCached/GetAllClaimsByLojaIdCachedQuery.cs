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
    public class GetAllClaimsByLojaIdCachedQuery : IRequest<Result<List<ClaimCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int lojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsByLojaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllClaimsByLojaIdCachedQueryHandler : IRequestHandler<GetAllClaimsByLojaIdCachedQuery, Result<List<ClaimCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimCacheRepository _claimCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsByLojaIdCachedQueryHandler(IClaimCacheRepository claimCache, IMapper mapper)
        {
            _claimCache = claimCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ClaimCachedResponse>>> Handle(GetAllClaimsByLojaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var claimList = await _claimCache.GetByLojaIdAsync(request.lojaId);
            var mappedClaims = _mapper.Map<List<ClaimCachedResponse>>(claimList);
            return Result<List<ClaimCachedResponse>>.Success(mappedClaims);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}