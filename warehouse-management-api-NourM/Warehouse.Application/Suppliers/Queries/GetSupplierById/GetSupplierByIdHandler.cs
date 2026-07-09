using MediatR;
using Warehouse.Domain.Repositories;
using Warehouse.Domain.Suppliers;

namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierByIdHandler
    : IRequestHandler<GetSupplierByIdQuery, Supplier?>
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSupplierByIdHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Task<Supplier?> Handle(
        GetSupplierByIdQuery request,
        CancellationToken cancellationToken)
    {
        Supplier? supplier = _supplierRepository.GetById(request.Id);

        return Task.FromResult(supplier);
    }
}