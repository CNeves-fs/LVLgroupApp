using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetByMercadoId
{
    public class GetVendasDiariasByMercadoIdMesQuery : IRequest<Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int MercadoId { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasByMercadoIdMesQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasDiariasByMercadoIdMesQueryHandler : IRequestHandler<GetVendasDiariasByMercadoIdMesQuery, Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaDiariaCacheRepository _vendaDiariaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasByMercadoIdMesQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
        {
            _vendaDiariaCache = vendaDiariaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaDiariaCachedResponse>>> Handle(GetVendasDiariasByMercadoIdMesQuery query, CancellationToken cancellationToken)
        {
            var vendaDiariaList = await _vendaDiariaCache.GetByMercadoIdMesAsync(query.MercadoId, query.Ano, query.Mes);
            var mappedVendasDiarias = _mapper.Map<List<VendaDiariaCachedResponse>>(vendaDiariaList);
            return Result<List<VendaDiariaCachedResponse>>.Success(mappedVendasDiarias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}