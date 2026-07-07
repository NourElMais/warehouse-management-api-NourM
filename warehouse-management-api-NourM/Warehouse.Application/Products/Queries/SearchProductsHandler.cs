using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Queries;

public class SearchProductsHandler
{
    private readonly IProductRepository _productRepository;

    public SearchProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public List<Product> Handle(SearchProductsQuery query)
    {
        return _productRepository.Search(query.SearchTerm);
    }
}