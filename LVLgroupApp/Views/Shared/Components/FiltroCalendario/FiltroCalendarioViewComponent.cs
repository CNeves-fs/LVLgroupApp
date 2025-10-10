using LVLgroupApp.Views.Shared.Components.FiltroCalendario.Models;
using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.FiltroCalendario

{
    public class FiltroCalendarioViewComponent : ViewComponent
    {

        //---------------------------------------------------------------------------------------------------


        public IViewComponentResult Invoke(FiltroCalendarioViewModel model)
        {
            return View(model);
        }
    }
}