using Core.Entities.Charts;
using System.Threading.Tasks;

namespace Core.Interfaces.ChartCacheRepositories
{
    public interface ILojaChartCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<ChartPoint> CountAllLojasCachedAsync();


        //---------------------------------------------------------------------------------------------------

    }
}