namespace WebApi.Exceptions;

public sealed class NotFoundException(string message, string code = "not_found")
    : AppException(message, code)
{
}
