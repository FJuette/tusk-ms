using MediatR;
using Serilog;

namespace Tusk.Application.Behaviours;

public class EventLoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Log.Information("[{Class}] - Before calling next",
            "EventLoggerBehavior");

        var response = await next();

        var requestName = request.ToString();
        Log.Information("[{Class}] - RequestName: {Request}",
            "EventLoggerBehavior", requestName);

        Log.Information("[{Class}] - After calling next, before return",
            "EventLoggerBehavior");
        return response;
    }
}
