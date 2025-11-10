using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasDiarias.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasDiarias.Queries.GetById
{
    public class GetVendaDiariaByIdQuery : IRequest<Result<VendaDiariaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetVendaDiariaByIdQueryHandler : IRequestHandler<GetVendaDiariaByIdQuery, Result<VendaDiariaCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IVendaDiariaCacheRepository _vendaDiariaCache;
            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetVendaDiariaByIdQueryHandler(IVendaDiariaCacheRepository vendaDiariaCache, IMapper mapper)
            {
                _vendaDiariaCache = vendaDiariaCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<VendaDiariaCachedResponse>> Handle(GetVendaDiariaByIdQuery query, CancellationToken cancellationToken)
            {
                var vendaDiaria = await _vendaDiariaCache.GetByIdAsync(query.Id);
                var mappedVendaDiaria = _mapper.Map<VendaDiariaCachedResponse>(vendaDiaria);
                return Result<VendaDiariaCachedResponse>.Success(mappedVendaDiaria);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}