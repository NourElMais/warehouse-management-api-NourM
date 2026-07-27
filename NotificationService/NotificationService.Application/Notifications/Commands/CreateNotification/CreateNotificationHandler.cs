using System.Threading;
using System.Threading.Tasks;
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
            Type = request.Request.Type,
            Title = request.Request.Title,
            Message = request.Request.Message,
            Severity = request.Request.Severity,
            Status = "Unread",
            RelatedEntityId = request.Request.RelatedEntityId,
            RelatedEntityType = request.Request.RelatedEntityType,
        };

        await _notificationRepository.AddAsync(notification, cancellationToken);

        await _notificationRepository.SaveChangesAsync(cancellationToken);

        return notification;
    }
    
}