using Core.Entities.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IGrupolojaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Grupoloja>> GetCachedListAsync();

        Task<List<Grupoloja>> GetByEmpresaIdCachedListAsync(int empresaId);

        Task<Grupoloja> GetByIdAsync(int grouplojaId);


        //---------------------------------------------------------------------------------------------------

    }
}