namespace Warehouse.Application.Products.Queries;

public class ListProductsQuery
{
    public bool OnlyAvailable { get; set; }

    public ListProductsQuery(bool onlyAvailable)
    {
        OnlyAvailable = onlyAvailable;
    }
}