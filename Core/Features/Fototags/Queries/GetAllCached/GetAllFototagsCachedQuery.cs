using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Fototags.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fototags.Queries.GetAllCached
{
    public class GetAllFototagsCachedQuery : IRequest<Result<List<FototagCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllFototagsCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllFototagsCachedQueryHandler : IRequestHandler<GetAllFototagsCachedQuery, Result<List<FototagCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IFototagCacheRepository _fototagCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllFototagsCachedQueryHandler(IFototagCacheRepository fototagCache, IMapper mapper)
        {
            _fototagCache = fototagCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<FototagCachedResponse>>> Handle(GetAllFototagsCachedQuery request, CancellationToken cancellationToken)
        {
            var fototagList = await _fototagCache.GetCachedListAsync();
            var mappedFototags = _mapper.Map<List<FototagCachedResponse>>(fototagList);
            return Result<List<FototagCachedResponse>>.Success(mappedFototags);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}