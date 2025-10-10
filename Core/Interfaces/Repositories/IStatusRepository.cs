using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IStatusRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Status> Status { get; }

        Task<List<Status>> GetListAsync();

        Task<List<Status>> GetTipoListAsync(int tipo);

        Task<Status> GetByIdAsync(int statusId);

        Task<int> InsertAsync(Status status);

        Task UpdateAsync(Status status);

        Task DeleteAsync(Status status);


        //---------------------------------------------------------------------------------------------------

    }
}