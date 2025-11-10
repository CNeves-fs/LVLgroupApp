using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IOcorrenciaRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<Ocorrencia> Ocorrencias { get; }

        Task<List<Ocorrencia>> GetListAsync();

        Task<List<Ocorrencia>> GetListFromCategoriaAsync(int categoriaId);

        Task<List<Ocorrencia>> GetListFromTipoOcorrenciaAsync(int tipoOcorrenciaId);

        Task<List<Ocorrencia>> GetListFromStatusAsync(int statusId);

        Task<List<Ocorrencia>> GetListFromEmpresaAsync(int empresaId);

        Task<List<Ocorrencia>> GetListFromGrupolojaAsync(int grupolojaId);

        Task<List<Ocorrencia>> GetListFromLojaAsync(int lojaId);

        Task<List<Ocorrencia>> GetListFromMercadoAsync(int? mercadoId);

        Task<Ocorrencia> GetByIdAsync(int ocorrenciaId);

        Task<int> InsertAsync(Ocorrencia ocorrencia);

        Task UpdateAsync(Ocorrencia ocorrencia);

        Task DeleteAsync(Ocorrencia ocorrencia);


        //---------------------------------------------------------------------------------------------------

    }
}