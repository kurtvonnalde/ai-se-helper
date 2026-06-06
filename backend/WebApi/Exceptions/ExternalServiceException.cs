namespace WebApi.Exceptions;

public sealed class ExternalServiceException(string message, string code = "external_service_error")
    : AppException(message, code)
{
}
