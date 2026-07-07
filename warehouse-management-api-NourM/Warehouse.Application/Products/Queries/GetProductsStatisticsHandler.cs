using MediatR;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class GetProductsStatisticsHandler
    : IRequestHandler<GetProductsStatisticsQuery, object>
{
    private readonly IProductRepository _productRepository;

    public GetProductsStatisticsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<object> Handle(
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

        object statistics = new
        {
            TotalProducts = totalProducts,
            ActiveProducts = activeProducts,
            ArchivedProducts = archivedProducts,
            LowStockProducts = lowStockProducts
        };

        return Task.FromResult(statistics);
    }
}