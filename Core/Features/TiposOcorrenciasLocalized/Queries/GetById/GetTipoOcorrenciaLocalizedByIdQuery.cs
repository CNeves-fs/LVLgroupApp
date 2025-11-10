using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.TiposOcorrenciasLocalized.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrencias.Queries.GetById
{
    public class GetTipoOcorrenciaLocalizedByIdQuery : IRequest<Result<TipoOcorrenciaLocalizedCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetTipoOcorrenciaLocalizedByIdQueryHandler : IRequestHandler<GetTipoOcorrenciaLocalizedByIdQuery, Result<TipoOcorrenciaLocalizedCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly ITipoOcorrenciaLocalizedCacheRepository _tipoOcorrenciaLocalizedCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetTipoOcorrenciaLocalizedByIdQueryHandler(ITipoOcorrenciaLocalizedCacheRepository tipoOcorrenciaLocalizedCache, IMapper mapper)
            {
                _tipoOcorrenciaLocalizedCache = tipoOcorrenciaLocalizedCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<TipoOcorrenciaLocalizedCachedResponse>> Handle(GetTipoOcorrenciaLocalizedByIdQuery query, CancellationToken cancellationToken)
            {
                var tipoOcorrenciaLocalized = await _tipoOcorrenciaLocalizedCache.GetByIdAsync(query.Id);
                var mappedTipoOcorrenciaLocalized = _mapper.Map<TipoOcorrenciaLocalizedCachedResponse>(tipoOcorrenciaLocalized);
                return Result<TipoOcorrenciaLocalizedCachedResponse>.Success(mappedTipoOcorrenciaLocalized);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}