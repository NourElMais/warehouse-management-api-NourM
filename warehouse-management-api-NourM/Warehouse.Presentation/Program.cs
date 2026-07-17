using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Repositories;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Presentation.Filters;
using Warehouse.Presentation.Mapping;
using Warehouse.Presentation.Middleware;
using Warehouse.Presentation.Swagger;

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
// builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AcceptLanguageHeaderFilter>();
});

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

//Localization:To enable the localization service in the app
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

//to tell ASP.NET that these are the only languages supported.
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("fr"),
    new CultureInfo("ar")
};

var app = builder.Build();

//localization settings that the app should use.
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseSwagger();
app.UseSwaggerUI();

app.UseRequestLocalization(localizationOptions);
//Middlewares sequence:
app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();


app.Run();