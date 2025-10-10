using AspNetCoreHero.Results;
using Core.Entities.Ocorrencias;
using Core.Extensions;
using Core.Features.TiposOcorrencias.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TiposOcorrencias.Queries.GetAllPaged
{
    public class GetAllTiposOcorrenciasQuery : IRequest<PaginatedResult<TipoOcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllTiposOcorrenciasQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllTiposOcorrenciasQueryHandler : IRequestHandler<GetAllTiposOcorrenciasQuery, PaginatedResult<TipoOcorrenciaCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ITipoOcorrenciaRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllTiposOcorrenciasQueryHandler(ITipoOcorrenciaRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<TipoOcorrenciaCachedResponse>> Handle(GetAllTiposOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<TipoOcorrencia, TipoOcorrenciaCachedResponse>> expression = to => new TipoOcorrenciaCachedResponse
            {
                Id = to.Id,
                DefaultName = to.DefaultName
            };
            var paginatedList = await _repository.TiposOcorrencias
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}