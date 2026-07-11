using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Products.Commands;

public class AssignSupplierToProductHandler 
    : IRequestHandler<AssignSupplierToProductCommand, ProductViewModel?>
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

    public Task<ProductViewModel> Handle(
        AssignSupplierToProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = _productRepository.GetById(command.ProductId);

        if (product is null)
            return Task.FromResult<ProductViewModel>(null);

        var supplier = _supplierRepository.GetById(command.SupplierId);

        if (supplier is null)
            return Task.FromResult<ProductViewModel>(null);

        product.AssignSupplier(supplier);

        _productRepository.Update(product);
        var viewModel = _mapper.Map<ProductViewModel>(product);

        return Task.FromResult<ProductViewModel?>(viewModel);
    }
}