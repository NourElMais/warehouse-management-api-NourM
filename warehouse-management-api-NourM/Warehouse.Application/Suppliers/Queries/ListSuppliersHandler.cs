using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class ListSuppliersHandler
{
    private readonly ISupplierRepository _supplierRepository;

    public ListSuppliersHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public List<Supplier> Handle(ListSuppliersQuery query)
    {
        return _supplierRepository.GetAll();
    }
}