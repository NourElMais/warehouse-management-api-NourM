namespace Warehouse.Application.Products.Commands;

public class CreateProductCommand
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public string SupplierName { get; set; }
    public DateTime ExpiryDate { get; set; }
}