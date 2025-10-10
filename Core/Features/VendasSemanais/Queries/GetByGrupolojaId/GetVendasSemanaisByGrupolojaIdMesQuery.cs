using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasSemanais.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasSemanais.Queries.GetByGrupolojaId
{
    public class GetVendasSemanaisByGrupolojaIdMesQuery : IRequest<Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int GrupolojaId { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByGrupolojaIdMesQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasSemanaisByGrupolojaIdMesQueryHandler : IRequestHandler<GetVendasSemanaisByGrupolojaIdMesQuery, Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaSemanalCacheRepository _vendaSemanalCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByGrupolojaIdMesQueryHandler(IVendaSemanalCacheRepository vendaSemanalCache, IMapper mapper)
        {
            _vendaSemanalCache = vendaSemanalCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaSemanalCachedResponse>>> Handle(GetVendasSemanaisByGrupolojaIdMesQuery query, CancellationToken cancellationToken)
        {
            var vendaSemanalList = await _vendaSemanalCache.GetByGrupolojaIdMesAsync(query.GrupolojaId, query.Ano, query.Mes);
            var mappedVendasSemanais = _mapper.Map<List<VendaSemanalCachedResponse>>(vendaSemanalList);
            return Result<List<VendaSemanalCachedResponse>>.Success(mappedVendasSemanais);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}