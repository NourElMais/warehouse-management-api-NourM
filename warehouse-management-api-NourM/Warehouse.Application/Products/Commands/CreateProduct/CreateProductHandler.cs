using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Product> Handle(
        CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = new Product(
            command.Name,
            command.SKU,
            command.Description,
            command.Price,
            command.QuantityInStock,
            command.SupplierName,
            command.ExpiryDate
        );

        _productRepository.Add(product);

        return Task.FromResult(product);
    }
}