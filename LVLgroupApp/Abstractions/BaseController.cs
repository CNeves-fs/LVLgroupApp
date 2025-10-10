using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Infrastructure.Data.DbContext;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LVLgroupApp.Abstractions
{
    public abstract class BaseController<T> : Controller
    {

        //---------------------------------------------------------------------------------------------------


        private IMediator _mediatorInstance;

        private ILogger<T> _loggerInstance;

        private IViewRenderService _viewRenderInstance;

        private IMapper _mapperInstance;

        private INotyfService _notifyInstance;

        private string _sessionNameInstance;

        private string _sessionIdInstance;

        private LVLgroupDbContext _contextInstance;

        private IRequestCultureFeature _cultureFeature;


        //---------------------------------------------------------------------------------------------------


        protected string _sessionId => _sessionIdInstance ??= HttpContext.Session.Id;

        protected string _sessionName => _sessionNameInstance ??= HttpContext.Session.GetString("SessionName");

        protected INotyfService _notify => _notifyInstance ??= HttpContext.RequestServices.GetService<INotyfService>();

        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ILogger<T> _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();

        protected IViewRenderService _viewRenderer => _viewRenderInstance ??= HttpContext.RequestServices.GetService<IViewRenderService>();

        protected IMapper _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();

        protected LVLgroupDbContext _context => _contextInstance ??= HttpContext.RequestServices.GetRequiredService<LVLgroupDbContext>();

        protected IRequestCultureFeature _culture => _cultureFeature ??= HttpContext.Features.Get<IRequestCultureFeature>();


        //---------------------------------------------------------------------------------------------------

    }
}