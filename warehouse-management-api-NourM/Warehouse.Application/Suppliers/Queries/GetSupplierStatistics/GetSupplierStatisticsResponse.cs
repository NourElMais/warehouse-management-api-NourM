namespace Warehouse.Application.Suppliers.Queries;

public class GetSupplierStatisticsResponse
{
    public  int TotalSuppliers { get; set; }
    public  int ActiveSuppliers { get; set; } 
    public  int InactiveSuppliers { get; set; }
}
