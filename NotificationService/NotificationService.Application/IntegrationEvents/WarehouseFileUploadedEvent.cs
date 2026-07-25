namespace NotificationService.Application.IntegrationEvents;

public class WarehouseFileUploadedEvent
{
    public Guid EventId { get; set; } 
    
    public string CorrelationId { get; set; }

    public DateTime EventTime { get; set; }

    public string EventType { get; set; } 

    public string RelatedEntityId { get; set; }

    public string RelatedEntityType { get; set; }

    public string FileName { get; set; }

    public string Severity { get; set; } 
}