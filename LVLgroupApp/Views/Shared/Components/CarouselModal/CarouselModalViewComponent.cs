using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.ImageModal
{
    public class CarouselModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}