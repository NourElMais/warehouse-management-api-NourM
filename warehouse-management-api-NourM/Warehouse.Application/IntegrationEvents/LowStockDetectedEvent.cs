namespace Warehouse.Application.IntegrationEvents;

public class LowStockDetectedEvent
{
    public string ProductId { get; set; }
    
    public Guid EventId { get; set; } = Guid.NewGuid();

    public string ProductName { get; set; } 

    public int QuantityInStock { get; set; }

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}