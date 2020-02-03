using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Tusk.Story.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly Lazy<IMediator> _mediator;

        protected BaseController()
        {
            _mediator = new Lazy<IMediator>(() => HttpContext.RequestServices.GetService<IMediator>(), true);
        }

        protected IMediator Mediator => _mediator.Value;
    }
}
