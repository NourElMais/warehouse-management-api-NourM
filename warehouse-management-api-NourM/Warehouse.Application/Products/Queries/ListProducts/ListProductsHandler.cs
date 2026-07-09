using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class ListProductsHandler 
    : IRequestHandler<ListProductsQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;

    public ListProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<List<Product>> Handle(
        ListProductsQuery query,
        CancellationToken cancellationToken)
    {
        List<Product> products = _productRepository.GetAll();

        if (!query.OnlyAvailable)
        {
            return Task.FromResult(products);
        }

        List<Product> availableProducts = new List<Product>();

        foreach (Product product in products)
        {
            if (!product.IsArchived)
            {
                availableProducts.Add(product);
            }
        }

        return Task.FromResult(availableProducts);
    }
}