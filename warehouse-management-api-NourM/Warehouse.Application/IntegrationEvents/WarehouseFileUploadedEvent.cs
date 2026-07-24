namespace Warehouse.Application.IntegrationEvents;

public class WarehouseFileUploadedEvent
{
    public Guid FileId { get; set; }

    public string FileName { get; set; }

    public string UploadedBy { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    
}