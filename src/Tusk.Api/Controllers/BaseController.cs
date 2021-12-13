using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Tusk.Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    private readonly Lazy<IMediator> _mediator;

    #nullable disable
    protected BaseController() =>
        _mediator = new Lazy<IMediator>(
            () => HttpContext.RequestServices.GetService<IMediator>(),
            true);

    protected IMediator Mediator => _mediator.Value;
}
