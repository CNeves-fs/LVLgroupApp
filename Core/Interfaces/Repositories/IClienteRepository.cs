using Core.Entities.Clientes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IClienteRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Cliente> Clientes { get; }

        Task<List<Cliente>> GetListAsync();

        Task<Cliente> GetByIdAsync(int clienteId);

        Task<Cliente> GetByTelefoneAsync(string telefone);

        Task<Cliente> GetByEmailAsync(string email);

        Task<Cliente> GetByNIFAsync(string nif);

        Task<Cliente> GetByNomeAsync(string nome);

        Task<bool> ExistByTelefoneAsync(string telefone);

        Task<bool> ExistByEmailAsync(string email);

        Task<bool> ExistByNomeAsync(string nome);

        Task<int> InsertAsync(Cliente cliente);

        Task UpdateAsync(Cliente cliente);

        Task DeleteAsync(Cliente cliente);


        //---------------------------------------------------------------------------------------------------

    }
}