using AspNetCoreHero.Results;
using Core.Entities.Business;
using Core.Entities.Claims;
using Core.Extensions;
using Core.Features.Fototags.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fototags.Queries.GetAllPaged
{
    public class GetAllFototagsQuery : IRequest<PaginatedResult<FototagCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllFototagsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllFototagsQueryHandler : IRequestHandler<GetAllFototagsQuery, PaginatedResult<FototagCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IFototagRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllFototagsQueryHandler(IFototagRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<FototagCachedResponse>> Handle(GetAllFototagsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Fototag, FototagCachedResponse>> expression = f => new FototagCachedResponse
            {
                Id = f.Id,
                Tag = f.Tag
            };
            var paginatedList = await _repository.Fototags
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}