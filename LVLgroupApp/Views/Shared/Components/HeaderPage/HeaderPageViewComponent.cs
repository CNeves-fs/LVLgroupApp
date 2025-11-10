using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace LVLgroupApp.Views.Shared.Components.HeaderPage
{
    public class HeaderPageViewComponent : ViewComponent
    {

        //---------------------------------------------------------------------------------------------------


        //private IMediator _mediatorInstance;
        
        //private IMapper _mapperInstance;

        //protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        //protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();



        //---------------------------------------------------------------------------------------------------


        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}