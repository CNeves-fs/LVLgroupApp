using LVLgroupApp.Views.Shared.Components.FiltroLoja.Models;
using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.FiltroLoja

{
    public class FiltroLojaViewComponent : ViewComponent
    {

        //---------------------------------------------------------------------------------------------------


        public IViewComponentResult Invoke(FiltroLojaViewModel model)
        {
            return View(model);
        }
    }
}