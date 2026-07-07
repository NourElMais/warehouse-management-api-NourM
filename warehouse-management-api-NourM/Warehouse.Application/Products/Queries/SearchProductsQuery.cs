namespace Warehouse.Application.Products.Queries;

public class SearchProductsQuery
{
    public string? Name { get; set; }
    public string? Supplier { get; set; }

    public SearchProductsQuery(string? name, string? supplier)
    {
        Name = name;
        Supplier = supplier;
    }
}