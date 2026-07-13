using Warehouse.Domain.Suppliers;

namespace Warehouse.Domain.Repositories;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<Supplier?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken);

    Task AddAsync(
        Supplier supplier,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Supplier supplier,
        CancellationToken cancellationToken);
}