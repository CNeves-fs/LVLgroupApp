using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetByVendaSemanalId
{
    public class GetVendasDiariasByVendaSemanalIdQuery : IRequest<Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int VendaSemanalId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasByVendaSemanalIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasDiariasByVendaSemanalIdQueryHandler : IRequestHandler<GetVendasDiariasByVendaSemanalIdQuery, Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaDiariaCacheRepository _vendaDiariaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasByVendaSemanalIdQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
        {
            _vendaDiariaCache = vendaDiariaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaDiariaCachedResponse>>> Handle(GetVendasDiariasByVendaSemanalIdQuery query, CancellationToken cancellationToken)
        {
            var vendaDiariaList = await _vendaDiariaCache.GetByVendaSemanalIdAsync(query.VendaSemanalId);
            var mappedVendasDiarias = _mapper.Map<List<VendaDiariaCachedResponse>>(vendaDiariaList);
            return Result<List<VendaDiariaCachedResponse>>.Success(mappedVendasDiarias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}