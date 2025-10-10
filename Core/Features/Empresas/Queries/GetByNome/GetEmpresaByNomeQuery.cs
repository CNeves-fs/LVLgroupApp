using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Empresas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Empresas.Queries.GetByNome
{
    public class GetEmpresaByNomeQuery : IRequest<Result<EmpresaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Nome { get; set; }


        public class GetEmpresaByNomeQueryHandler : IRequestHandler<GetEmpresaByNomeQuery, Result<EmpresaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IEmpresaCacheRepository _empresaCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetEmpresaByNomeQueryHandler(IEmpresaCacheRepository empresaCache, IMapper mapper)
            {
                _empresaCache = empresaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<EmpresaCachedResponse>> Handle(GetEmpresaByNomeQuery query, CancellationToken cancellationToken)
            {
                var empresa = await _empresaCache.GetByNomeAsync(query.Nome);
                var mappedEmpresa = _mapper.Map<EmpresaCachedResponse>(empresa);
                return Result<EmpresaCachedResponse>.Success(mappedEmpresa);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}