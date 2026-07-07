using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class DeactivateSupplierHandler
{
    private readonly ISupplierRepository _supplierRepository;

    public DeactivateSupplierHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Supplier? Handle(DeactivateSupplierCommand command)
    {
        var supplier = _supplierRepository.GetById(command.SupplierId);

        if (supplier is null)
            return null;

        supplier.Deactivate();

        _supplierRepository.Update(supplier);

        return supplier;
    }
}