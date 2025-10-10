using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface ITipoOcorrenciaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<TipoOcorrencia>> GetCachedListAsync();

        Task<List<TipoOcorrencia>> GetByCategoriaIdCachedListAsync(int CategoriaId);

        Task<TipoOcorrencia> GetByIdAsync(int tipoocorrenciaId);

        Task<TipoOcorrencia> GetByNameAsync(string defaultname);


        //---------------------------------------------------------------------------------------------------

    }
}