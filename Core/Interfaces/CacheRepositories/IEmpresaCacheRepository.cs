using Core.Entities.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IEmpresaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Empresa>> GetCachedListAsync();

        Task<Empresa> GetByIdAsync(int empresaId);

        Task<Empresa> GetByNomeAsync(string nome);


        //---------------------------------------------------------------------------------------------------

    }
}