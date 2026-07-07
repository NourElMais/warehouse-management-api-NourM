using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierByIdHandler
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSupplierByIdHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Supplier? Handle(GetSupplierByIdQuery query)
    {
        return _supplierRepository.GetById(query.Id);
    }
}