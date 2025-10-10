using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Lojas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Lojas.Queries.GetById
{
    public class GetLojaByIdQuery : IRequest<Result<LojaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetLojaByIdQueryHandler : IRequestHandler<GetLojaByIdQuery, Result<LojaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly ILojaCacheRepository _lojaCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetLojaByIdQueryHandler(ILojaCacheRepository lojaCache, IMapper mapper)
            {
                _lojaCache = lojaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<LojaCachedResponse>> Handle(GetLojaByIdQuery query, CancellationToken cancellationToken)
            {
                var loja = await _lojaCache.GetByIdAsync(query.Id);
                var mappedLoja = _mapper.Map<LojaCachedResponse>(loja);
                return Result<LojaCachedResponse>.Success(mappedLoja);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}