using AutoMapper;
using Core.Entities.Artigos;
using Core.Entities.Logs;
using Core.Extensions;
using Core.Features.Logs.Response;
using Core.Interfaces.Repositories;
using Core.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    //---------------------------------------------------------------------------------------------------


    public class LogRepository : ILogRepository
    {

        //---------------------------------------------------------------------------------------------------


        private readonly IMapper _mapper;

        private readonly IRepositoryAsync<Audit> _repository;

        private readonly IDateTimeService _dateTimeService;


        //---------------------------------------------------------------------------------------------------


        public LogRepository(IRepositoryAsync<Audit> repository, IMapper mapper, IDateTimeService dateTimeService)
        {
            _repository = repository;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
        }


        //---------------------------------------------------------------------------------------------------


        public IQueryable<Audit> AuditLogs => _repository.Entities;


        //---------------------------------------------------------------------------------------------------


        public async Task AddLogAsync(string action, string userId, string email)
        {
            var audit = new Audit()
            {
                Type = action,
                UserId = userId,
                Email = email,
                DateTime = _dateTimeService.NowUtc
            };
            await _repository.AddAsync(audit);
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Audit>> GetAllAuditLogsAsync()
        {
            return await _repository.Entities.ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Audit>> GeAuditLogsByUserIdAsync(string userId)
        {
            //return await _repository.Entities.Where(a => a.UserId == userId).OrderByDescending(a => a.DateTime).Take(250).ToListAsync();
            return await _repository.Entities.Where(a => a.UserId == userId).OrderByDescending(a => a.DateTime).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Audit>> GeAuditLogsByEmailAsync(string email)
        {
            return await _repository.Entities.Where(a => a.Email == email).OrderByDescending(a => a.DateTime).Take(250).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<List<Audit>> GetPaginatedAuditLogsAsync(int pageNumber, int pageSize)
        {
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize <= 0 ? 10 : pageSize;
            long count = await _repository.Entities.LongCountAsync();

            return await _repository.Entities.OrderByDescending(a => a.DateTime).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task<Audit> GetByIdAsync(int auditId)
        {
            return await _repository.Entities.Where(a => a.Id == auditId).FirstOrDefaultAsync();
        }


        //---------------------------------------------------------------------------------------------------


        public async Task DeleteAsync(Audit audit)
        {
            await _repository.DeleteAsync(audit);
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------


    public class LogProfile : Profile
    {

        //---------------------------------------------------------------------------------------------------


        public LogProfile()
        {
            CreateMap<AuditLogResponse, Audit>().ReverseMap();
        }


        //---------------------------------------------------------------------------------------------------

    }


    //---------------------------------------------------------------------------------------------------

}