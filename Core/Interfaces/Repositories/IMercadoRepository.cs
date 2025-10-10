using Core.Entities.Business;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IMercadoRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Mercado> Mercados { get; }

        Task<List<Mercado>> GetListAsync();

        Task<Mercado> GetByIdAsync(int mercadoId);

        Task<Mercado> GetByNomeAsync(string nome);

        Task<int> InsertAsync(Mercado mercado);

        Task UpdateAsync(Mercado mercado);

        Task DeleteAsync(Mercado mercado);


        //---------------------------------------------------------------------------------------------------

    }
}