using System;
using Tusk.Api.Exceptions;

namespace Tusk.Api.Infrastructure
{
    public static class EnvFactory
    {
        public static string GetConnectionString()
        {
            return TryGetEnv("CONNECTION_STRING");
        }

        public static string GetJwtIssuer()
        {
            return TryGetEnv("JWT_ISSUER");
        }

        public static string GetJwtKey()
        {
            return TryGetEnv("JWT_KEY");
        }

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
}
