using System.Threading.Tasks;

namespace LVLgroupApp.Abstractions
{
    public interface IViewRenderService
    {

        //---------------------------------------------------------------------------------------------------


        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);


        //---------------------------------------------------------------------------------------------------

    }
}