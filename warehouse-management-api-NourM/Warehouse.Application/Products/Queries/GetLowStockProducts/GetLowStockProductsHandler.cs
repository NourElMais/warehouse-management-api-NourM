using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class GetLowStockProductsHandler
    : IRequestHandler<GetLowStockProductsQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetLowStockProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<List<Product>> Handle(
        GetLowStockProductsQuery query,
        CancellationToken cancellationToken)
    {
        List<Product> products = _productRepository.GetAll();
        List<Product> lowStockProducts = new List<Product>();

        foreach (Product product in products)
        {
            if (!product.IsArchived &&
                product.QuantityInStock <= query.Threshold)
            {
                lowStockProducts.Add(product);
            }
        }

        return Task.FromResult(lowStockProducts);
    }
}