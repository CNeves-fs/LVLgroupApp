using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetByLojaId
{
    public class GetVendaDiariaByLojaIdDataQuery : IRequest<Result<VendaDiariaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int LojaId { get; set; }

        public int Ano { get; set; }

        public int Mês { get; set; }

        public int DiaDoMês { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetVendaDiariaByLojaIdDataQueryHandler : IRequestHandler<GetVendaDiariaByLojaIdDataQuery, Result<VendaDiariaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IVendaDiariaCacheRepository _vendaDiariaCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetVendaDiariaByLojaIdDataQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
            {
                _vendaDiariaCache = vendaDiariaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<VendaDiariaCachedResponse>> Handle(GetVendaDiariaByLojaIdDataQuery query, CancellationToken cancellationToken)
            {
                var vendaDiaria = await _vendaDiariaCache.GetByLojaIdDataAsync(query.LojaId, query.Ano, query.Mês, query.DiaDoMês);
                var mappedVendaDiaria = _mapper.Map<VendaDiariaCachedResponse>(vendaDiaria);
                return Result<VendaDiariaCachedResponse>.Success(mappedVendaDiaria);
            }


            //---------------------------------------------------------------------------------------------------

        }
    }
}