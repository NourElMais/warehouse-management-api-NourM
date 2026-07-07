namespace Warehouse.Application.Products.Queries;

public class GetLowStockProductsQuery
{
    public int Threshold { get; set; }

    public GetLowStockProductsQuery(int threshold)
    {
        Threshold = threshold;
    }
}