using System.Globalization;
using Tusk.Application.Exceptions;

namespace Tusk.Api.Infrastructure;

public static class EnvFactory
{
    public static string GetConnectionString() => TryGetEnv<string>("CONNECTION_STRING");

    private static T TryGetEnv<T>(
        string name)
    {
        var env = Environment.GetEnvironmentVariable(name);
        if (string.IsNullOrEmpty(env))
        {
            throw new MissingEnvException(name);
        }

        return (T)Convert.ChangeType(env, typeof(T), CultureInfo.InvariantCulture);
    }
}

