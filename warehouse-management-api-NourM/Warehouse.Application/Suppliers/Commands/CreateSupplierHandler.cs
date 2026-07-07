using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Commands;

public class CreateSupplierHandler
{
    private readonly ISupplierRepository _supplierRepository;

    public CreateSupplierHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Supplier Handle(CreateSupplierCommand command)
    {
        var supplier = new Supplier(
            command.Name,
            command.Country,
            command.ContactEmail,
            command.PhoneNumber
        );

        _supplierRepository.Add(supplier);

        return supplier;
    }
}