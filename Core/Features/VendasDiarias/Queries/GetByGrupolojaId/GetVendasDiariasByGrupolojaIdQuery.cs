using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetByGrupolojaId
{
    public class GetVendasDiariasByGrupolojaIdQuery : IRequest<Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int GrupolojaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasByGrupolojaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasDiariasByGrupolojaIdQueryHandler : IRequestHandler<GetVendasDiariasByGrupolojaIdQuery, Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaDiariaCacheRepository _vendaDiariaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasByGrupolojaIdQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
        {
            _vendaDiariaCache = vendaDiariaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaDiariaCachedResponse>>> Handle(GetVendasDiariasByGrupolojaIdQuery query, CancellationToken cancellationToken)
        {
            var vendaDiariaList = await _vendaDiariaCache.GetByGrupolojaIdAsync(query.GrupolojaId);
            var mappedVendasDiarias = _mapper.Map<List<VendaDiariaCachedResponse>>(vendaDiariaList);
            return Result<List<VendaDiariaCachedResponse>>.Success(mappedVendasDiarias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}