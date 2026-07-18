using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.VisualBasic;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WarehouseDbContext _db;
    private readonly IDistributedCache _cache;
    private readonly ICacheStatisticsService _cacheStatistics;
    public ProductRepository(WarehouseDbContext context, IDistributedCache cache, ICacheStatisticsService cacheStatistics)
    {
        _db = context;
        _cache = cache;
        _cacheStatistics = cacheStatistics;
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
    {  
        string cacheKey = "Products";

        string? cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            _cacheStatistics.RecordHit(cacheKey);
            return JsonSerializer.Deserialize<List<Product>>(cachedValue);
        }

        _cacheStatistics.RecordMiss(cacheKey);
        List<Product> products =await _db.Products.Include(product => product.Supplier).ToListAsync(cancellationToken);
        var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            string cacheValue = JsonSerializer.Serialize(products, options);

            await _cache.SetStringAsync(cacheKey, cacheValue,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },
                cancellationToken);
            _cacheStatistics.RecordRefresh(cacheKey);
            return products;
        }
    

    public async Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        string cacheKey = $"Product:{id}";

        string? cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            _cacheStatistics.RecordHit(cacheKey);
            return JsonSerializer.Deserialize<Product>(cachedValue);
        }

        _cacheStatistics.RecordMiss(cacheKey);
        Product? product = await _db.Products.Include(product => product.Supplier).FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        if (product is not null)
        {
            // IgnoreCycles prevents serialization errors caused by circular references
            // because in this app, Product points to Supplier and vice versa...
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            string cacheValue = JsonSerializer.Serialize(product,options);

            await _cache.SetStringAsync(cacheKey, cacheValue,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },
                cancellationToken);
            _cacheStatistics.RecordRefresh(cacheKey);
        }
        

        return product;
    }

    public async Task<List<Product>> SearchAsync(string? name, string? supplier, CancellationToken cancellationToken)
    {
        IQueryable<Product> query = _db.Products.Include(product => product.Supplier).AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(product => product.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(supplier))
        {
            query = query.Where(product => product.Supplier != null && product.Supplier.Name.Contains(supplier));
        }

        return await query.ToListAsync(cancellationToken);
    }

    //invalidating the cache whenever related data changes.
    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _db.Products.AddAsync(product, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync("Products",cancellationToken);
        _cacheStatistics.RemoveKey("Products");
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync("Products",cancellationToken);
        await _cache.RemoveAsync($"Product:{product.Id}", cancellationToken);
        _cacheStatistics.RemoveKey("Products");
        _cacheStatistics.RemoveKey($"Product:{product.Id}");
    }
}