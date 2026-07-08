using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Tests.Application;

public class FakeProductRepository : IProductRepository
{
    public bool AddWasCalled { get; private set; }

    public List<Product> Products { get; } = new List<Product>();

    public List<Product> GetAll()
    {
        return Products;
    }

    public Product? GetById(string id)
    {
        return null;
    }

    public List<Product> Search(string? name, string? supplier)
    {
        return Products;
    }

    public void Add(Product product)
    {
        AddWasCalled = true;
        Products.Add(product);
    }

    public void Update(Product product)
    {
    }
}