using FluentValidation;
using Microsoft.AspNetCore.Mvc;

internal sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred: {Message}", ex.Message);
            ExceptionDetails exceptionDetails = GetExceptionDetails(ex);
            var problemDetails = new ProblemDetails
            {
                Status = exceptionDetails.Status,
                Type = exceptionDetails.Type,
                Title = exceptionDetails.Title,
                Detail = exceptionDetails.Detail,
            };
            if (exceptionDetails.Error is not null)
            {
                problemDetails.Extensions["errors"] = exceptionDetails.Error;
            }
            context.Response.StatusCode = exceptionDetails.Status;
        }
    }
    private static ExceptionDetails GetExceptionDetails(Exception exception)
    {
        return exception switch
        {
            _ => new ExceptionDetails(
                StatusCodes.Status500InternalServerError,
                "ServerError",
                "Server error",
                "An unexpected error has occurred",
                null)
        };
    }
    internal sealed record ExceptionDetails(
        int Status,
        string Type,
        string Title,
        string Detail,
        string Error);
}