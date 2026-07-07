namespace Warehouse.Domain.WareHouseItems;

public class WareHouseItem
{
    public string ProductId { get; private set; }
    public int Quantity { get; private set; }

    public WareHouseItem(string productId, int quantity)
    {
        if (string.IsNullOrWhiteSpace(productId))
            throw new ArgumentException("Product Id is required.");

        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative.");

        ProductId = productId;
        Quantity = quantity;
    }
}