using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Prazoslimite.Queries.GetAllCached;
using Core.Features.Statuss.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Statuss.Queries.GetAllCached
{
    public class GetAllStatusCachedQuery : IRequest<Result<List<StatusCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllStatusCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllStatusCachedQueryHandler : IRequestHandler<GetAllStatusCachedQuery, Result<List<StatusCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStatusCacheRepository _statusCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllStatusCachedQueryHandler(IStatusCacheRepository statusCache, IMapper mapper)
        {
            _statusCache = statusCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<StatusCachedResponse>>> Handle(GetAllStatusCachedQuery request, CancellationToken cancellationToken)
        {
            var statusList = await _statusCache.GetCachedListAsync();
            var mappedStatus = _mapper.Map<List<StatusCachedResponse>>(statusList);
            return Result<List<StatusCachedResponse>>.Success(mappedStatus);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}