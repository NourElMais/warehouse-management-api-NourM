using System;

namespace NotificationService.Application.IntegrationEvents;

public class LowStockDetectedEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();

    public string ProductId { get; set; }

    public string ProductName { get; set; }

    public int QuantityInStock { get; set; }

    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}
