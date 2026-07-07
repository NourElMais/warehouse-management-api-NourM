using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UpdateProductQuantityHandler
{
    private readonly IProductRepository _productRepository;

    public UpdateProductQuantityHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product? Handle(UpdateProductQuantityCommand command)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return null;

        product.UpdateQuantity(command.NewQuantity);

        _productRepository.Update(product);

        return product;
    }
}