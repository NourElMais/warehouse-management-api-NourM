using Warehouse.Domain.Products;

namespace Warehouse.Domain.Repositories;

public class IProductRepository
{
    List<Product> GetAll();
    Product? GetById(string id);
    List<Product> Search(string searchTerm);
    void Add(Product product);
    void Update(Product product);
}