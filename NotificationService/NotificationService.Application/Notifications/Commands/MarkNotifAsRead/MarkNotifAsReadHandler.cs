using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NotificationService.Application.Interfaces;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Commands.MarkNotifAsRead;

public class MarkNotifAsReadHandler : IRequestHandler<MarkNotifAsReadCommand, Notification>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotifAsReadHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Notification> Handle(MarkNotifAsReadCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (notification is null)
        {
            return null;
        }

        notification.Status = "Read";

        await _notificationRepository.SaveChangesAsync(cancellationToken);

        return notification;
    }
    }
