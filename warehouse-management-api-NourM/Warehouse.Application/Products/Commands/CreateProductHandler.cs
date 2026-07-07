using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class CreateProductHandler
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product Handle(CreateProductCommand command)
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

        return product;
    }
}