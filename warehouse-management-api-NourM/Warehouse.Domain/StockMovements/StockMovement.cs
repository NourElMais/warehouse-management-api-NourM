namespace Warehouse.Domain.StockMovements;

public class StockMovement
{
    public string ProductId { get; private set; }
    public int QuantityChanged { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public StockMovement(string productId, int quantityChanged)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product Id is required.");

        if (quantityChanged == 0)
            throw new ArgumentException("Quantity changed cannot be zero.");

        ProductId = productId;
        QuantityChanged = quantityChanged;
        CreatedAt = DateTime.UtcNow;
    }
}