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

namespace Core.Features.Logs.Queries.GetAllAuditLogs
{

    //---------------------------------------------------------------------------------------------------


    public class GetAllAuditLogsQuery : IRequest<Result<List<AuditLogResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public GetAllAuditLogsQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAllAuditLogsQueryHandler : IRequestHandler<GetAllAuditLogsQuery, Result<List<AuditLogResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILogRepository _repository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAllAuditLogsQueryHandler(ILogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<AuditLogResponse>>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
        {
            var auditLogs = await _repository.GetAllAuditLogsAsync();
            var mappedAuditLogs = _mapper.Map<List<AuditLogResponse>>(auditLogs);
            return Result<List<AuditLogResponse>>.Success(mappedAuditLogs);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}
