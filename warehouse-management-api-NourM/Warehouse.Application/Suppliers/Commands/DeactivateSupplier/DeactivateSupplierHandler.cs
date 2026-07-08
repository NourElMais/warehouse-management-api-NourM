using MediatR;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class DeactivateSupplierHandler 
    : IRequestHandler<DeactivateSupplierCommand, Supplier?>
{
    private readonly ISupplierRepository _supplierRepository;

    public DeactivateSupplierHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Task<Supplier?> Handle(
        DeactivateSupplierCommand request,
        CancellationToken cancellationToken)
    {
        var supplier = _supplierRepository.GetById(request.SupplierId);

        if (supplier is null)
            return Task.FromResult<Supplier?>(null);

        supplier.Deactivate();

        _supplierRepository.Update(supplier);

        return Task.FromResult<Supplier?>(supplier);
    }
}