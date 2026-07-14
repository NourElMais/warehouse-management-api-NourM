namespace Warehouse.Presentation.Middleware;

//purpose of this middleware: Give every request an ID.
public class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-ID";
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault() ?? Guid.NewGuid().ToString();

        context.Response.Headers[HeaderName] = correlationId;

        await _next(context);
    }
}