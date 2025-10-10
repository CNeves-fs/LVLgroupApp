using Core.Entities.Artigos;
using Core.Entities.Business;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IGenderRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Gender> Genders { get; }

        Task<List<Gender>> GetListAsync();

        Task<Gender> GetByIdAsync(int genderId);

        Task<Gender> GetByNomeAsync(string nome);

        Task<int> InsertAsync(Gender gender);

        Task UpdateAsync(Gender gender);

        Task DeleteAsync(Gender gender);


        //---------------------------------------------------------------------------------------------------

    }
}