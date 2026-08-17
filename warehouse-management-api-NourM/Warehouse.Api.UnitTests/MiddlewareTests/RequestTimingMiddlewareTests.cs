using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Warehouse.Presentation.Middleware;

namespace Warehouse.Api.UnitTests.MiddlewareTests;

public class RequestTimingMiddlewareTests
{
    private readonly Mock<ILogger<RequestTimingMiddleware>> _loggerMock;

    public RequestTimingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<RequestTimingMiddleware>>();
    }
    
    //fake next middleware that sets the status code to 200
    public Task NextMiddleware(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        return Task.CompletedTask;
    }
    

    [Fact]
    public async Task InvokeAsync_ShouldLogPathStatusCodeAndElapsedMilliseconds()
    {
        var context = new DefaultHttpContext(); //creates a fake HTTP request

        context.Request.Method = "GET";
        context.Request.Path = "/api/products";
        RequestDelegate next = NextMiddleware; //Delegate: variable that stores a method that takes an httpContext and returns a Task

        var middleware = new RequestTimingMiddleware(next, _loggerMock.Object);
        
        await middleware.InvokeAsync(context); //method that runs the middleware
        
        //Verifies that the logger received this exact call
        _loggerMock.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>(
                    (state, _) =>
                        state.ToString()!.Contains("/api/products") &&
                        state.ToString()!.Contains("200") &&
                        state.ToString()!.Contains("ms")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
    }
}