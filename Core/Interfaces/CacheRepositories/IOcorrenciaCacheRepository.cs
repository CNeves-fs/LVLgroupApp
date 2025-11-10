using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface IOcorrenciaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<Ocorrencia>> GetCachedListAsync();

        Task<List<Ocorrencia>> GetByCategoriaIdCachedListAsync(int categoriaId);

        Task<List<Ocorrencia>> GetByTipoOcorrenciaIdCachedListAsync(int tipoOcorrenciaId);

        Task<List<Ocorrencia>> GetByStatusIdCachedListAsync(int statusId);

        Task<List<Ocorrencia>> GetByEmpresaIdCachedListAsync(int empresaId);

        Task<List<Ocorrencia>> GetByGrupolojaIdCachedListAsync(int grupolojaId);

        Task<List<Ocorrencia>> GetByLojaIdCachedListAsync(int lojaId);

        Task<List<Ocorrencia>> GetByMercadoIdCachedListAsync(int? mercadoId);

        Task<Ocorrencia> GetByIdAsync(int ocorrenciaId);


        //---------------------------------------------------------------------------------------------------

    }
}