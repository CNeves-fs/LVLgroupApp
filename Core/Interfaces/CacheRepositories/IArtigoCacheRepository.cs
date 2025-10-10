using Core.Entities.Artigos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IArtigoCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Artigo>> GetCachedListAsync();

        Task<List<Artigo>> GetByEmpresaCachedListAsync(int empresaId);

        Task<Artigo> GetByIdAsync(int artigoId);

        Task<Artigo> GetByReferenciaAsync(string referencia);


        //---------------------------------------------------------------------------------------------------

    }
}