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
    public class GetVendasSemanaisByEmpresaIdQuery : IRequest<Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int EmpresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByEmpresaIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasSemanaisByEmpresaIdQueryHandler : IRequestHandler<GetVendasSemanaisByEmpresaIdQuery, Result<List<VendaSemanalCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaSemanalCacheRepository _vendaSemanalCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasSemanaisByEmpresaIdQueryHandler(IVendaSemanalCacheRepository vendaSemanalCache, IMapper mapper)
        {
            _vendaSemanalCache = vendaSemanalCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaSemanalCachedResponse>>> Handle(GetVendasSemanaisByEmpresaIdQuery query, CancellationToken cancellationToken)
        {
            var vendaSemanalList = await _vendaSemanalCache.GetByEmpresaIdCachedListAsync(query.EmpresaId);
            var mappedVendasSemanais = _mapper.Map<List<VendaSemanalCachedResponse>>(vendaSemanalList);
            return Result<List<VendaSemanalCachedResponse>>.Success(mappedVendasSemanais);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}