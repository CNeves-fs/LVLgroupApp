using Core.Entities.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IParecerRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Parecer> Pareceres { get; }

        Task<List<Parecer>> GetListAsync();

        //Task<List<Parecer>> GetListFromClaimAsync(int claimId);

        Task<Parecer> GetByIdAsync(int parecerId);

        Task<int> InsertAsync(Parecer parecer);

        Task UpdateAsync(Parecer parecer);

        Task DeleteAsync(Parecer parecer);


        //---------------------------------------------------------------------------------------------------

    }
}