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

namespace Core.Features.Logs.Queries.GetByEmailUserLogs
{

    //---------------------------------------------------------------------------------------------------


    public class GetAuditLogsByEmailQuery : IRequest<Result<List<AuditLogResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        public string email { get; set; }


        //---------------------------------------------------------------------------------------------------


        public GetAuditLogsByEmailQuery()
        {
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class GetAuditLogsByEmailQueryHandler : IRequestHandler<GetAuditLogsByEmailQuery, Result<List<AuditLogResponse>>>
    {

        //---------------------------------------------------------------------------------------------------


        private readonly ILogRepository _repository;

        private readonly IMapper _mapper;


        //---------------------------------------------------------------------------------------------------


        public GetAuditLogsByEmailQueryHandler(ILogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Result<List<AuditLogResponse>>> Handle(GetAuditLogsByEmailQuery request, CancellationToken cancellationToken)
        {
            var auditLogs = await _repository.GeAuditLogsByEmailAsync(request.email);
            var mappedAuditLogs = _mapper.Map<List<AuditLogResponse>>(auditLogs);
            return Result<List<AuditLogResponse>>.Success(mappedAuditLogs);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}