using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LVLgroupApp.Abstractions
{
    public abstract class BasePageModel<T> : PageModel where T : class
    {

        //---------------------------------------------------------------------------------------------------


        private IMediator _mediatorInstance;

        private ILogger<T> _loggerInstance;

        private IMapper _mapperInstance;

        private INotyfService _notifyInstance;

        private string _sessionNameInstance;

        private string _sessionIdInstance;


        //---------------------------------------------------------------------------------------------------


        protected string _sessionId => _sessionIdInstance ??= HttpContext.Session.Id;

        protected string _sessionName => _sessionNameInstance ??= HttpContext.Session.GetString("SessionName");

        protected INotyfService _notify => _notifyInstance ??= HttpContext.RequestServices.GetService<INotyfService>();

        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();

        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();


        //---------------------------------------------------------------------------------------------------

    }
}