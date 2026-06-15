using System.Diagnostics;
using DispatchR.Abstractions.Send;
using Serilog;

namespace Tusk.Application.Behaviours;

public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, ValueTask<TResponse>>
    where TRequest : class, IRequest<TRequest, ValueTask<TResponse>>
{
    public required IRequestHandler<TRequest, ValueTask<TResponse>> NextPipeline { get; set; }

    public async ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await NextPipeline.Handle(request, cancellationToken);

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > TimeSpan.FromSeconds(5).TotalMilliseconds)
        {
            Log.Warning("[{Class}] - {Request} has taken {Time}ms to run completely !",
                nameof(RequestPerformanceBehavior<TRequest, TResponse>), request, stopwatch.ElapsedMilliseconds);
        }
        else
        {
            Log.Information("[{Class}] - {Request} has taken {Time}ms to run completely !",
                nameof(RequestPerformanceBehavior<TRequest, TResponse>), request, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
