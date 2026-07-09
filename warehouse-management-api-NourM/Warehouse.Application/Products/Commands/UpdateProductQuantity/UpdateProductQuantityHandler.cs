using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UpdateProductQuantityHandler 
    : IRequestHandler<UpdateProductQuantityCommand, Product?>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductQuantityHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Product?> Handle(
        UpdateProductQuantityCommand command,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return Task.FromResult<Product?>(null);

        product.UpdateQuantity(command.NewQuantity);

        _productRepository.Update(product);

        return Task.FromResult<Product?>(product);
    }
}