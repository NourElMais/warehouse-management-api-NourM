namespace NotificationService.Presentation;

public class CreateNotificationRequest
{
    public string Severity { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }
    public string RelatedEntityId { get; set; }
    public string RelatedEntityType { get; set; }
}