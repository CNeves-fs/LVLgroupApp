using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Artigos.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Artigos.Queries.GetByRef
{
    public class GetArtigoByReferenciaQuery : IRequest<Result<ArtigoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Referencia { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetArtigoByReferenciaQueryHandler : IRequestHandler<GetArtigoByReferenciaQuery, Result<ArtigoCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IArtigoCacheRepository _artigoCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetArtigoByReferenciaQueryHandler(IArtigoCacheRepository artigoCache, IMapper mapper)
            {
                _artigoCache = artigoCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ArtigoCachedResponse>> Handle(GetArtigoByReferenciaQuery query, CancellationToken cancellationToken)
            {
                var artigo = await _artigoCache.GetByReferenciaAsync(query.Referencia);
                var mappedArtigo = _mapper.Map<ArtigoCachedResponse>(artigo);
                return Result<ArtigoCachedResponse>.Success(mappedArtigo);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}