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
    public class GetLojasByEmpresaIdCachedQuery : IRequest<Result<List<LojaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetLojasByEmpresaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetLojasByEmpresaIdCachedQueryHandler : IRequestHandler<GetLojasByEmpresaIdCachedQuery, Result<List<LojaCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILojaCacheRepository _lojaCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetLojasByEmpresaIdCachedQueryHandler(ILojaCacheRepository lojaCache, IMapper mapper)
        {
            _lojaCache = lojaCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<LojaCachedResponse>>> Handle(GetLojasByEmpresaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var lojaList = await _lojaCache.GetByEmpresaIdCachedListAsync(request.empresaId);
            var mappedLojas = _mapper.Map<List<LojaCachedResponse>>(lojaList);
            return Result<List<LojaCachedResponse>>.Success(mappedLojas);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}