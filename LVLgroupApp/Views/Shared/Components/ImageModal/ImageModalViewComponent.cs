using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.ImageModal
{
    public class ImageModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}