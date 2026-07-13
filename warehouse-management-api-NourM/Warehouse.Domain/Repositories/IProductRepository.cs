using Warehouse.Domain.Products;

namespace Warehouse.Domain.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<Product?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken);

    Task<List<Product>> SearchAsync(
        string? name,
        string? supplier,
        CancellationToken cancellationToken);

    Task AddAsync(
        Product product,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Product product,
        CancellationToken cancellationToken);
}