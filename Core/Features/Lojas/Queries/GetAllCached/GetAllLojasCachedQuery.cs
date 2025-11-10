using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Lojas.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Lojas.Queries.GetAllCached
{
    public class GetAllLojasCachedQuery : IRequest<Result<List<LojaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllLojasCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllLojasCachedQueryHandler : IRequestHandler<GetAllLojasCachedQuery, Result<List<LojaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILojaCacheRepository _lojaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllLojasCachedQueryHandler(ILojaCacheRepository lojaCache, IMapper mapper)
        {
            _lojaCache = lojaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<LojaCachedResponse>>> Handle(GetAllLojasCachedQuery request, CancellationToken cancellationToken)
        {
            var lojaList = await _lojaCache.GetCachedAllLojasListAsync();
            var mappedLojas = _mapper.Map<List<LojaCachedResponse>>(lojaList);
            return Result<List<LojaCachedResponse>>.Success(mappedLojas);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}