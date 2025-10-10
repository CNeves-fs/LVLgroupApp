using AutoMapper;
using Infrastructure.Data.DbContext;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace LVLgroupApi.Abstractions
{
    public abstract class BaseController<T> : Controller
    {

        //---------------------------------------------------------------------------------------------------


        private IMediator? _mediatorInstance;

        private ILogger<T>? _loggerInstance;

        private IMapper? _mapperInstance;

        private LVLgroupDbContext? _contextInstance;

        private IRequestCultureFeature? _cultureFeature;


        //---------------------------------------------------------------------------------------------------


        protected IMediator? _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();

        protected ILogger<T>? _logger => _loggerInstance ??= HttpContext.RequestServices.GetService<ILogger<T>>();

        protected IMapper? _mapper => _mapperInstance ??= HttpContext.RequestServices.GetService<IMapper>();

        protected LVLgroupDbContext _context => _contextInstance ??= HttpContext.RequestServices.GetRequiredService<LVLgroupDbContext>();

        protected IRequestCultureFeature? _culture => _cultureFeature ??= HttpContext.Features.Get<IRequestCultureFeature>();


        //---------------------------------------------------------------------------------------------------

    }
}