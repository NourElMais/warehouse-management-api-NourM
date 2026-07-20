namespace Warehouse.Infrastructure.Caching;

public static class CacheKeys
{
    public const string Products = "Products";
    public static string Product(string id)
    {
        return $"Product:{id}";
    }
    
    public const string Suppliers = "Suppliers";
    public static string Supplier(string id)
    {
        return $"Supplier:{id}";
    }
    
}