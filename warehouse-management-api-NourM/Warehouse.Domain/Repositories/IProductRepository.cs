using Warehouse.Domain.ProductImages;
using Warehouse.Domain.Products;

namespace Warehouse.Domain.Repositories;

public interface IProductRepository: IRepository<Product>
{
    Task<List<Product>> SearchAsync(string? name, string? supplier, CancellationToken cancellationToken);
    Task AddImageAsync(ProductImage image, CancellationToken cancellationToken);
    Task<ProductImage?> GetImageAsync(string productId, CancellationToken cancellationToken);
}