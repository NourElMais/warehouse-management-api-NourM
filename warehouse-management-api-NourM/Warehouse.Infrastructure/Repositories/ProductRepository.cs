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

    public List<Product> Search(string? name, string? supplier)
    {
        List<Product> products = FakeWarehouseStore.Products;
        List<Product> result = new List<Product>();

        foreach (Product product in products)
        {
            bool nameMatches = string.IsNullOrWhiteSpace(name) ||
                               product.Name.Contains(name, StringComparison.OrdinalIgnoreCase);

            bool supplierMatches = string.IsNullOrWhiteSpace(supplier) ||
                                   product.SupplierName.Contains(supplier, StringComparison.OrdinalIgnoreCase);

            if (nameMatches && supplierMatches)
            {
                result.Add(product);
            }
        }

        return result;
    }
    public void Add(Product product)
    {
        FakeWarehouseStore.Products.Add(product);
    }

    public void Update(Product product)
    {
        throw new NotImplementedException();
    }
}