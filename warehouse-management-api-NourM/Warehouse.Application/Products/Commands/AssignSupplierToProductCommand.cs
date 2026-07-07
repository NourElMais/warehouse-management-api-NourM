namespace Warehouse.Application.Products.Commands;

public class AssignSupplierToProductCommand
{
    public string ProductId { get; set; }
    public string SupplierId { get; set; }

    public AssignSupplierToProductCommand(string productId, string supplierId)
    {
        ProductId = productId;
        SupplierId = supplierId;
    }
}