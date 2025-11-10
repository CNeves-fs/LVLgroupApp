using Core.Entities.Artigos;
using Core.Entities.Business;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IArtigoRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Artigo> Artigos { get; }

        Task<List<Artigo>> GetListAsync();

        Task<List<Artigo>> GetListFromEmpresaAsync(int empresaId);

        Task<Artigo> GetByIdAsync(int artigoId);

        Task<Artigo> GetByReferenciaAsync(string referencia);

        Task<int> InsertAsync(Artigo artigo);

        Task UpdateAsync(Artigo artigo);

        Task DeleteAsync(Artigo artigo);


        //---------------------------------------------------------------------------------------------------

    }
}