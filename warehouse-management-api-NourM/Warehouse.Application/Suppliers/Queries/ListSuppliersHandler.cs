using MediatR;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class ListSuppliersHandler
    : IRequestHandler<ListSuppliersQuery, List<Supplier>>
{
    private readonly ISupplierRepository _supplierRepository;

    public ListSuppliersHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Task<List<Supplier>> Handle(
        ListSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        List<Supplier> suppliers = _supplierRepository.GetAll();

        return Task.FromResult(suppliers);
    }
}