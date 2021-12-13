using System.Diagnostics;
using MediatR;
using Serilog;

namespace Tusk.Api.Infrastructure.Behaviours;
public class RequestPerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        TResponse response = await next();

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > TimeSpan.FromSeconds(5).TotalMilliseconds)
        {
            // This method has taken a long time, So we log that to check it later.
            Log.Warning("[{class}] - {request} has taken {time}ms to run completely !",
                "RequestPerformanceBehavior", request, stopwatch.ElapsedMilliseconds);
        }
        else
        {
            Log.Information("[{class}] - {request} has taken {time}ms to run completely !",
                "RequestPerformanceBehavior", request, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
