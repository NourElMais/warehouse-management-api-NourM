using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class GetProductsStatisticsHandler
{
    private readonly IProductRepository _productRepository;

    public GetProductsStatisticsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public object Handle(GetProductsStatisticsQuery query)
    {
        var products = _productRepository.GetAll();

        return new
        {
            TotalProducts = products.Count,
            ActiveProducts = products.Count(p => !p.IsArchived),
            ArchivedProducts = products.Count(p => p.IsArchived),
            LowStockProducts = products.Count(p => !p.IsArchived && p.QuantityInStock <= 5),
        };
    }
}