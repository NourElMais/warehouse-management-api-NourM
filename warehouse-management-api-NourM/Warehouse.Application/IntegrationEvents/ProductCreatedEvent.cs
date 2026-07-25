namespace Warehouse.Application.IntegrationEvents;

public class ProductCreatedEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid(); //to prevent duplicate notifications.

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}