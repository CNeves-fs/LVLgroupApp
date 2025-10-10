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
    public class GetAllFotosAllTempFolderCachedQuery : IRequest<Result<List<FotoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllFotosAllTempFolderCachedQuery()
        {

        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllFotosAllTempFolderCachedQueryHandler : IRequestHandler<GetAllFotosAllTempFolderCachedQuery, Result<List<FotoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IFotoCacheRepository _fotoCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllFotosAllTempFolderCachedQueryHandler(IFotoCacheRepository fotoCache, IMapper mapper)
        {
            _fotoCache = fotoCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<FotoCachedResponse>>> Handle(GetAllFotosAllTempFolderCachedQuery request, CancellationToken cancellationToken)
        {
            var fotoList = await _fotoCache.GetAllTempFolderCachedListAsync();
            var mappedFotos = _mapper.Map<List<FotoCachedResponse>>(fotoList);
            return Result<List<FotoCachedResponse>>.Success(mappedFotos);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}