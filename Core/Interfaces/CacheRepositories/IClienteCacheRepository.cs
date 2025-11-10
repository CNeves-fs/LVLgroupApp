using Core.Entities.Clientes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IClienteCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Cliente>> GetCachedListAsync();

        Task<Cliente> GetByIdAsync(int clienteId);

        Task<Cliente> GetByTelefoneAsync(string telefone);

        Task<Cliente> GetByEmailAsync(string email);

        Task<Cliente> GetByNIFAsync(string nif);

        Task<Cliente> GetByNomeAsync(string nome);


        //---------------------------------------------------------------------------------------------------

    }
}