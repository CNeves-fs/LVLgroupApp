using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Extensions;
using Core.Features.Gruposlojas.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Gruposlojas.Queries.GetAllPaged
{
    public class GetAllGruposlojasQuery : IRequest<PaginatedResult<GrupolojasCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllGruposlojasQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllGruposlojasQueryHandler : IRequestHandler<GetAllGruposlojasQuery, PaginatedResult<GrupolojasCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IGrupolojaRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllGruposlojasQueryHandler(IGrupolojaRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<GrupolojasCachedResponse>> Handle(GetAllGruposlojasQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Grupoloja, GrupolojasCachedResponse>> expression = l => new GrupolojasCachedResponse
            {
                Id = l.Id,
                Nome = l.Nome,
                EmpresaId = l.EmpresaId
            };
            var paginatedList = await _repository.Gruposlojas
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}