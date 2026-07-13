using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Infrastructure.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly WarehouseDbContext _db;

    public SupplierRepository(WarehouseDbContext context)
    {
        _db = context;
    }

    public async Task<List<Supplier>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Suppliers.ToListAsync(cancellationToken);
    }

    public async Task<Supplier?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _db.Suppliers.FirstOrDefaultAsync(supplier => supplier.Id == id, cancellationToken);
    }

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        await _db.Suppliers.AddAsync(supplier, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}