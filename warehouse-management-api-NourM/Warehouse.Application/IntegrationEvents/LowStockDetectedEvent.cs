namespace Warehouse.Application.IntegrationEvents;

public class LowStockDetectedEvent
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; } 

    public int QuantityInStock { get; set; }

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}