using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Gruposlojas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Gruposlojas.Queries.GetById
{
    public class GetGrupolojaByIdQuery : IRequest<Result<GrupolojasCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetGrupolojaByIdQueryHandler : IRequestHandler<GetGrupolojaByIdQuery, Result<GrupolojasCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IGrupolojaCacheRepository _grupolojaCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetGrupolojaByIdQueryHandler(IGrupolojaCacheRepository grupolojaCache, IMapper mapper)
            {
                _grupolojaCache = grupolojaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<GrupolojasCachedResponse>> Handle(GetGrupolojaByIdQuery query, CancellationToken cancellationToken)
            {
                var grupoloja = await _grupolojaCache.GetByIdAsync(query.Id);
                var mappedGrupoloja = _mapper.Map<GrupolojasCachedResponse>(grupoloja);
                return Result<GrupolojasCachedResponse>.Success(mappedGrupoloja);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}