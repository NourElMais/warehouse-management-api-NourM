using Warehouse.Domain.Products;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class AssignSupplierToProductHandler
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

    public Product? Handle(AssignSupplierToProductCommand command)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return null;

        var supplier = _supplierRepository.GetById(command.SupplierId);

        if (supplier is null)
            return null;

        product.AssignSupplier(supplier.Name, supplier.IsActive);

        _productRepository.Update(product);

        return product;
    }
}