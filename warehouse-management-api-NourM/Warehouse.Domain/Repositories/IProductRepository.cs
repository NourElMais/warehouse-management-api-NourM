using Warehouse.Domain.Products;

namespace Warehouse.Domain.Repositories;

public interface IProductRepository
{
    public List<Product> GetAll();
    public Product? GetById(string id);
    public List<Product> Search(string searchTerm);
    public void Add(Product product);
    public void Update(Product product);
}