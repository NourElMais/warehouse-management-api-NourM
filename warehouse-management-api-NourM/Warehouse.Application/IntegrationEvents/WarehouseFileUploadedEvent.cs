namespace Warehouse.Application.IntegrationEvents;

public class WarehouseFileUploadedEvent
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();

    public DateTime EventTime { get; set; } = DateTime.UtcNow;

    public string EventType { get; set; } = "WarehouseFileUploaded";

    public string RelatedEntityId { get; set; }

    public string RelatedEntityType { get; set; }

    public string FileName { get; set; }

    public string Severity { get; set; } = "Information";
    
}