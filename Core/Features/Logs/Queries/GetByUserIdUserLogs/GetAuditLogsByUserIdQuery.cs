using AspNetCoreHero.Results;
using AutoMapper;
using Core.Entities.Artigos;
using Core.Features.Artigos.Response;
using Core.Features.Logs.Response;
using Core.Interfaces.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Core.Features.Logs.Queries.GetByUserIdUserLogs
{

    //---------------------------------------------------------------------------------------------------


    public class GetAuditLogsByUserIdQuery : IRequest<Result<List<AuditLogResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public string userId { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAuditLogsByUserIdQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAuditLogsByUserIdQueryHandler : IRequestHandler<GetAuditLogsByUserIdQuery, Result<List<AuditLogResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILogRepository _repository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAuditLogsByUserIdQueryHandler(ILogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<AuditLogResponse>>> Handle(GetAuditLogsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var auditLogs = await _repository.GeAuditLogsByUserIdAsync(request.userId);
            var mappedAuditLogs = _mapper.Map<List<AuditLogResponse>>(auditLogs);
            return Result<List<AuditLogResponse>>.Success(mappedAuditLogs);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}