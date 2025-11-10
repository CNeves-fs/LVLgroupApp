using Core.Entities.Business;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IGrupolojaRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Grupoloja> Gruposlojas { get; }

        Task<List<Grupoloja>> GetListAsync();

        Task<List<Grupoloja>> GetListFromEmpresaAsync(int empresaId);

        Task<Grupoloja> GetByIdAsync(int grupolojaId);

        Task<int> InsertAsync(Grupoloja grupoloja);

        Task UpdateAsync(Grupoloja grupoloja);

        Task DeleteAsync(Grupoloja grupoloja);


        //---------------------------------------------------------------------------------------------------

    }
}