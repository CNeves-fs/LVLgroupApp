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
    public class GetAllOcorenciasCachedQuery : IRequest<Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllOcorenciasCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllOcorrenciasCachedQueryHandler : IRequestHandler<GetAllOcorenciasCachedQuery, Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaCacheRepository _ocorrenciaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllOcorrenciasCachedQueryHandler(IOcorrenciaCacheRepository ocorrenciaCache, IMapper mapper)
        {
            _ocorrenciaCache = ocorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<OcorrenciaCachedResponse>>> Handle(GetAllOcorenciasCachedQuery request, CancellationToken cancellationToken)
        {
            var ocorrenciaList = await _ocorrenciaCache.GetCachedListAsync();
            var mappedOcorrencias = _mapper.Map<List<OcorrenciaCachedResponse>>(ocorrenciaList);
            return Result<List<OcorrenciaCachedResponse>>.Success(mappedOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}