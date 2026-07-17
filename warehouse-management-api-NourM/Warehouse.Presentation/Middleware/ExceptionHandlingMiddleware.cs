using Microsoft.Extensions.Localization;
using Warehouse.Application.Exceptions;
using Warehouse.Domain.Exceptions;
using Warehouse.Presentation.Contracts;

namespace Warehouse.Presentation.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IStringLocalizer<SharedResources> _localizer;
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IStringLocalizer<SharedResources> localizer)
    {
        _next = next; //continue to the next middleware or controller.
        _logger = logger;//to save the real exception details in the backend logs (for the developper to fix the error)
        _localizer = localizer;
    }

    // this method runs for every http request
    //context contains everything about the current request:Request,Response,Headers,User,TraceId...
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException exception)
        {
            _logger.LogError(exception, "Resource not found {CorrelationId}", context.Items["CorrelationId"]);

            context.Response.StatusCode = StatusCodes.Status404NotFound;

            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                ErrorCode = "NOT_FOUND",
                Message = _localizer[exception.Message].Value,
                TraceId = context.TraceIdentifier
            });
        }
        catch (BusinessRuleException exception)
        {
            _logger.LogWarning(exception, "Business rule violation {CorrelationId}", context.Items["CorrelationId"]);

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                ErrorCode = "BUSINESS_RULE_VIOLATION",
                Message = _localizer[exception.Message].Value,
                TraceId = context.TraceIdentifier
            });
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unexpected server error {CorrelationId}", context.Items["CorrelationId"]);

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                ErrorCode = "INTERNAL_SERVER_ERROR",
                Message = _localizer["UnexpectedError"].Value,
                TraceId = context.TraceIdentifier
            });
        }
    }
    
}