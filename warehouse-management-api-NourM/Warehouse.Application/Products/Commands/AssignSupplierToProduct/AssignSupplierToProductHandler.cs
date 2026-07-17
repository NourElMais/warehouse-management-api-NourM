using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<AssignSupplierToProductHandler> _logger;
    public AssignSupplierToProductHandler(IProductRepository productRepository, ISupplierRepository supplierRepository,
        IMapper mapper, ILogger<AssignSupplierToProductHandler> logger)
    {
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _logger = logger;
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
        _logger.LogInformation("Assigned Product {ProductId} to supplier {SupplierId}", command.ProductId, command.SupplierId);
        return _mapper.Map<ProductViewModel>(product);
    }
}