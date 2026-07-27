using System;
using MediatR;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Commands.MarkNotifAsRead;

public class MarkNotifAsReadCommand : IRequest<Notification>
{
    public Guid Id { get; }

    public MarkNotifAsReadCommand(Guid id)
    {
        Id = id;
    }
    
}