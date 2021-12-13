namespace Tusk.Api.Exceptions;

public class MissingEnvException : Exception
{
    public MissingEnvException()
    {
    }

    public MissingEnvException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }

    public MissingEnvException(
        string env)
        : base($"ENV variable '{env}' missing.")
    {
    }
}
