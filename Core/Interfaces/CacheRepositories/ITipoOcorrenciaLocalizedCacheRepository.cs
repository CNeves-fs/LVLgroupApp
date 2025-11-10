using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface ITipoOcorrenciaLocalizedCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<TipoOcorrenciaLocalized>> GetCachedListAsync();

        Task<List<TipoOcorrenciaLocalized>> GetByTipoOcorrenciaIdAsync(int tipoocorrenciaId);

        Task<List<TipoOcorrenciaLocalized>> GetByLanguageAsync(string language);

        Task<TipoOcorrenciaLocalized> GetByIdAsync(int tipoocorrencialocalizedId);

        Task<TipoOcorrenciaLocalized> GetByNameAsync(string name);


        //---------------------------------------------------------------------------------------------------

    }
}