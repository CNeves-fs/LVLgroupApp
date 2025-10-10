using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Artigos.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Artigos.Queries.GetById
{
    public class GetArtigoByIdQuery : IRequest<Result<ArtigoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetArtigoByIdQueryHandler : IRequestHandler<GetArtigoByIdQuery, Result<ArtigoCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IArtigoCacheRepository _artigoCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetArtigoByIdQueryHandler(IArtigoCacheRepository artigoCache, IMapper mapper)
            {
                _artigoCache = artigoCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<ArtigoCachedResponse>> Handle(GetArtigoByIdQuery query, CancellationToken cancellationToken)
            {
                var artigo = await _artigoCache.GetByIdAsync(query.Id);
                var mappedArtigo = _mapper.Map<ArtigoCachedResponse>(artigo);
                return Result<ArtigoCachedResponse>.Success(mappedArtigo);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}