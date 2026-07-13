using Microsoft.EntityFrameworkCore;
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
        return _db.Products.ToList();
    }

    public Product? GetById(string id)
    {
        return _db.Products
            .FirstOrDefault(p => p.Id == id);
    }

    public List<Product> Search(string? name, string? supplier)
    {
        // we include the Supplier so that we do not call product.Supplier.Name on a null Supplier
        List<Product> products = _db.Products.Include(p => p.Supplier).ToList();
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
       _db.Products.Add(product);
       _db.SaveChanges();
    }

    public void Update(Product product)
    {
        _db.SaveChanges();
    }
}