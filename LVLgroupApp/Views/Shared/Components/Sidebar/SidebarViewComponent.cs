using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.Sidebar
{
    public class SidebarViewComponent : ViewComponent
    {

        //---------------------------------------------------------------------------------------------------


        public IViewComponentResult Invoke()
        {
            return View();
        }


        //---------------------------------------------------------------------------------------------------

    }
}