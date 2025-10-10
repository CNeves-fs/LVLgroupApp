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
    public class GetAllClaimsByEmpresaIdCachedQuery : IRequest<Result<List<ClaimCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsByEmpresaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllClaimsByEmpresaIdCachedQueryHandler : IRequestHandler<GetAllClaimsByEmpresaIdCachedQuery, Result<List<ClaimCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IClaimCacheRepository _claimCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllClaimsByEmpresaIdCachedQueryHandler(IClaimCacheRepository claimCache, IMapper mapper)
        {
            _claimCache = claimCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ClaimCachedResponse>>> Handle(GetAllClaimsByEmpresaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var claimList = await _claimCache.GetByEmpresaIdAsync(request.empresaId);
            var mappedClaims = _mapper.Map<List<ClaimCachedResponse>>(claimList);
            return Result<List<ClaimCachedResponse>>.Success(mappedClaims);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}