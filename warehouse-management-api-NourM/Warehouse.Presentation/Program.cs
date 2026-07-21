using System.Globalization;
using Hangfire;
using Hangfire.PostgreSql;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi;
using Serilog;
using StackExchange.Redis;
using Warehouse.Application.Cache.CacheStatistics;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Repositories;
using Warehouse.Infrastructure;
using Warehouse.Infrastructure.BackgroundJobs;
using Warehouse.Infrastructure.HealthChecks;
using Warehouse.Infrastructure.Repositories;
using Warehouse.Presentation.Filters;
using Warehouse.Presentation.Mapping;
using Warehouse.Presentation.Middleware;
using Warehouse.Presentation.Swagger;
using Microsoft.OpenApi;

//serilog creates a new file every day.
Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

//Things that will be checked by the health check:
string databaseConnection =
    builder.Configuration.GetConnectionString("DefaultConnection");

string redisConnection =
    builder.Configuration.GetConnectionString("Redis");

builder.Services.AddHealthChecks()
    .AddNpgSql(
        databaseConnection,
        name: "PostgreSQL")
    .AddCheck<RedisRetryHealthCheck>("redis");

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

// whenever someone uses ILogger, Serilog will handle it.
builder.Host.UseSerilog();
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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter Firebase JWT token"
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
        });
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

//Firebase setup
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MetadataAddress =
            "https://securetoken.google.com/warehouse-api-nourm/.well-known/openid-configuration";

        options.Audience = "warehouse-api-nourm";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer =
                "https://securetoken.google.com/warehouse-api-nourm",

            ValidateAudience = true,
            ValidAudience =
                "warehouse-api-nourm",

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();
//Localization:To enable the localization service in the app
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

//caching using redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = 
        builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "MyApp_";
});

builder.Services.AddHealthChecksUI(options =>
    {
        options.AddHealthCheckEndpoint(
            "Warehouse API",
            "/health");

        options.SetEvaluationTimeInSeconds(10);
    }).AddInMemoryStorage();

//to tell ASP.NET that these are the only languages supported.
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("fr"),
    new CultureInfo("ar")
};

//Configuring Hangfire
//Hangfire will use PostgreSQL to store: job schedules, job status, execution history , failed jobs... (it creates its own tables)

builder.Services.AddHangfire(configuration =>
{
    configuration.UsePostgreSqlStorage(options =>
    {
        options.UseNpgsqlConnection(
            builder.Configuration.GetConnectionString("DefaultConnection"));
    });
});

builder.Services.AddHangfireServer();

builder.Services.AddScoped<ProductExpirationJob>();

//Register the cache service
//Assumption: one shared statistics object for the whole application.
builder.Services.AddSingleton<ICacheStatisticsService, CacheStatisticsService>();
Console.WriteLine(
    builder.Configuration["Firebase:ProjectId"]
);
var app = builder.Build();
//Log to see when the application started running
Log.Information("Warehouse Management API started successfully.");

//localization settings that the app should use.
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseDefaultFiles(); //lets ASP.NET open index.html automatically.
app.UseStaticFiles(); //allows the browser to load HTML, CSS, and JavaScript files.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.InjectJavascript("/Frontend/swaggerAuth.js");
});

app.UseRequestLocalization(localizationOptions);
//Middlewares sequence:
app.UseMiddleware<RequestTimingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

//registering authentication and authorization

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

//Configuring Hangfire's dashboard
app.UseHangfireDashboard("/hangfire");

string productExpirationCron = builder.Configuration["Hangfire:ProductExpirationCron"];

RecurringJob.AddOrUpdate<ProductExpirationJob>("product-expiration-check", job => job.CheckProductsAsync(CancellationToken.None),
    productExpirationCron);

app.Run();