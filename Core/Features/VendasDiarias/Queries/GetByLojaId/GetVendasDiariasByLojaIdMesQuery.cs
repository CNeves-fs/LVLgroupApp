using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetByLojaId
{
    public class GetVendasDiariasByLojaIdMesQuery : IRequest<Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int LojaId { get; set; }

        public int Ano { get; set; }

        public int Mês { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetVendasDiariasByLojaIdMesQueryHandler : IRequestHandler<GetVendasDiariasByLojaIdMesQuery, Result<List<VendaDiariaCachedResponse>>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IVendaDiariaCacheRepository _vendaDiariaCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetVendasDiariasByLojaIdMesQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
            {
                _vendaDiariaCache = vendaDiariaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<List<VendaDiariaCachedResponse>>> Handle(GetVendasDiariasByLojaIdMesQuery query, CancellationToken cancellationToken)
            {
                var vendasDiarias = await _vendaDiariaCache.GetByLojaIdMesAsync(query.LojaId, query.Ano, query.Mês);
                var mappedVendasDiarias = _mapper.Map<List<VendaDiariaCachedResponse>>(vendasDiarias);
                return Result<List<VendaDiariaCachedResponse>>.Success(mappedVendasDiarias);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}