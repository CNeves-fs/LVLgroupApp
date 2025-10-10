using AspNetCoreHero.Results;
using AutoMapper;
using Core.Features.Artigos.Response;
using Core.Interfaces.CacheRepositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Artigos.Queries.GetAllCached
{
    public class GetAllArtigosByEmpresaIdCachedQuery : IRequest<Result<List<ArtigoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------

        public int empresaId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllArtigosByEmpresaIdCachedQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllArtigosByEmpresaIdCachedQueryHandler : IRequestHandler<GetAllArtigosByEmpresaIdCachedQuery, Result<List<ArtigoCachedResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IArtigoCacheRepository _artigoCache;
        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllArtigosByEmpresaIdCachedQueryHandler(IArtigoCacheRepository artigoCache, IMapper mapper)
        {
            _artigoCache = artigoCache;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<ArtigoCachedResponse>>> Handle(GetAllArtigosByEmpresaIdCachedQuery request, CancellationToken cancellationToken)
        {
            var artigoList = await _artigoCache.GetByEmpresaCachedListAsync(request.empresaId);
            var mappedGruposartigos = _mapper.Map<List<ArtigoCachedResponse>>(artigoList);
            return Result<List<ArtigoCachedResponse>>.Success(mappedGruposartigos);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}