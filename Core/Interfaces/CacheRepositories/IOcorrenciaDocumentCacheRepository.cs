using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IOcorrenciaDocumentCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<OcorrenciaDocument>> GetCachedListAsync();

        Task<List<OcorrenciaDocument>> GetByOcorrenciaIdAsync(int? ocorrenciaId);

        Task<List<OcorrenciaDocument>> GetByFolderAsync(string folder);

        Task<OcorrenciaDocument> GetByIdAsync(int ocorrenciadocumentId);

        Task<OcorrenciaDocument> GetByFileNameAsync(string filenamename);


        //---------------------------------------------------------------------------------------------------

    }
}