using LVLgroupApp.Areas.Vendas.Models.VendaMensal;
using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.BoxVendaMensal

{
    public class BoxVendaMensalViewComponent : ViewComponent
    {

        //---------------------------------------------------------------------------------------------------


        public IViewComponentResult Invoke(BoxMensalViewModel model)
        {
            return View(model);
        }


        //---------------------------------------------------------------------------------------------------

    }
}