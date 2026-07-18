using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly WarehouseDbContext _db;
    private readonly IDistributedCache _cache;

    public SupplierRepository(WarehouseDbContext context, IDistributedCache cache)
    {
        _db = context;
        _cache = cache;
    }

    public async Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken)
    {
        string cacheKey = "Suppliers";

        string? cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            return JsonSerializer.Deserialize<List<Supplier>>(cachedValue);
        }

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
        return suppliers;
    }

    public async Task<Supplier?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        string cacheKey = $"Supplier:{id}";

        string? cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            return JsonSerializer.Deserialize<Supplier>(cachedValue);
        }

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
        }

        return supplier;
    }

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        await _db.Suppliers.AddAsync(supplier, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync("Suppliers", cancellationToken);
    }

    public async Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        _db.Suppliers.Update(supplier);
        await _db.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync("Suppliers", cancellationToken);
        await _cache.RemoveAsync($"Supplier:{supplier.Id}", cancellationToken);
    }
}