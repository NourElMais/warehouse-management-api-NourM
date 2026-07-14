using Microsoft.AspNetCore.Http;
using Warehouse.Presentation.Middleware;

namespace Warehouse.Tests.Validation;

public class CorrelationIdMiddlewareTests
{
    [Fact]
    public async Task ShouldAddCorrelationIdToResponse()
    {
        Task FakeNext(HttpContext context)
        {
            return Task.CompletedTask;
        }

        RequestDelegate next = FakeNext;

        var middleware = new CorrelationIdMiddleware(next);
        var context = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.True(context.Response.Headers.ContainsKey("X-Correlation-ID"));
    }
}