using MediatR;
using Serilog;

namespace Tusk.Api.Infrastructure.Behaviours;
public class EventLoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Log.Information("[{class}] - Before calling next",
            "EventLoggerBehavior");

        TResponse response = await next();

        var requestName = request.ToString();
        Log.Information("[{class}] - RequestName: {request}",
            "EventLoggerBehavior", requestName);

        Log.Information("[{class}] - After calling next, before return",
            "EventLoggerBehavior");
        return response;
    }
}
