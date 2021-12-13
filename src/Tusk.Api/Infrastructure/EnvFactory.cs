using Tusk.Api.Exceptions;

namespace Tusk.Api.Infrastructure;

public static class EnvFactory
{
    public static string GetConnectionString() => TryGetEnv("CONNECTION_STRING");

    public static string GetJwtIssuer() => TryGetEnv("JWT_ISSUER");

    public static string GetJwtKey() => TryGetEnv("JWT_KEY");

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

