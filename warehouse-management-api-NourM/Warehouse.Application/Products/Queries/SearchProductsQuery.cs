namespace Warehouse.Application.Products.Queries;

public class SearchProductsQuery
{
    public string SearchTerm { get; set; }

    public SearchProductsQuery(string searchTerm)
    {
        SearchTerm = searchTerm;
    }
}