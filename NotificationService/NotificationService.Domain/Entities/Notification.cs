namespace Warehouse.Notifications.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid(); //to identify the notification

    public string Type { get; set; } //specifies the nature of the notif (lowstock...)

    public string Title { get; set; }  // short title describing what the notif is about

    public string Message { get; set; }  //whole message

    public string Severity { get; set; } //warning, info...

    public string Status { get; set; } = "Unread"; //default status of the notif

    public string RelatedEntityId { get; set; } //id of the entity that the notification is tackling

    public string? RelatedEntityType { get; set; } //for example Product, Supplier...

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}