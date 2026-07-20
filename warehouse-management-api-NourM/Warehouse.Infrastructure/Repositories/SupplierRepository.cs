using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;
using Warehouse.Infrastructure.Caching;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly WarehouseDbContext _db;
    private readonly IDistributedCache _cache;
    private readonly ICacheStatisticsService _cacheStatistics;

    public SupplierRepository(WarehouseDbContext context, IDistributedCache cache, ICacheStatisticsService cacheStatistics)
    {
        _db = context;
        _cache = cache;
        _cacheStatistics = cacheStatistics;
    }

    public async Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken)
    {
        string cacheKey = CacheKeys.Suppliers;
        string? cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            _cacheStatistics.RecordHit(cacheKey);
            return JsonSerializer.Deserialize<List<Supplier>>(cachedValue);
        }
        _cacheStatistics.RecordMiss();

        List<Supplier> suppliers = await _db.Suppliers.ToListAsync(cancellationToken);
        
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        string cacheValue = JsonSerializer.Serialize(suppliers, options);

        await _cache.SetStringAsync(cacheKey, cacheValue,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            },
            cancellationToken);
        _cacheStatistics.RecordRefresh(cacheKey);
        return suppliers;
    }

    public async Task<Supplier?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        string cacheKey = CacheKeys.Supplier(id);

        string? cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            _cacheStatistics.RecordHit(cacheKey);
            return JsonSerializer.Deserialize<Supplier>(cachedValue);
        }

        _cacheStatistics.RecordMiss();
        Supplier? supplier = await _db.Suppliers.FirstOrDefaultAsync(supplier => supplier.Id == id, cancellationToken);

        if (supplier is not null)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            string cacheValue = JsonSerializer.Serialize(supplier,options);

            await _cache.SetStringAsync(cacheKey, cacheValue,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                },
                cancellationToken);
            _cacheStatistics.RecordRefresh(cacheKey);
        }

        return supplier;
    }

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        await _db.Suppliers.AddAsync(supplier, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(CacheKeys.Suppliers, cancellationToken);
        _cacheStatistics.RemoveKey(CacheKeys.Suppliers);
        
    }

    public async Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        _db.Suppliers.Update(supplier);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(CacheKeys.Suppliers, cancellationToken);
        await _cache.RemoveAsync(CacheKeys.Supplier(supplier.Id), cancellationToken);
        _cacheStatistics.RemoveKey(CacheKeys.Suppliers);
        _cacheStatistics.RemoveKey(CacheKeys.Supplier(supplier.Id));
    }
}