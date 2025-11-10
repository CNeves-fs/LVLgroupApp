using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ITipoOcorrenciaLocalizedRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<TipoOcorrenciaLocalized> TiposOcorrenciasLocalized { get; }

        Task<List<TipoOcorrenciaLocalized>> GetListAsync();

        Task<List<TipoOcorrenciaLocalized>> GetListFromTipoOcorrenciaIdAsync(int tipoocorrenciaId);

        Task<List<TipoOcorrenciaLocalized>> GetByLanguageAsync(string language);

        Task<TipoOcorrenciaLocalized> GetByIdAsync(int tipoocorrencialocalizedId);

        Task<TipoOcorrenciaLocalized> GetByNameAsync(string name);

        Task<int> InsertAsync(TipoOcorrenciaLocalized tipoocorrencialocalized);

        Task UpdateAsync(TipoOcorrenciaLocalized tipoocorrencialocalized);

        Task DeleteAsync(TipoOcorrenciaLocalized tipoocorrencialocalized);


        //---------------------------------------------------------------------------------------------------

    }
}