using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class UpdateProductPriceHandler 
    : IRequestHandler<UpdateProductPriceCommand, Product?>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductPriceHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<Product?> Handle(
        UpdateProductPriceCommand command,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return Task.FromResult<Product?>(null);

        decimal oldPrice = product.Price;

        product.UpdatePrice(command.NewPrice);

        _productRepository.Update(product);

        Console.WriteLine(
            $"Product {product.Name} price changed from {oldPrice} to {product.Price}");

        return Task.FromResult<Product?>(product);
        }
}