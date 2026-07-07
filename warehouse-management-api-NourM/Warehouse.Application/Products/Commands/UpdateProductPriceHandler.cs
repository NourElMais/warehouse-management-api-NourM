using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UpdateProductPriceHandler
{
    private readonly IProductRepository _productRepository;

    public UpdateProductPriceHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product? Handle(UpdateProductPriceCommand command)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return null;

        product.UpdatePrice(command.NewPrice);

        _productRepository.Update(product);

        return product;
    }
}