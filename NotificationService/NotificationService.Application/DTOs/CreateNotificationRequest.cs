using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Presentation;

public class CreateNotificationRequest
{
    public NotificationSeverity Severity { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public string RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; }
}