using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IOcorrenciaDocumentRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<OcorrenciaDocument> OcorrenciaDocuments { get; }

        Task<List<OcorrenciaDocument>> GetListAsync();

        Task<List<OcorrenciaDocument>> GetListFromOcorrenciaIdAsync(int? ocorrenciaId);

        Task<List<OcorrenciaDocument>> GetListFromFolderAsync(string folder);

        Task<OcorrenciaDocument> GetByFileNameAsync(string filename);

        Task<OcorrenciaDocument> GetByIdAsync(int ocorrenciaId);

        Task<int> InsertAsync(OcorrenciaDocument ocorrenciadocument);

        Task UpdateAsync(OcorrenciaDocument ocorrenciadocument);

        Task DeleteAsync(OcorrenciaDocument ocorrenciadocument);


        //---------------------------------------------------------------------------------------------------

    }
}