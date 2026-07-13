using Microsoft.EntityFrameworkCore;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly WarehouseDbContext _db;

    public ProductRepository(WarehouseDbContext context)
    {
        _db = context;
    }

    public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Products.Include(product => product.Supplier)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _db.Products.Include(product => product.Supplier).FirstOrDefaultAsync(
                product => product.Id == id,
                cancellationToken);
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

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _db.Products.AddAsync(product, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}