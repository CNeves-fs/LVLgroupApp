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
    public class GetOcorrenciasByLojaIdQuery : IRequest<Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int LojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetOcorrenciasByLojaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetOcorrenciasByLojaIdQueryHandler : IRequestHandler<GetOcorrenciasByLojaIdQuery, Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaCacheRepository _ocorrenciaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetOcorrenciasByLojaIdQueryHandler(IOcorrenciaCacheRepository ocorrenciaCache, IMapper mapper)
        {
            _ocorrenciaCache = ocorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<OcorrenciaCachedResponse>>> Handle(GetOcorrenciasByLojaIdQuery query, CancellationToken cancellationToken)
        {
            var ocorrenciasList = await _ocorrenciaCache.GetByLojaIdCachedListAsync(query.LojaId);
            var mappedOcorrencias = _mapper.Map<List<OcorrenciaCachedResponse>>(ocorrenciasList);
            return Result<List<OcorrenciaCachedResponse>>.Success(mappedOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}