using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.ProductImages;
using Warehouse.Domain.Products;
using Warehouse.Domain.StockMovements;
using Warehouse.Domain.Suppliers;
namespace Warehouse.Infrastructure;
public class WarehouseDbContext : DbContext
{
    // Constructor for Dependency Injection (standard for ASP.NET Core)
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
    {
    }

    // DbSet properties represent your database tables
    public DbSet<Product> Products { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<ProductImage>  ProductImages { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>();

        base.OnModelCreating(modelBuilder);
    }
}