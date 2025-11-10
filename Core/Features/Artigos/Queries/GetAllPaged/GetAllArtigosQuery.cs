using AspNetCoreHero.Results;
using Core.Entities.Artigos;
using Core.Extensions;
using Core.Features.Artigos.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Artigos.Queries.GetAllPaged
{
    public class GetAllArtigosQuery : IRequest<PaginatedResult<ArtigoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllArtigosQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllArtigosQueryHandler : IRequestHandler<GetAllArtigosQuery, PaginatedResult<ArtigoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IArtigoRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllArtigosQueryHandler(IArtigoRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<ArtigoCachedResponse>> Handle(GetAllArtigosQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Artigo, ArtigoCachedResponse>> expression = l => new ArtigoCachedResponse
            {
                Id = l.Id,
                Referencia = l.Referencia,
                EmpresaId = l.EmpresaId,
                //GenderId = l.GenderId,
                Pele = l.Pele,
                Cor = l.Cor
            };
            var paginatedList = await _repository.Artigos
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}