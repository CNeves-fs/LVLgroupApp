using AspNetCoreHero.Results;
using Core.Entities.Claims;
using Core.Extensions;
using Core.Features.Fotos.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Fotos.Queries.GetAllPaged
{
    public class GetAllFotosQuery : IRequest<PaginatedResult<FotoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllFotosQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllFotosQueryHandler : IRequestHandler<GetAllFotosQuery, PaginatedResult<FotoCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IFotoRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllFotosQueryHandler(IFotoRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<FotoCachedResponse>> Handle(GetAllFotosQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Foto, FotoCachedResponse>> expression = f => new FotoCachedResponse
            {
                Id = f.Id,
                FileName = f.FileName,
                FototagId = f.FototagId,
                Path = f.Path,
                ClaimId = (int) f.ClaimId
            };
            var paginatedList = await _repository.Fotos
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}