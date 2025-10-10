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
    public class GetOcorrenciasByEmpresaIdQuery : IRequest<Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int EmpresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetOcorrenciasByEmpresaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetOcorrenciasByEmpresaIdQueryHandler : IRequestHandler<GetOcorrenciasByEmpresaIdQuery, Result<List<OcorrenciaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IOcorrenciaCacheRepository _ocorrenciaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetOcorrenciasByEmpresaIdQueryHandler(IOcorrenciaCacheRepository ocorrenciaCache, IMapper mapper)
        {
            _ocorrenciaCache = ocorrenciaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<OcorrenciaCachedResponse>>> Handle(GetOcorrenciasByEmpresaIdQuery query, CancellationToken cancellationToken)
        {
            var ocorrenciasList = await _ocorrenciaCache.GetByEmpresaIdCachedListAsync(query.EmpresaId);
            var mappedOcorrencias = _mapper.Map<List<OcorrenciaCachedResponse>>(ocorrenciasList);
            return Result<List<OcorrenciaCachedResponse>>.Success(mappedOcorrencias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}