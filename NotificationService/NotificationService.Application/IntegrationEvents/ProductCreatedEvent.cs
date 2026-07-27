namespace NotificationService.Application.IntegrationEvents;

public class ProductCreatedEvent
{
    public Guid EventId { get; set; }

    public Guid ProductId { get; set; } 

    public string ProductName { get; set; } 

    public string SKU { get; set; } 

    public DateTime OccurredAt { get; set; }
}