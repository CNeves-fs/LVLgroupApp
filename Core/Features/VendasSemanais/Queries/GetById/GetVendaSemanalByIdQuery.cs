using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.VendasSemanais.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.VendasSemanais.Queries.GetById
{
    public class GetVendaSemanalByIdQuery : IRequest<Result<VendaSemanalCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int Id { get; set; }


        //---------------------------------------------------------------------------------------------------


        public class GetVendaSemanalByIdQueryHandler : IRequestHandler<GetVendaSemanalByIdQuery, Result<VendaSemanalCachedResponse>>
        {

            //---------------------------------------------------------------------------------------------------


            private readonly IVendaSemanalCacheRepository _vendaSemanalCache;

            private readonly IMapper _mapper;


            //---------------------------------------------------------------------------------------------------


            public GetVendaSemanalByIdQueryHandler(IVendaSemanalCacheRepository vendaSemanalCache, IMapper mapper)
            {
                _vendaSemanalCache = vendaSemanalCache;
                _mapper = mapper;
            }


            //---------------------------------------------------------------------------------------------------


            public async Task<Result<VendaSemanalCachedResponse>> Handle(GetVendaSemanalByIdQuery query, CancellationToken cancellationToken)
            {
                var vendaSemanal = await _vendaSemanalCache.GetByIdAsync(query.Id);
                var mappedVendaSemanal = _mapper.Map<VendaSemanalCachedResponse>(vendaSemanal);
                return Result<VendaSemanalCachedResponse>.Success(mappedVendaSemanal);
            }


            //---------------------------------------------------------------------------------------------------

        }


        //---------------------------------------------------------------------------------------------------

    }
}