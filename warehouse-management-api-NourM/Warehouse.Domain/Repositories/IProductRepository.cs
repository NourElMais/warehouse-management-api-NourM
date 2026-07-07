using Warehouse.Domain.Products;

namespace Warehouse.Domain.Repositories;

public interface IProductRepository
{
    public List<Product> GetAll();
    public Product? GetById(string id);
    List<Product> Search(string? name, string? supplier);
    public void Add(Product product);
    public void Update(Product product);
}