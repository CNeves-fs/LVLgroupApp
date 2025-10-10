using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IPrazolimiteRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Prazolimite> Prazoslimite { get; }

        Task<List<Prazolimite>> GetListAsync();

        Task<Prazolimite> GetByIdAsync(int prazolimiteId);

        Task<Prazolimite> GetByLimiteAsync(int prazo);

        Task<int> InsertAsync(Prazolimite prazolimite);

        Task UpdateAsync(Prazolimite prazolimite);

        Task DeleteAsync(Prazolimite prazolimite);


        //---------------------------------------------------------------------------------------------------

    }
}