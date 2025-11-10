using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Claims.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Claims.Queries.GetById
{
    public class GetClaimByIdQuery : IRequest<Result<ClaimCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, Result<ClaimCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IClaimCacheRepository _claimCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetClaimByIdQueryHandler(IClaimCacheRepository claimCache, IMapper mapper)
            {
                _claimCache = claimCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ClaimCachedResponse>> Handle(GetClaimByIdQuery query, CancellationToken cancellationToken)
            {
                var claim = await _claimCache.GetByIdAsync(query.Id);
                var mappedClaim = _mapper.Map<ClaimCachedResponse>(claim);
                return Result<ClaimCachedResponse>.Success(mappedClaim);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}