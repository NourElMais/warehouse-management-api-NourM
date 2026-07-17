using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Presentation.Filters;
using Warehouse.Presentation.Mapping;
using Warehouse.Presentation.Middleware;

var builder = WebApplication.CreateBuilder(args);
var odataBuilder = new ODataConventionModelBuilder();

builder.Services
    .AddControllers()
    .AddOData(options =>
    {
        options
            .Select()
            .Filter()
            .OrderBy()
            .Expand()
            .Count()
            .SetMaxTop(100)
            .AddRouteComponents("odata", odataBuilder.GetEdmModel());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Register filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelValidationFilter>();
    options.Filters.Add<ActionLoggingFilter>();
});
// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

// Register MediatR handlers from Application project
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ListProductsHandler).Assembly));

builder.Services.AddDbContext<WarehouseDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Register AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ProductProfile>();
    config.AddProfile<SupplierProfile>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//Middlewares sequence:
app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();