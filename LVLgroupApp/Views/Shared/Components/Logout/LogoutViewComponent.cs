using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.Logout
{
    public class LogoutViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}