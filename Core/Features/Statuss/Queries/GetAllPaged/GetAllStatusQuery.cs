using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Extensions;
using Core.Features.Statuss.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Statuss.Queries.GetAllPaged
{
    public class GetAllStatusQuery : IRequest<PaginatedResult<StatusCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllStatusQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllStatusQueryHandler : IRequestHandler<GetAllStatusQuery, PaginatedResult<StatusCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IStatusRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllStatusQueryHandler(IStatusRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<StatusCachedResponse>> Handle(GetAllStatusQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Core.Entities.Claims.Status, StatusCachedResponse>> expression = s => new StatusCachedResponse
            {
                Id = s.Id,
                Tipo = s.Tipo,
                Texto = s.Texto,
                Cortexto = s.Cortexto,
                Corfundo = s.Corfundo
            };
            var paginatedList = await _repository.Status
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}