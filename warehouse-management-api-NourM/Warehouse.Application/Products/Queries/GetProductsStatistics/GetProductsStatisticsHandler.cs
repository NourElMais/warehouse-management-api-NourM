using MediatR;
using Warehouse.Application.Products.Queries;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.GetProductsStatistics;

public class GetProductsStatisticsHandler
    : IRequestHandler<GetProductsStatisticsQuery, GetProductsStatisticsResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductsStatisticsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<GetProductsStatisticsResponse> Handle(
        GetProductsStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var products = _productRepository.GetAll();

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
            }
            else
            {
                activeProducts++;

                if (product.QuantityInStock <= 5)
                {
                    lowStockProducts++;
                }
            }
        }

        var statistics = new GetProductsStatisticsResponse
        {
            TotalProducts = totalProducts,
            ActiveProducts = activeProducts,
            ArchivedProducts = archivedProducts,
            LowStockProducts = lowStockProducts
        };

        return Task.FromResult(statistics);
    }
}