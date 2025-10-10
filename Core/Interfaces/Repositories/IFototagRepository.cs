using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IFototagRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Fototag> Fototags { get; }

        Task<List<Fototag>> GetListAsync();

        Task<Fototag> GetByIdAsync(int fototagId);

        Task<int> InsertAsync(Fototag fototag);

        Task UpdateAsync(Fototag fototag);

        Task DeleteAsync(Fototag fototag);


        //---------------------------------------------------------------------------------------------------

    }
}