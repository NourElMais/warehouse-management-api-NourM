using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Warehouse.Infrastructure;
   
public class WarehouseDbContextFactory
        : IDesignTimeDbContextFactory<WarehouseDbContext>
    {
        public WarehouseDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder =
                new DbContextOptionsBuilder<WarehouseDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5434;Database=WarehouseDb;Username=nour;Password=nour123"
            );

            return new WarehouseDbContext(optionsBuilder.Options);
        }
}