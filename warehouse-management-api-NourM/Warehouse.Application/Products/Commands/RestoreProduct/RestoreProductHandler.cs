using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class RestoreProductHandler 
    : IRequestHandler<RestoreProductCommand, Product?>
{
    private readonly IProductRepository _productRepository;

    public RestoreProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Product?> Handle(
        RestoreProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return Task.FromResult<Product?>(null);

        product.Restore();

        _productRepository.Update(product);

        return Task.FromResult<Product?>(product);
    }
}