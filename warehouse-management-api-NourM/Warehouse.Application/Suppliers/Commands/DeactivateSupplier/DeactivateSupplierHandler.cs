using AutoMapper;
using MediatR;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class DeactivateSupplierHandler 
    : IRequestHandler<DeactivateSupplierCommand, SupplierViewModel?>
{
    private readonly ISupplierRepository _supplierRepository;
    private IMapper _mapper;
    public DeactivateSupplierHandler(ISupplierRepository supplierRepository, IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public Task<SupplierViewModel?> Handle(
        DeactivateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = _supplierRepository.GetById(request.SupplierId);

        if (supplier is null)
            return Task.FromResult<SupplierViewModel?>(null);

        supplier.Deactivate();

        _supplierRepository.Update(supplier);

        var viewModel = _mapper.Map<SupplierViewModel>(supplier);

        return Task.FromResult(viewModel);
    }
}