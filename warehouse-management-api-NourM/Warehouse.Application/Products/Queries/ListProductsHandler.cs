using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class ListProductsHandler
{
    private readonly IProductRepository _productRepository;

    public ListProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<Product> Handle(ListProductsQuery query)
    {
        return _productRepository.GetAll();
    }
}