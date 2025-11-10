using Core.Entities.Charts;
using Core.Entities.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.ChartCacheRepositories
{
    public interface IClaimChartCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<ChartPoint> CountAllClaimsCachedAsync();

        Task<ChartPoint> CountClaimsByLojaIdAsync(int lojaId);

        Task<ChartPoint> CountClaimsByGrupolojaIdAsync(int grupolojaId);

        Task<ChartPoint> CountClaimsByEmpresaIdAsync(int empresaId);

        Task<ChartPoint> CountClaimsPorDecidirCachedAsync();

        Task<ChartPoint> CountClaimsPorFecharCachedAsync();

        Task<ChartPoint> CountClaimsNaoAceiteCachedAsync(int empresaId);

        Task<ChartPoint> CountClaimsAceiteCachedAsync(int empresaId);

        Task<ChartPoint> CountClaimsAguardaDeciCachedAsync(int empresaId);

        Task<ChartPoint> CountClaimsPendentesCachedAsync(int empresaId);

        Task<ChartPoint> CountClaimsFechadasCachedQuery(int empresaId);

        Task<ChartPoint> CountClaimsAguardaOpiniaoCachedAsync(int empresaId);

        Task<ChartPoint> CountClaimsAguardaValidCachedAsync(int empresaId);


        //---------------------------------------------------------------------------------------------------

    }
}