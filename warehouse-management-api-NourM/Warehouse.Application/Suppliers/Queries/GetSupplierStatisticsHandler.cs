using MediatR;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierStatisticsHandler
    : IRequestHandler<GetSupplierStatisticsQuery, object>
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSupplierStatisticsHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public Task<object> Handle(
        GetSupplierStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var suppliers = _supplierRepository.GetAll();

        int totalSuppliers = 0;
        int activeSuppliers = 0;
        int inactiveSuppliers = 0;

        foreach (var supplier in suppliers)
        {
            totalSuppliers++;

            if (supplier.IsActive)
            {
                activeSuppliers++;
            }
            else
            {
                inactiveSuppliers++;
            }
        }

        object statistics = new
        {
            TotalSuppliers = totalSuppliers,
            ActiveSuppliers = activeSuppliers,
            InactiveSuppliers = inactiveSuppliers
        };

        return Task.FromResult(statistics);
    }
}