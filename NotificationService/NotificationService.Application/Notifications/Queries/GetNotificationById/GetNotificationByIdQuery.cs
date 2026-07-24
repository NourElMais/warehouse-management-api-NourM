using MediatR;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Queries.GetNotificationById;

public class GetNotificationByIdQuery : IRequest<Notification>
{
    public Guid Id { get; }

    public GetNotificationByIdQuery(Guid id)
    {
        Id = id;
    }
}