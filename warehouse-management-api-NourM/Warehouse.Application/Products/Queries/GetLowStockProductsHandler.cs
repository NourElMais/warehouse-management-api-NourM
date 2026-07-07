using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class GetLowStockProductsHandler
{
    private readonly IProductRepository _productRepository;

    public GetLowStockProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<Product> Handle(GetLowStockProductsQuery query)
    {
        return _productRepository
            .GetAll()
            .Where(p => !p.IsArchived && p.QuantityInStock <= query.Threshold)
            .ToList();
    }
}