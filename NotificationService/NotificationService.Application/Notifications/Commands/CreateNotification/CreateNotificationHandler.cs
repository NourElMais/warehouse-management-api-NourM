using MediatR;
using NotificationService.Application.Interfaces;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Commands.CreateNotification;

public class CreateNotificationHandler: IRequestHandler<CreateNotificationCommand,Notification>
{
    private readonly INotificationRepository _notificationRepository;

    public CreateNotificationHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Notification> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification {
            Type = request.Type,
            Title = request.Title,
            Message = request.Message,
            Severity = request.Severity,
            Status = "Unread",
            RelatedEntityId = request.RelatedEntityId,
            RelatedEntityType = request.RelatedEntityType,
        };

        await _notificationRepository.AddAsync(notification, cancellationToken);

        await _notificationRepository.SaveChangesAsync(cancellationToken);

        return notification;
    }
    
}