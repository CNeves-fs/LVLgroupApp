using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.CacheRepositories
{
    public interface INotificacaoOcorrenciaCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<List<NotificacaoOcorrencia>> GetCachedListAsync();

        Task<List<NotificacaoOcorrencia>> GetByTipoOcorrenciaIdAsync(int tipoocorrenciaId);

        Task<NotificacaoOcorrencia> GetByIdAsync(int notificacaoocorrenciaId);


        //---------------------------------------------------------------------------------------------------

    }
}