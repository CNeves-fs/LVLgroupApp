using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Core.Interfaces.Shared
{
    public interface IDocumentService
    {

        //---------------------------------------------------------------------------------------------------


        public Task UploadDocumentAsync(int ocorrenciaId, IFormFile file);


        //---------------------------------------------------------------------------------------------------

    }
}