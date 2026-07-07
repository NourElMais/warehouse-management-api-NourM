namespace Warehouse.Application.Products.Commands;

public class UpdateProductPriceCommand
{
    public string ProductId { get; set; }
    public decimal NewPrice { get; set; }

    public UpdateProductPriceCommand(string productId, decimal newPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
    }
}