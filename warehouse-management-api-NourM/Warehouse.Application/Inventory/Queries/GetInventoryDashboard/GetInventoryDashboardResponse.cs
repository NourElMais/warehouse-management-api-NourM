using Warehouse.Application.Products.Queries;
using Warehouse.Application.Suppliers.Queries;

namespace Warehouse.Application.Inventory.Queries.GetInventoryDashboard;

public class GetInventoryDashboardResponse
{
    //I will use the classes I did previously
    public GetProductsStatisticsResponse ProductStatistics { get; set; }
    public GetSupplierStatisticsResponse SupplierStatistics { get; set; }
}