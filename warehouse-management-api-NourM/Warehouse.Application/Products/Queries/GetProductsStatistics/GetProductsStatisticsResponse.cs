namespace Warehouse.Application.Products.Queries;

public class GetProductsStatisticsResponse
{
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }
    public int ArchivedProducts { get; set; }
    public int LowStockProducts { get; set; }
}