using Core.Entities.Business;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface ILojaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Loja>> GetCachedAllLojasListAsync();

        Task<List<Loja>> GetCachedListAsync();

        Task<List<Loja>> GetByEmpresaIdCachedListAsync(int empresaId);

        Task<List<Loja>> GetByGrupolojaIdCachedListAsync(int grupolojaId);

        Task<List<Loja>> GetByMercadoIdCachedListAsync(int? mercadoId);

        Task<Loja> GetByIdAsync(int lojaId);

        Task<Loja> GetByNomeAsync(string nome);


        //---------------------------------------------------------------------------------------------------

    }
}