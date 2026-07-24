using MediatR;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Commands.CreateNotification;

public class CreateNotificationCommand:IRequest<Notification>
{
    public string Severity { get; }
    public string Title { get; }
    public string Type { get; }
    public string Message { get; }
    public string RelatedEntityId { get; }
    public string RelatedEntityType { get; }

    public CreateNotificationCommand(string Severity,string Title, string Type, string Message, string RelatedEntityId, string RelatedEntityType)
    {
        this.Severity = Severity;
        this.Title = Title;
        this.Type = Type;
        this.Message = Message;
        this.RelatedEntityId = RelatedEntityId;
        this.RelatedEntityType = RelatedEntityType;
    }
}