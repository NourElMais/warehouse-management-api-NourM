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
    public async Task CheckProductsAsync(CancellationToken cancellationToken)
    {
        DateTime today = DateTime.UtcNow.Date;

        DateTime thirtyDaysFromToday = today.AddDays(30);

        //We query the db once, then seperate them using c# code
        List<Product> products = await _db.Products.Where(p => p.ExpiryDate <= today.AddDays(7)).ToListAsync();
        List<Product> expiredProducts = products.Where(p => p.ExpiryDate < today).ToList();
        List<Product> soonToExpireProducts = products.Where(p => p.ExpiryDate >= today).ToList();
        
        _logger.LogInformation("Expired products: {ExpiredCount}", expiredProducts.Count);

        _logger.LogInformation("Soon to expire products: {SoonToExpireCount}", soonToExpireProducts.Count);

        _logger.LogInformation("Expired product names: {ExpiredNames}", expiredProducts.Select(product => product.Name).ToList());

        _logger.LogInformation("Soon to expire product names: {SoonToExpireNames}",soonToExpireProducts.Select(product => product.Name).ToList());
        
        //Challenge: Archiving products expired more than 7 days ago
        DateTime archiveLimit = DateTime.UtcNow.AddDays(-7);
        List<Product> productsToArchive = await _db.Products.Where(product => product.ExpiryDate < archiveLimit && !product.IsArchived).ToListAsync(cancellationToken);
        foreach (Product product in productsToArchive)
        {
            product.Archive();
            _logger.LogInformation("Automatically archived product: {ProductName}", product.Name);
        }
        if (productsToArchive.Count > 0)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
       
    }
    
}