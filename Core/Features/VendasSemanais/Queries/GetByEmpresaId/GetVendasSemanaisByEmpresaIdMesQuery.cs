using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasSemanais.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasSemanais.Queries.GetByEmpresaId
{
    public class GetVendasSemanaisByEmpresaIdMesQuery : IRequest<Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int EmpresaId { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByEmpresaIdMesQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasSemanaisByEmpresaIdMesQueryHandler : IRequestHandler<GetVendasSemanaisByEmpresaIdMesQuery, Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaSemanalCacheRepository _vendaSemanalCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByEmpresaIdMesQueryHandler(IVendaSemanalCacheRepository vendaSemanalCache, IMapper mapper)
        {
            _vendaSemanalCache = vendaSemanalCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaSemanalCachedResponse>>> Handle(GetVendasSemanaisByEmpresaIdMesQuery query, CancellationToken cancellationToken)
        {
            var vendaSemanalList = await _vendaSemanalCache.GetByEmpresaIdMesAsync(query.EmpresaId, query.Ano, query.Mes);
            var mappedVendasSemanais = _mapper.Map<List<VendaSemanalCachedResponse>>(vendaSemanalList);
            return Result<List<VendaSemanalCachedResponse>>.Success(mappedVendasSemanais);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}