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
        List<Product> products = _productRepository.GetAll();
        List<Product> result = new List<Product>();

        foreach (Product product in products)
        {
            bool matches = true;

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                if (!product.Name.Contains(query.Name, StringComparison.OrdinalIgnoreCase))
                {
                    matches = false;
                }
            }

            if (!string.IsNullOrWhiteSpace(query.Supplier))
            {
                if (!product.SupplierName.Contains(query.Supplier, StringComparison.OrdinalIgnoreCase))
                {
                    matches = false;
                }
            }

            if (matches)
            {
                result.Add(product);
            }
        }

        return result;
    }
}