using Core.Entities.Artigos;
using Core.Entities.Charts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.ChartCacheRepositories
{
    public interface IArtigoChartCacheRepository
    {

        //---------------------------------------------------------------------------------------------------


        Task<ChartPoint> CountAllArtigosCachedAsync();


        //---------------------------------------------------------------------------------------------------

    }
}