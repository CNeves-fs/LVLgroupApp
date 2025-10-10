using Core.Entities.Charts;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IClaimRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Claim> Claims { get; }

        Task<List<Claim>> GetListAsync();

        Task<List<Claim>> GetListFromEmpresaAsync(int empresaId);

        Task<List<Claim>> GetListFromGrupolojaAsync(int grupolojaId);

        Task<List<Claim>> GetListFromLojaAsync(int lojaId);

        Task<Claim> GetByIdAsync(int claimId);

        Task<int> InsertAsync(Claim claim);

        Task UpdateAsync(Claim claim);

        Task DeleteAsync(Claim claim);


        //---------------------------------------------------------------------------------------------------

    }
}