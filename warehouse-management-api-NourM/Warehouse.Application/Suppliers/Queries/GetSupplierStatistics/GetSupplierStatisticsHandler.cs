using MediatR;
using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierStatisticsHandler
    : IRequestHandler<GetSupplierStatisticsQuery, GetSupplierStatisticsResponse>
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSupplierStatisticsHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public async Task<GetSupplierStatisticsResponse> Handle(
        GetSupplierStatisticsQuery request,
        CancellationToken cancellationToken)
    {
        var suppliers = await _supplierRepository.GetAllAsync(cancellationToken);

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

        var statistics = new GetSupplierStatisticsResponse()
        {
            TotalSuppliers = totalSuppliers,
            ActiveSuppliers = activeSuppliers,
            InactiveSuppliers = inactiveSuppliers
        };
        

        return statistics;
    }
}