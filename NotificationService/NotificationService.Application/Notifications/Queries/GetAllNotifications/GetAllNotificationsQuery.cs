using System.Collections.Generic;
using MediatR;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Queries.GetAllNotifications;

public class GetAllNotificationsQuery : IRequest<List<Notification>>
{
    
}