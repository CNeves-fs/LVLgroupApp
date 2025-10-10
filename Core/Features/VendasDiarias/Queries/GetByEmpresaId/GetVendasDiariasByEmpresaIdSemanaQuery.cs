using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetByEmpresaId
{
    public class GetVendasDiariasByEmpresaIdSemanaQuery : IRequest<Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public int EmpresaId { get; set; }

        public int Ano { get; set; }

        public int NumeroDaSemana { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasByEmpresaIdSemanaQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetVendasDiariasByEmpresaIdSemanaQueryHandler : IRequestHandler<GetVendasDiariasByEmpresaIdSemanaQuery, Result<List<VendaDiariaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IVendaDiariaCacheRepository _vendaDiariaCache;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetVendasDiariasByEmpresaIdSemanaQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
        {
            _vendaDiariaCache = vendaDiariaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<VendaDiariaCachedResponse>>> Handle(GetVendasDiariasByEmpresaIdSemanaQuery query, CancellationToken cancellationToken)
        {
            var vendaDiariaList = await _vendaDiariaCache.GetByEmpresaIdSemanaAsync(query.EmpresaId, query.Ano, query.NumeroDaSemana);
            var mappedVendasDiarias = _mapper.Map<List<VendaDiariaCachedResponse>>(vendaDiariaList);
            return Result<List<VendaDiariaCachedResponse>>.Success(mappedVendasDiarias);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}