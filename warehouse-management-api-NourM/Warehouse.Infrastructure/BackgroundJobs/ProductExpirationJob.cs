using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Warehouse.Domain.Products;

namespace Warehouse.Infrastructure.BackgroundJobs;

public class ProductExpirationJob
{
    //every time Hangfire runs the job, it will have access to the database and the logger.
    private readonly WarehouseDbContext _db;// for the job to access the database
    private readonly ILogger<ProductExpirationJob> _logger;

    public ProductExpirationJob(WarehouseDbContext db, ILogger<ProductExpirationJob> logger)
    {
        _db = db;
        _logger = logger;
    }
    //method that checks already expired products, and products expiring within 30 days
    public async Task CheckProductsAsync(
        CancellationToken cancellationToken)
    {
        DateTime today = DateTime.UtcNow.Date;

        DateTime thirtyDaysFromToday = today.AddDays(30);

        List<Product> expiredProducts = await _db.Products.Where(product => product.ExpiryDate < today).ToListAsync(cancellationToken);

        List<Product> soonToExpireProducts = await _db.Products.Where(product => product.ExpiryDate >= today && product.ExpiryDate <= thirtyDaysFromToday).ToListAsync(cancellationToken);
        _logger.LogInformation("Expired products: {ExpiredCount}", expiredProducts.Count);

        _logger.LogInformation("Soon to expire products: {SoonToExpireCount}", soonToExpireProducts.Count);

        _logger.LogInformation("Expired product names: {ExpiredNames}", expiredProducts.Select(product => product.Name).ToList());

        _logger.LogInformation("Soon to expire product names: {SoonToExpireNames}",soonToExpireProducts.Select(product => product.Name).ToList());
    }
    
}