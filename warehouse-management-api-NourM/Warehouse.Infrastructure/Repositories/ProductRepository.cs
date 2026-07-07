using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure.Repositories;

public class ProductRepository:IProductRepository
{
    public List<Product> GetAll()
    {
        return FakeWarehouseStore.Products;
    }

    public Product? GetById(string id)
    {
        return FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id);
    }

    public List<Product> Search(string searchTerm)
    {
        return FakeWarehouseStore.Products
            .Where(p =>
                p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.SKU.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public void Add(Product product)
    {
        FakeWarehouseStore.Products.Add(product);
    }

    public void Update(Product product)
    {
        // In-memory list stores the same object reference,
        // so no extra code is needed for now.
    }
}