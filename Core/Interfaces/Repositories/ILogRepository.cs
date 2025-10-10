using Core.Entities.Artigos;
using Core.Entities.Claims;
using Core.Entities.Logs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ILogRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Audit> AuditLogs { get; }

        Task<List<Audit>> GetAllAuditLogsAsync();

        Task<List<Audit>> GeAuditLogsByUserIdAsync(string userId);

        Task<List<Audit>> GeAuditLogsByEmailAsync(string email);

        Task<List<Audit>> GetPaginatedAuditLogsAsync(int pageNumber, int pageSize);

        Task AddLogAsync(string action, string userId, string email);

        Task<Audit> GetByIdAsync(int auditId);

        Task DeleteAsync(Audit audit);


        //---------------------------------------------------------------------------------------------------

    }
}