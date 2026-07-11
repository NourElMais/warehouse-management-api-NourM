using MediatR;
using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class AssignSupplierToProductHandler 
    : IRequestHandler<AssignSupplierToProductCommand, Product?>
{
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;

    public AssignSupplierToProductHandler(
        IProductRepository productRepository,
        ISupplierRepository supplierRepository)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
    }

    public Task<Product?> Handle(
        AssignSupplierToProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return Task.FromResult<Product?>(null);

        var supplier = _supplierRepository.GetById(command.SupplierId);

        if (supplier is null)
            return Task.FromResult<Product?>(null);

        product.AssignSupplier(supplier);

        _productRepository.Update(product);

        return Task.FromResult<Product?>(product);
    }
}