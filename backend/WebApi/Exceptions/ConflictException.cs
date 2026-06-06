namespace WebApi.Exceptions;

public sealed class ConflictException(string message, string code = "conflict")
    : AppException(message, code)
{
}
