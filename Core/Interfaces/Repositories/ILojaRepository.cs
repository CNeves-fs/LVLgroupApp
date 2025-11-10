using Core.Entities.Business;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ILojaRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Loja> Lojas { get; }

        Task<List<Loja>> GetAllLojasListAsync();

        Task<List<Loja>> GetListAsync();

        Task<List<Loja>> GetListFromEmpresaAsync(int empresaId);

        Task<List<Loja>> GetListFromGrupolojaAsync(int grupolojaId);

        Task<List<Loja>> GetListFromMercadoAsync(int? mercadoId);

        Task<Loja> GetByIdAsync(int lojaId);

        Task<Loja> GetByNomeAsync(string nome);

        Task<int> InsertAsync(Loja loja);

        Task UpdateAsync(Loja loja);

        Task DeleteAsync(Loja loja);


        //---------------------------------------------------------------------------------------------------

    }
}