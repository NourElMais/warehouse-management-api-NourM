using AutoMapper;
using MediatR;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class AssignSupplierToProductHandler 
    : IRequestHandler<AssignSupplierToProductCommand, ProductViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    public AssignSupplierToProductHandler(
        IProductRepository productRepository,
        ISupplierRepository supplierRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public async Task<ProductViewModel> Handle(AssignSupplierToProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException("ProductNotFound");

        var supplier = await _supplierRepository.GetByIdAsync(command.SupplierId, cancellationToken);

        if (supplier is null)
            throw new NotFoundException("SupplierNotFound");

        product.AssignSupplier(supplier);

        await _productRepository.UpdateAsync(product, cancellationToken);
        return _mapper.Map<ProductViewModel>(product);
    }
}