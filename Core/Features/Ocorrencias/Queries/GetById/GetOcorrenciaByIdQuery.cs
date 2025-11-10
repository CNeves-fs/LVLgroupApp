using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Ocorrencias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Ocorrencias.Queries.GetById
{
    public class GetOcorrenciaByIdQuery : IRequest<Result<OcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetOcorrenciaByIdQueryHandler : IRequestHandler<GetOcorrenciaByIdQuery, Result<OcorrenciaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IOcorrenciaCacheRepository _ocorrenciaCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetOcorrenciaByIdQueryHandler(IOcorrenciaCacheRepository ocorrenciaCache, IMapper mapper)
            {
                _ocorrenciaCache = ocorrenciaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<OcorrenciaCachedResponse>> Handle(GetOcorrenciaByIdQuery query, CancellationToken cancellationToken)
            {
                var ocorrencia = await _ocorrenciaCache.GetByIdAsync(query.Id);
                var mappedOcorrencia = _mapper.Map<OcorrenciaCachedResponse>(ocorrencia);
                return Result<OcorrenciaCachedResponse>.Success(mappedOcorrencia);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}