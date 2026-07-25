namespace Warehouse.Notifications.Domain.Entities;

public class NotificationPreference
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string NotificationType { get; set; }

    public NotificationSeverity Severity { get; set; }
}