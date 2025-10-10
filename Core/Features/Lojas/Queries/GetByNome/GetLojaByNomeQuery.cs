using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Lojas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Lojas.Queries.GetByNome
{
    public class GetLojaByNomeQuery : IRequest<Result<LojaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public string Nome { get; set; }


        public class GetLojaByNomeQueryHandler : IRequestHandler<GetLojaByNomeQuery, Result<LojaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly ILojaCacheRepository _lojaCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetLojaByNomeQueryHandler(ILojaCacheRepository lojaCache, IMapper mapper)
            {
                _lojaCache = lojaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<LojaCachedResponse>> Handle(GetLojaByNomeQuery query, CancellationToken cancellationToken)
            {
                var loja = await _lojaCache.GetByNomeAsync(query.Nome);
                var mappedLoja = _mapper.Map<LojaCachedResponse>(loja);
                return Result<LojaCachedResponse>.Success(mappedLoja);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}