using Tusk.Application.Exceptions;

namespace Tusk.Api.Infrastructure;

public static class EnvFactory
{
    public static string GetConnectionString() => TryGetEnv("CONNECTION_STRING");

    private static string TryGetEnv(
        string name)
    {
        var env = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrEmpty(env))
        {
            throw new MissingEnvException(name);
        }

        return env;
    }
}

