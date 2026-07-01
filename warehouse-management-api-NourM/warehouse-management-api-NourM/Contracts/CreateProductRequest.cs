namespace warehouse_management_api_NourM.Contracts;

public class CreateProductRequest
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public string SupplierName { get; set; }
    public DateTime ExpiryDate { get; set; }
}