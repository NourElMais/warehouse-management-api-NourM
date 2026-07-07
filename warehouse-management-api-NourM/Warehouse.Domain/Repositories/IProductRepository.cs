using Warehouse.Domain.Products;

namespace Warehouse.Domain.Repositories;

public class IProductRepository
{
    List<Product> GetAll();
    public Product? GetById(string id);
    List<Product> Search(string searchTerm);
    public void Add(Product product);
    public void Update(Product product);
}