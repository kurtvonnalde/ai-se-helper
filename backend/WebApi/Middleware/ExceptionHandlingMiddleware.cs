using Microsoft.AspNetCore.Mvc;
using WebApi.Exceptions;

namespace WebApi.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, logger);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception,
        ILogger logger)
    {
        var (status, title, code) = exception switch
        {
            ValidationException validation => (StatusCodes.Status400BadRequest, "Validation Error", validation.Code),
            NotFoundException notFound => (StatusCodes.Status404NotFound, "Not Found", notFound.Code),
            ConflictException conflict => (StatusCodes.Status409Conflict, "Conflict", conflict.Code),
            ExternalServiceException external => (StatusCodes.Status503ServiceUnavailable, "External Service Error", external.Code),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "internal_server_error")
        };

        if (status >= 500)
        {
            logger.LogError(exception,
                "Unhandled exception at {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);
        }
        else
        {
            logger.LogWarning(exception,
                "Request failed at {Method} {Path}. TraceId: {TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);
        }

        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.Clear();
        context.Response.StatusCode = status;

        var problem = new ProblemDetails
        {
            Title = title,
            Detail = exception.Message,
            Status = status,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;
        problem.Extensions["code"] = code;

        await context.Response.WriteAsJsonAsync(problem, cancellationToken: context.RequestAborted);
    }
}
