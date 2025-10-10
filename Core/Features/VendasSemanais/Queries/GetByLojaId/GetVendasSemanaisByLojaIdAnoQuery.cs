using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasSemanais.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasSemanais.Queries.GetByLojaId
{
    public class GetVendasSemanaisByLojaIdAnoQuery : IRequest<Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int LojaId { get; set; }

        public int Ano { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByLojaIdAnoQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasSemanaisByLojaIdAnoQueryHandler : IRequestHandler<GetVendasSemanaisByLojaIdAnoQuery, Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaSemanalCacheRepository _vendaSemanalCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByLojaIdAnoQueryHandler(IVendaSemanalCacheRepository vendaSemanalCache, IMapper mapper)
        {
            _vendaSemanalCache = vendaSemanalCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaSemanalCachedResponse>>> Handle(GetVendasSemanaisByLojaIdAnoQuery query, CancellationToken cancellationToken)
        {
            var vendaSemanalList = await _vendaSemanalCache.GetByLojaIdAnoAsync(query.LojaId, query.Ano);
            var mappedVendasSemanais = _mapper.Map<List<VendaSemanalCachedResponse>>(vendaSemanalList);
            return Result<List<VendaSemanalCachedResponse>>.Success(mappedVendasSemanais);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}