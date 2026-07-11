namespace Warehouse.Presentation.ViewModels;

public class ProductViewModel
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string SKU { get; set; } = "";
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsArchived { get; set; }

    public string SupplierId { get; set; } = "";
    public string? SupplierName { get; set; }
}