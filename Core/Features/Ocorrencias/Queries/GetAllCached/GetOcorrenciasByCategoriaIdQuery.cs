using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Ocorrencias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Ocorrencias.Queries.GetAllCached
{
    public class GetOcorrenciasByCategoriaIdQuery : IRequest<Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int CategoriaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetOcorrenciasByCategoriaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetOcorrenciasByCategoriaIdQueryHandler : IRequestHandler<GetOcorrenciasByCategoriaIdQuery, Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaCacheRepository _ocorrenciaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetOcorrenciasByCategoriaIdQueryHandler(IOcorrenciaCacheRepository ocorrenciaCache, IMapper mapper)
        {
            _ocorrenciaCache = ocorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<OcorrenciaCachedResponse>>> Handle(GetOcorrenciasByCategoriaIdQuery query, CancellationToken cancellationToken)
        {
            var ocorrenciasList = await _ocorrenciaCache.GetByCategoriaIdCachedListAsync(query.CategoriaId);
            var mappedOcorrencias = _mapper.Map<List<OcorrenciaCachedResponse>>(ocorrenciasList);
            return Result<List<OcorrenciaCachedResponse>>.Success(mappedOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}