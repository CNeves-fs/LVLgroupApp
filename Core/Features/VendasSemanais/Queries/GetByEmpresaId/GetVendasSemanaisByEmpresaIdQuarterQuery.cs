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
    public class GetVendasSemanaisByEmpresaIdQuarterQuery : IRequest<Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int EmpresaId { get; set; }

        public int Ano { get; set; }

        public int Quarter { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByEmpresaIdQuarterQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasSemanaisByEmpresaIdQuarterQueryHandler : IRequestHandler<GetVendasSemanaisByEmpresaIdQuarterQuery, Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaSemanalCacheRepository _vendaSemanalCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByEmpresaIdQuarterQueryHandler(IVendaSemanalCacheRepository vendaSemanalCache, IMapper mapper)
        {
            _vendaSemanalCache = vendaSemanalCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaSemanalCachedResponse>>> Handle(GetVendasSemanaisByEmpresaIdQuarterQuery query, CancellationToken cancellationToken)
        {
            var vendaSemanalList = await _vendaSemanalCache.GetByEmpresaIdQuarterAsync(query.EmpresaId, query.Ano, query.Quarter);
            var mappedVendasSemanais = _mapper.Map<List<VendaSemanalCachedResponse>>(vendaSemanalList);
            return Result<List<VendaSemanalCachedResponse>>.Success(mappedVendasSemanais);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}