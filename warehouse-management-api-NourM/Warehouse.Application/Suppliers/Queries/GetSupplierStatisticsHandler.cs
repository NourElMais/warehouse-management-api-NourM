using Warehouse.Domain.Repositories;

namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierStatisticsHandler
{
    private readonly ISupplierRepository _supplierRepository;

    public GetSupplierStatisticsHandler(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    public object Handle(GetSupplierStatisticsQuery query)
    {
        var suppliers = _supplierRepository.GetAll();

        return new
        {
            TotalSuppliers = suppliers.Count,
            ActiveSuppliers = suppliers.Count(s => s.IsActive),
            InactiveSuppliers = suppliers.Count(s => !s.IsActive)
        };
    }
}