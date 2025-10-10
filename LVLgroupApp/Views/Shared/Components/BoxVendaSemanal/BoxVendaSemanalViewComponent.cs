using LVLgroupApp.Areas.Vendas.Models.VendaSemanal;
using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.BoxVendaSemanal

{
    public class BoxVendaSemanalViewComponent : ViewComponent
    {

        //---------------------------------------------------------------------------------------------------


        public IViewComponentResult Invoke(BoxSemanalViewModel model)
        {
            return View(model);
        }
    }
}