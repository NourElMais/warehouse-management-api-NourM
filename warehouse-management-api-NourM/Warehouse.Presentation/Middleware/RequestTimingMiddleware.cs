namespace Warehouse.Presentation.Middleware;

public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        var startTime = DateTime.Now;
        await _next(context);
        var endTime = DateTime.Now;
        var duration = endTime - startTime;

        _logger.LogInformation("Request {Method} {Path} returned {StatusCode} in {Duration} ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            duration.TotalMilliseconds);
    }
}