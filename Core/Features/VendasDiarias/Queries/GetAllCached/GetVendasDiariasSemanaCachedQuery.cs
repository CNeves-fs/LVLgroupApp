using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetAllCached
{
    public class GetVendasDiariasSemanaCachedQuery : IRequest<Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Ano { get; set; }

        public int Semana { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasSemanaCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasDiariasSemanaCachedQueryHandler : IRequestHandler<GetVendasDiariasSemanaCachedQuery, Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaDiariaCacheRepository _vendaDiariaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasSemanaCachedQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
        {
            _vendaDiariaCache = vendaDiariaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaDiariaCachedResponse>>> Handle(GetVendasDiariasSemanaCachedQuery request, CancellationToken cancellationToken)
        {
            var vendaDiariaList = await _vendaDiariaCache.GetBySemanaAsync(request.Ano, request.Semana);
            var mappedVendasDiarias = _mapper.Map<List<VendaDiariaCachedResponse>>(vendaDiariaList);
            return Result<List<VendaDiariaCachedResponse>>.Success(mappedVendasDiarias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}