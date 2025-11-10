using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.TiposOcorrencias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrencias.Queries.GetById
{
    public class GetTipoOcorrenciaByIdQuery : IRequest<Result<TipoOcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetTipoOcorrenciaByIdQueryHandler : IRequestHandler<GetTipoOcorrenciaByIdQuery, Result<TipoOcorrenciaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly ITipoOcorrenciaCacheRepository _tipoOcorrenciaCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetTipoOcorrenciaByIdQueryHandler(ITipoOcorrenciaCacheRepository tipoOcorrenciaCache, IMapper mapper)
            {
                _tipoOcorrenciaCache = tipoOcorrenciaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<TipoOcorrenciaCachedResponse>> Handle(GetTipoOcorrenciaByIdQuery query, CancellationToken cancellationToken)
            {
                var tipoOcorrencia = await _tipoOcorrenciaCache.GetByIdAsync(query.Id);
                var mappedTipoOcorrencia = _mapper.Map<TipoOcorrenciaCachedResponse>(tipoOcorrencia);
                return Result<TipoOcorrenciaCachedResponse>.Success(mappedTipoOcorrencia);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}