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
    public class GetVendaSemanalByLojaIdSemanaQuery : IRequest<Result<VendaSemanalCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int LojaId { get; set; }

        public int Ano { get; set; }

        public int NumeroDaSemana { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendaSemanalByLojaIdSemanaQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendaSemanalByLojaIdSemanaQueryHandler : IRequestHandler<GetVendaSemanalByLojaIdSemanaQuery, Result<VendaSemanalCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaSemanalCacheRepository _vendaSemanalCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendaSemanalByLojaIdSemanaQueryHandler(IVendaSemanalCacheRepository vendaSemanalCache, IMapper mapper)
        {
            _vendaSemanalCache = vendaSemanalCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<VendaSemanalCachedResponse>> Handle(GetVendaSemanalByLojaIdSemanaQuery query, CancellationToken cancellationToken)
        {
            var vendaSemanal = await _vendaSemanalCache.GetByLojaIdSemanaAsync(query.LojaId, query.Ano, query.NumeroDaSemana);
            var mappedVendaSemanal = _mapper.Map<VendaSemanalCachedResponse>(vendaSemanal);
            return Result<VendaSemanalCachedResponse>.Success(mappedVendaSemanal);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}