using LVLgroupApp.Views.Shared.Components.AnswerText.Models;
using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApp.Views.Shared.Components.AnswerText

{
    public class AnswerTextViewComponent : ViewComponent
    {

        //---------------------------------------------------------------------------------------------------


        public IViewComponentResult Invoke(AnswerTextViewModel model)
        {
            return View(model);
        }
    }
}