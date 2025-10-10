using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Empresas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Empresas.Queries.GetAllCached
{
    public class GetAllEmpresasCachedQuery : IRequest<Result<List<EmpresaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllEmpresasCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }

    public class GetAllEmpresasCachedQueryHandler : IRequestHandler<GetAllEmpresasCachedQuery, Result<List<EmpresaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IEmpresaCacheRepository _empresaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllEmpresasCachedQueryHandler(IEmpresaCacheRepository empresaCache, IMapper mapper)
        {
            _empresaCache = empresaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<EmpresaCachedResponse>>> Handle(GetAllEmpresasCachedQuery request, CancellationToken cancellationToken)
        {
            var empresaList = await _empresaCache.GetCachedListAsync();
            var mappedEmpresas = _mapper.Map<List<EmpresaCachedResponse>>(empresaList);
            return Result<List<EmpresaCachedResponse>>.Success(mappedEmpresas);
        }


        //---------------------------------------------------------------------------------------------------

    }
}