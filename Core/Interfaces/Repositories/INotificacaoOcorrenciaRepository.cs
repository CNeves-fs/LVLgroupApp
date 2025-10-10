using Core.Entities.Ocorrencias;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface INotificacaoOcorrenciaRepository
    {

        //---------------------------------------------------------------------------------------------------


        IQueryable<NotificacaoOcorrencia> NotificacoesOcorrencia { get; }

        Task<List<NotificacaoOcorrencia>> GetListAsync();

        Task<List<NotificacaoOcorrencia>> GetListFromTipoOcorrenciaIdAsync(int tipoocorrenciaId);

        Task<NotificacaoOcorrencia> GetByIdAsync(int notificacaoocorrenciaId);

        Task<int> InsertAsync(NotificacaoOcorrencia notificacaoocorrencia);

        Task UpdateAsync(NotificacaoOcorrencia notificacaoocorrencia);

        Task DeleteAsync(NotificacaoOcorrencia notificacaoocorrencia);


        //---------------------------------------------------------------------------------------------------

    }
}