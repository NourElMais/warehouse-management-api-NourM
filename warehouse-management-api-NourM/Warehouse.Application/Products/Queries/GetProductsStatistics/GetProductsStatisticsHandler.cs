using MediatR;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.GetProductsStatistics;

public class GetProductsStatisticsHandler
    : IRequestHandler<GetProductsStatisticsQuery, GetProductsStatisticsResponse>
{
    private const int LowStockThreshold = 5;
    private readonly IProductRepository _productRepository;

    public GetProductsStatisticsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<GetProductsStatisticsResponse> Handle(
        GetProductsStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        return CreateStatistics(products);
    }

    private static GetProductsStatisticsResponse CreateStatistics(IEnumerable<Product> products)
    {
        int totalProducts = 0;
        int activeProducts = 0;
        int archivedProducts = 0;
        int lowStockProducts = 0;

        foreach (var product in products)
        {
            totalProducts++;

            if (product.IsArchived)
            {
                archivedProducts++;

                continue;
            }

            activeProducts++;

            if (product.QuantityInStock <= LowStockThreshold)
            {
                lowStockProducts++;
            }
        }

        return new GetProductsStatisticsResponse
        {
            TotalProducts = totalProducts,
            ActiveProducts = activeProducts,
            ArchivedProducts = archivedProducts,
            LowStockProducts = lowStockProducts
        };
    }
}