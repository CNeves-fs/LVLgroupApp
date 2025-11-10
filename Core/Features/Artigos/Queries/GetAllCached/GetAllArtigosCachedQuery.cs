using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Artigos.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Artigos.Queries.GetAllCached
{
    public class GetAllArtigosCachedQuery : IRequest<Result<List<ArtigoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllArtigosCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllArtigosCachedQueryHandler : IRequestHandler<GetAllArtigosCachedQuery, Result<List<ArtigoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IArtigoCacheRepository _artigoCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllArtigosCachedQueryHandler(IArtigoCacheRepository artigoCache, IMapper mapper)
        {
            _artigoCache = artigoCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ArtigoCachedResponse>>> Handle(GetAllArtigosCachedQuery request, CancellationToken cancellationToken)
        {
            var artigoList = await _artigoCache.GetCachedListAsync();
            var mappedArtigos = _mapper.Map<List<ArtigoCachedResponse>>(artigoList);
            return Result<List<ArtigoCachedResponse>>.Success(mappedArtigos);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}