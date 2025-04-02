using System.Globalization;
using Tusk.Application.Exceptions;

namespace Tusk.Api.Infrastructure;

public static class EnvFactory
{
    public static string GetConnectionString() => TryGetEnv<string>("CONNECTION_STRING");
    
    public static string GetSeqUrl() => TryGetEnv<string>("SEQ_URL");

    public static bool UseSeq() => TryGetEnv<bool>("SEQ_URL", false);

    private static T TryGetEnv<T>(
        string name, T? fallback = default)
    {
        var env = Environment.GetEnvironmentVariable(name);
        if (!string.IsNullOrEmpty(env))
        {
            return (T)Convert.ChangeType(env, typeof(T), CultureInfo.InvariantCulture);
        }

        if (fallback != null)
        {
            return (T)Convert.ChangeType(fallback, typeof(T), CultureInfo.InvariantCulture);
        }
        throw new MissingEnvException(name);

    }
}
