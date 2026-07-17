using AutoMapper;
using MediatR;
using Warehouse.Application.Exceptions;
using Warehouse.Application.ViewModels;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Suppliers.Commands;

public class DeactivateSupplierHandler 
    : IRequestHandler<DeactivateSupplierCommand, SupplierViewModel>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    public DeactivateSupplierHandler(ISupplierRepository supplierRepository, IMapper mapper)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
    }

    public async Task<SupplierViewModel> Handle(
        DeactivateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId, cancellationToken);

        if (supplier is null)
            throw new NotFoundException("The supplier was not found");

        supplier.Deactivate();

        await _supplierRepository.UpdateAsync(supplier, cancellationToken);

        return _mapper.Map<SupplierViewModel>(supplier);
        
    }
}