using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Warehouse.Application.Exceptions;
using Warehouse.Domain.Exceptions;
using Warehouse.Presentation.Middleware;

namespace Warehouse.Tests.MiddleWare;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task NotFoundException_ShouldReturn404()
    {
        //this is a controller to simulate the fact that after this middleware, we have the controller that will throw an exception
        Task FakeController(HttpContext context)
        {
            throw new NotFoundException("Product not found.");
        }
        RequestDelegate _next = FakeController;
        //Note: NullLogger is a logger that does nothing
        var logger = NullLogger<ExceptionHandlingMiddleware>.Instance;
        var middleware = new ExceptionHandlingMiddleware(_next, logger);
        var httpContext = new DefaultHttpContext();
       
        await middleware.InvokeAsync(httpContext);
        Assert.Equal(404, httpContext.Response.StatusCode);
    }
    
    [Fact]
    public async Task BusinessRuleException_ShouldReturn400()
    {
        //this is a controller to simulate the fact that after this middleware, we have the controller that will throw an exception
        Task FakeController(HttpContext context)
        {
            throw new BusinessRuleException("Business Rule Violation.");
        }
        RequestDelegate _next = FakeController;
        //Note: NullLogger is a logger that does nothing
        var logger = NullLogger<ExceptionHandlingMiddleware>.Instance;
        var middleware = new ExceptionHandlingMiddleware(_next, logger);
        var httpContext = new DefaultHttpContext();
       
        await middleware.InvokeAsync(httpContext);
        Assert.Equal(400, httpContext.Response.StatusCode);
    }
    
    [Fact]
    public async Task Exception_ShouldReturn500()
    {
        //this is a controller to simulate the fact that after this middleware, we have the controller that will throw an exception
        Task FakeController(HttpContext context)
        {
            throw new Exception("Internal Server Error.");
        }
        RequestDelegate _next = FakeController;
        //Note: NullLogger is a logger that does nothing
        var logger = NullLogger<ExceptionHandlingMiddleware>.Instance;
        var middleware = new ExceptionHandlingMiddleware(_next, logger);
        var httpContext = new DefaultHttpContext();
       
        await middleware.InvokeAsync(httpContext);
        Assert.Equal(500, httpContext.Response.StatusCode);
    }
}
