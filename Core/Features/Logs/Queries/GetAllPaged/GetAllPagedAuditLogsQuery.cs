using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Artigos;
using Core.Entities.Logs;
using Core.Extensions;
using Core.Features.Artigos.Response;
using Core.Features.Logs.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.Logs.Queries.GetAllPaged
{

    //---------------------------------------------------------------------------------------------------


    public class GetAllPagedAuditLogsQuery : IRequest<PaginatedResult<AuditLogResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAllPagedAuditLogsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllAuditLogsQueryHandler : IRequestHandler<GetAllPagedAuditLogsQuery, PaginatedResult<AuditLogResponse>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILogRepository _repository;


        //---------------------------------------------------------------------------------------------------


        public GetAllAuditLogsQueryHandler(ILogRepository repository)
        {
            _repository = repository;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<PaginatedResult<AuditLogResponse>> Handle(GetAllPagedAuditLogsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Audit, AuditLogResponse>> expression = l => new AuditLogResponse
            {
                Id = l.Id,
                UserId = l.UserId,
                Email = l.Email,
                Type = l.Type,
                TableName = l.TableName,
                DateTime = l.DateTime,
                OldValues = l.OldValues,
                NewValues = l.NewValues,
                AffectedColumns = l.AffectedColumns,
                PrimaryKey = l.PrimaryKey
            };
            var paginatedList = await _repository.AuditLogs
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginatedList;
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}
