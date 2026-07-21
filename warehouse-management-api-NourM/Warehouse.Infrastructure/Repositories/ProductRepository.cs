using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Infrastructure.Caching;

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
        string cacheKey = CacheKeys.Products;

        string? cachedValue =
            await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            _cacheStatistics.RecordHit(cacheKey);

            return JsonSerializer.Deserialize<List<Product>>(cachedValue) ?? new List<Product>();
        }

        _cacheStatistics.RecordMiss();

        List<Product> products = await _db.Products.Include(product => product.Supplier).ToListAsync(cancellationToken);

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        string cacheValue = JsonSerializer.Serialize(products, options);

        await _cache.SetStringAsync(
            cacheKey,
            cacheValue,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(5)
            },
            cancellationToken);

        _cacheStatistics.RecordRefresh(cacheKey);

        return products;
    }

    public async Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        string cacheKey = CacheKeys.Product(id);

        string? cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            _cacheStatistics.RecordHit(cacheKey);

            return JsonSerializer.Deserialize<Product>(cachedValue);
        }

        _cacheStatistics.RecordMiss();

        Product? product = await _db.Products.Include(product => product.Supplier).FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        string cacheValue = JsonSerializer.Serialize(product, options);

        await _cache.SetStringAsync(
            cacheKey,
            cacheValue,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow =
                    TimeSpan.FromMinutes(5)
            },
            cancellationToken);

        _cacheStatistics.RecordRefresh(cacheKey);

        return product;
    }

    public async Task<List<Product>> SearchAsync(string? name, string? supplier, CancellationToken cancellationToken)
    {
        IQueryable<Product> query = _db.Products.Include(product => product.Supplier);

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

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _db.Products.AddAsync(product, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        await _cache.RemoveAsync(CacheKeys.Products, cancellationToken);

        _cacheStatistics.RemoveKey(CacheKeys.Products);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync(cancellationToken);

        string productCacheKey = CacheKeys.Product(product.Id);

        await _cache.RemoveAsync(CacheKeys.Products, cancellationToken);

        await _cache.RemoveAsync(productCacheKey, cancellationToken);

        _cacheStatistics.RemoveKey(CacheKeys.Products);
        _cacheStatistics.RemoveKey(productCacheKey);
    }
}