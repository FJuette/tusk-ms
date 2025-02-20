using System.Diagnostics;
using MediatR;
using Serilog;

namespace Tusk.Application.Behaviours;

public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var response = await next();

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > TimeSpan.FromSeconds(5).TotalMilliseconds)
        {
            // This method has taken a long time, So we log that to check it later.
            Log.Warning("[{Class}] - {Request} has taken {Time}ms to run completely !",
                "RequestPerformanceBehavior", request, stopwatch.ElapsedMilliseconds);
        }
        else
        {
            Log.Information("[{Class}] - {Request} has taken {Time}ms to run completely !",
                "RequestPerformanceBehavior", request, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
