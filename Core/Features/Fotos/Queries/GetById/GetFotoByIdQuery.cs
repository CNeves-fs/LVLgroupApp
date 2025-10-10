using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Fotos.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fotos.Queries.GetById
{
    public class GetFotoByIdQuery : IRequest<Result<FotoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetFotoByIdQueryHandler : IRequestHandler<GetFotoByIdQuery, Result<FotoCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IFotoCacheRepository _fotoCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetFotoByIdQueryHandler(IFotoCacheRepository fotoCache, IMapper mapper)
            {
                _fotoCache = fotoCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<FotoCachedResponse>> Handle(GetFotoByIdQuery query, CancellationToken cancellationToken)
            {
                var foto = await _fotoCache.GetByIdAsync(query.Id);
                var mappedFoto = _mapper.Map<FotoCachedResponse>(foto);
                return Result<FotoCachedResponse>.Success(mappedFoto);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}