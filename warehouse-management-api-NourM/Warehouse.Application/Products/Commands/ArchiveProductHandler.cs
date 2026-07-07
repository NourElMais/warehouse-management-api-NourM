using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class ArchiveProductHandler
{
    private readonly IProductRepository _productRepository;

    public ArchiveProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product? Handle(ArchiveProductCommand command)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return null;

        product.Archive();

        _productRepository.Update(product);

        return product;
    }
}