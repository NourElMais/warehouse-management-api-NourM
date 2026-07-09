using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class ArchiveProductHandler 
    : IRequestHandler<ArchiveProductCommand, Product?>
{
    private readonly IProductRepository _productRepository;

    public ArchiveProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Product?> Handle(
        ArchiveProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return Task.FromResult<Product?>(null);

        product.Archive();

        _productRepository.Update(product);

        return Task.FromResult<Product?>(product);
    }
}