using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ITipoOcorrenciaRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<TipoOcorrencia> TiposOcorrencias { get; }

        Task<List<TipoOcorrencia>> GetListAsync();

        Task<List<TipoOcorrencia>> GetByCategoriaIdListAsync(int categoriaId);

        Task<TipoOcorrencia> GetByIdAsync(int tipoocorrenciaId);

        Task<TipoOcorrencia> GetByDefaultNameAsync(string defaultName);

        Task<int> InsertAsync(TipoOcorrencia tipoocorrencia);

        Task UpdateAsync(TipoOcorrencia tipoocorrencia);

        Task DeleteAsync(TipoOcorrencia tipoocorrencia);


        //---------------------------------------------------------------------------------------------------

    }
}