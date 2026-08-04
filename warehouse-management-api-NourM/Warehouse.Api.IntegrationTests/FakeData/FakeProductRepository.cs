using Warehouse.Domain.ProductImages;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Api.IntegrationTests.FakeData;

public class FakeProductRepository:IProductRepository
{
    private readonly List<Product> _products;
    private readonly List<ProductImage> _productImages;

    public FakeProductRepository()
    {
        _productImages = [];
        _products=
        [
            new Product("Laptop","lap/123","Gaming laptop",1200,23,"supplier-id1",DateTime.UtcNow.AddYears(2),"35feb37b-05e6-4b53-bb7b-264ecc8714c1"),
            new Product("Mouse","mouse/123","Wireless mouse", 100, 8,"supplier-id2", DateTime.UtcNow.AddYears(1), "c50d9e28-60be-407d-a163-1af84755c3e0"),
            new Product("Headset","head/123","Noise cancelling headset", 200, 0,"supplier-id3", DateTime.UtcNow.AddYears(1), "b3f3a7b2-2d0b-48db-a5dd-6ae2d8c3c111")
        ];
    }

    public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_products.ToList());
    }

    public Task<Product?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var product = _products.FirstOrDefault(x => x.Id == id);

        return Task.FromResult(product);
    }

    public Task AddAsync(Product entity, CancellationToken cancellationToken)
    {
        _products.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product entity, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task<List<Product>> SearchAsync(string? name, string? supplier, CancellationToken cancellationToken)
    {
        var products = _products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            products = products.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(products.ToList());
    }

    public Task<List<Product>> GetExpiringSoonAsync(int daysAhead, CancellationToken cancellationToken)
    {
        DateTime today = DateTime.Today;
        DateTime endDate = today.AddDays(daysAhead);

        var products = _products
            .Where(product => !product.IsArchived &&
                              product.ExpiryDate >= today &&
                              product.ExpiryDate <= endDate)
            .OrderBy(product => product.ExpiryDate)
            .ToList();

        return Task.FromResult(products);
    }

    public Task AddImageAsync(ProductImage image, CancellationToken cancellationToken)
    {
        _productImages.Add(image);
        return Task.CompletedTask;
    }

    public Task<ProductImage?> GetImageAsync(string productId, CancellationToken cancellationToken)
    {
        var image = _productImages.FirstOrDefault(productImage => productImage.ProductId == productId);

        return Task.FromResult(image);
    }

    public Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken)
    {
        var product = _products.FirstOrDefault(x => x.SKU == sku);

        return Task.FromResult(product);
    }
}