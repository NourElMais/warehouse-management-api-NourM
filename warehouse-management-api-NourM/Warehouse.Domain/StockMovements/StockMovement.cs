using Warehouse.Domain.Products;

namespace Warehouse.Domain.StockMovements;

public class StockMovement
{
    public string Id { get; private set; }
    public string ProductId { get; private set; }
    public int QuantityChanged { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    //We add the navigation property:
    public virtual Product? Product { get; private set; }

    private StockMovement()
    {
        
    }
    public StockMovement(string productId, int quantityChanged, string? id = null)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product Id is required.");

        if (quantityChanged == 0)
            throw new ArgumentException("Quantity changed cannot be zero.");
        
        Id = id?? Guid.NewGuid().ToString();
        ProductId = productId;
        QuantityChanged = quantityChanged;
        CreatedAt = DateTime.UtcNow;
    }
}