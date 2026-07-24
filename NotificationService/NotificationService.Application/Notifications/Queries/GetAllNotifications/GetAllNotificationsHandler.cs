using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NotificationService.Application.Interfaces;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Queries.GetAllNotifications;

public class GetAllNotificationsHandler: IRequestHandler<GetAllNotificationsQuery, List<Notification>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetAllNotificationsHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    public async Task<List<Notification>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.GetAllAsync(cancellationToken);
    }
}