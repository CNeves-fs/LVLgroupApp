using AspNetCoreHero.Results;
using Core.Entities.Reports;
using Core.Extensions;
using Core.Features.ReportTypes.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReportTypes.Queries.GetAllPaged
{
    public class GetAllReportTypesQuery : IRequest<PaginatedResult<ReportTypeCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTypesQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllReportTypesQueryHandler : IRequestHandler<GetAllReportTypesQuery, PaginatedResult<ReportTypeCachedResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IReportTypeRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllReportTypesQueryHandler(IReportTypeRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<ReportTypeCachedResponse>> Handle(GetAllReportTypesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ReportType, ReportTypeCachedResponse>> expression = to => new ReportTypeCachedResponse
            {
                Id = to.Id,
                DefaultName = to.DefaultName
            };
            var paginatedList = await _repository.ReportTypes
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }
}