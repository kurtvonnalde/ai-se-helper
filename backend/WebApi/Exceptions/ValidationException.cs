namespace WebApi.Exceptions;

public sealed class ValidationException(string message, string code = "validation_error")
    : AppException(message, code)
{
}
