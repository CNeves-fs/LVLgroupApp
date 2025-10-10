using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetByVendaSemanalId
{
    public class GetVendaDiariaByVendaSemanalIdDiaQuery : IRequest<Result<VendaDiariaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int VendaSemanalId { get; set; }

        public int DiaDaSemana { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetVendaDiariaByVendaSemanalIdDiaQueryHandler : IRequestHandler<GetVendaDiariaByVendaSemanalIdDiaQuery, Result<VendaDiariaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IVendaDiariaCacheRepository _vendaDiariaCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetVendaDiariaByVendaSemanalIdDiaQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
            {
                _vendaDiariaCache = vendaDiariaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<VendaDiariaCachedResponse>> Handle(GetVendaDiariaByVendaSemanalIdDiaQuery query, CancellationToken cancellationToken)
            {
                var vendaDiaria = await _vendaDiariaCache.GetByVendaSemanalIdDiaAsync(query.VendaSemanalId, query.DiaDaSemana);
                var mappedVendaDiaria = _mapper.Map<VendaDiariaCachedResponse>(vendaDiaria);
                return Result<VendaDiariaCachedResponse>.Success(mappedVendaDiaria);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}