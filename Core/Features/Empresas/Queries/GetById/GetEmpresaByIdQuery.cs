using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Empresas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Empresas.Queries.GetById
{
    public class GetEmpresaByIdQuery : IRequest<Result<EmpresaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        public class GetEmpresaByIdQueryHandler : IRequestHandler<GetEmpresaByIdQuery, Result<EmpresaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IEmpresaCacheRepository _empresaCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetEmpresaByIdQueryHandler(IEmpresaCacheRepository empresaCache, IMapper mapper)
            {
                _empresaCache = empresaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<EmpresaCachedResponse>> Handle(GetEmpresaByIdQuery query, CancellationToken cancellationToken)
            {
                var empresa = await _empresaCache.GetByIdAsync(query.Id);
                var mappedEmpresa = _mapper.Map<EmpresaCachedResponse>(empresa);
                return Result<EmpresaCachedResponse>.Success(mappedEmpresa);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}