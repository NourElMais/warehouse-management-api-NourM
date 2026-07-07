using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class SearchProductsHandler
    : IRequestHandler<SearchProductsQuery, List<Product>>
{
    private readonly IProductRepository _productRepository;

    public SearchProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<List<Product>> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        List<Product> products = _productRepository.Search(request.Name, request.Supplier);

        return Task.FromResult(products);
    }
}