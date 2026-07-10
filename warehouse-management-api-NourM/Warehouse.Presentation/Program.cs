using Microsoft.EntityFrameworkCore;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Repositories;
using Warehouse.Infrastructure.Models;
using Warehouse.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<ISupplierRepository, SupplierRepository>();

// Register MediatR handlers from Application project
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ListProductsHandler).Assembly));

builder.Services.AddDbContext<WarehouseDbFirstContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();