namespace Warehouse.Application.Products.Commands;

public class RestoreProductCommand
{
    public string ProductId { get; set; }

    public RestoreProductCommand(string productId)
    {
        ProductId = productId;
    }
}