using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Infrastructure.Repositories;

public class ProductRepository:IProductRepository
{
    private readonly WarehouseDbContext _db;

    public ProductRepository(WarehouseDbContext context)
    {
        _db = context;
    }
    public List<Product> GetAll()
    {
        return _db.products.ToList();
    }

    public Product? GetById(string id)
    {
        return _db.products
            .FirstOrDefault(p => p.Id == id);
    }

    public List<Product> Search(string? name, string? supplier)
    {
        List<Product> products = _db.products.ToList();
        List<Product> result = new List<Product>();

        foreach (Product product in products)
        {
            bool nameMatches = string.IsNullOrWhiteSpace(name) ||
                               product.Name.Contains(name, StringComparison.OrdinalIgnoreCase);

            bool supplierMatches = string.IsNullOrWhiteSpace(supplier) ||
                                   product.Supplier.Name.Contains(supplier, StringComparison.OrdinalIgnoreCase);

            if (nameMatches && supplierMatches)
            {
                result.Add(product);
            }
        }

        return result;
    }
    public void Add(Product product)
    {
       _db.products.Add(product);
       _db.SaveChanges();
    }
}