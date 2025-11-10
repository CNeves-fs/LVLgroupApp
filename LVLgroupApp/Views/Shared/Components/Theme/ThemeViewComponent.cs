using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.Theme
{
    public class ThemeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}