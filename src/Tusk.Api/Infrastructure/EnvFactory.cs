using System;
using Tusk.Api.Exceptions;

namespace Tusk.Api.Infrastructure
{
    public static class EnvFactory
    {
        public static string GetConnectionString()
        {
            var envConn = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            if (string.IsNullOrEmpty(envConn))
            {
                throw new MissingEnvException("CONNECTION_STRING");
            }

            return envConn;
        }
    }
}
