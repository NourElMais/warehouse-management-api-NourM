using Warehouse.Domain.Products;

namespace Warehouse.Domain.Repositories;

public interface IProductRepository: IRepository<Product>
{
    Task<List<Product>> SearchAsync(string? name, string? supplier, CancellationToken cancellationToken);
}