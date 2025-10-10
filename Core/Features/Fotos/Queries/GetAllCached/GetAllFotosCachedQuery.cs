using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Fotos.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fotos.Queries.GetAllCached
{
    public class GetAllFotosCachedQuery : IRequest<Result<List<FotoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllFotosCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllFotosCachedQueryHandler : IRequestHandler<GetAllFotosCachedQuery, Result<List<FotoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IFotoCacheRepository _fotoCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllFotosCachedQueryHandler(IFotoCacheRepository fotoCache, IMapper mapper)
        {
            _fotoCache = fotoCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<FotoCachedResponse>>> Handle(GetAllFotosCachedQuery request, CancellationToken cancellationToken)
        {
            var fotoList = await _fotoCache.GetCachedListAsync();
            var mappedFotos = _mapper.Map<List<FotoCachedResponse>>(fotoList);
            return Result<List<FotoCachedResponse>>.Success(mappedFotos);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}