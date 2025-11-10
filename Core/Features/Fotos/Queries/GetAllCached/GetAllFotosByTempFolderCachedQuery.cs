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
    public class GetAllFotosByTempFolderCachedQuery : IRequest<Result<List<FotoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public string tempFolder { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllFotosByTempFolderCachedQuery()
        {

        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllFotosByTempFolderCachedQueryHandler : IRequestHandler<GetAllFotosByTempFolderCachedQuery, Result<List<FotoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IFotoCacheRepository _fotoCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllFotosByTempFolderCachedQueryHandler(IFotoCacheRepository fotoCache, IMapper mapper)
        {
            _fotoCache = fotoCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<FotoCachedResponse>>> Handle(GetAllFotosByTempFolderCachedQuery request, CancellationToken cancellationToken)
        {
            var fotoList = await _fotoCache.GetByTempFolderCachedListAsync(request.tempFolder);
            var mappedFotos = _mapper.Map<List<FotoCachedResponse>>(fotoList);
            return Result<List<FotoCachedResponse>>.Success(mappedFotos);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}