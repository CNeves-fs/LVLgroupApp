using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IFotoRepository
    {

        //---------------------------------------------------------------------------------------------------


        // All Fotos
        IQueryable<Foto> Fotos { get; }


        // All Fotos
        Task<List<Foto>> GetListAsync();


        // All Fotos by ClaimId
        Task<List<Foto>> GetListFromClaimAsync(int claimId);


        // All Fotos in all TempFolder
        Task<List<Foto>> GetListFromAllTempFolderAsync();


        // All Fotos by TempFolder
        Task<List<Foto>> GetListFromTempFolderAsync(string tempFolder);


        // Foto by Id
        Task<Foto> GetByIdAsync(int fotoId);


        Task<int> InsertAsync(Foto foto);


        Task UpdateAsync(Foto foto);


        Task DeleteAsync(Foto foto);


        //---------------------------------------------------------------------------------------------------

    }
}