using System.Collections.Generic;
using MediatR;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Queries.GetAllNotifications;

public class GetAllNotificationsQuery : IRequest<List<Notification>>
{
    public string? Type { get; }
    public string?Severity { get; }
    public string? Status { get; }
    public GetAllNotificationsQuery(string? type, string? severity, string? status, CancellationToken cancellationToken)
    {
        Type = type;
        Severity = severity;
        Status = status;
    }

}