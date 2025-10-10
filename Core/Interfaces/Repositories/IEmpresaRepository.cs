using Core.Entities.Business;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IEmpresaRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Empresa> Empresas { get; }

        Task<List<Empresa>> GetListAsync();

        Task<Empresa> GetByIdAsync(int empresaId);

        Task<Empresa> GetByNomeAsync(string nome);

        Task<int> InsertAsync(Empresa empresa);

        Task UpdateAsync(Empresa empresa);

        Task DeleteAsync(Empresa empresa);


        //---------------------------------------------------------------------------------------------------

    }
}