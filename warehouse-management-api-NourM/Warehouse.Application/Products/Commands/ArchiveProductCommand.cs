namespace Warehouse.Application.Products.Commands;

public class ArchiveProductCommand
{
    public string ProductId { get; set; }

    public ArchiveProductCommand(string productId)
    {
        ProductId = productId;
    }
}