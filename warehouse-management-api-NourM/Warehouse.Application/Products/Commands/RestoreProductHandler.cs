using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class RestoreProductHandler
{
    private readonly IProductRepository _productRepository;
    public RestoreProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product? Handle(RestoreProductCommand command)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return null;

        product.Restore();

        _productRepository.Update(product);

        return product;
    }
}