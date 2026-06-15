using DispatchR.Abstractions.Send;
using Serilog;

namespace Tusk.Application.Behaviours;

public class EventLoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, ValueTask<TResponse>>
    where TRequest : class, IRequest<TRequest, ValueTask<TResponse>>
{
    public required IRequestHandler<TRequest, ValueTask<TResponse>> NextPipeline { get; set; }

    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        Log.Information("[{Class}] - Before calling next",
            nameof(EventLoggerBehavior<TRequest, TResponse>));

        var response = await NextPipeline.Handle(request, cancellationToken);

        var requestName = request.ToString();
        Log.Information("[{Class}] - RequestName: {Request}",
            nameof(EventLoggerBehavior<TRequest, TResponse>), requestName);

        Log.Information("[{Class}] - After calling next, before return",
            nameof(EventLoggerBehavior<TRequest, TResponse>));
        return response;
    }
}
