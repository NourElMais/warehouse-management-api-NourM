using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Notifications.Commands.MarkNotifAsRead;
using NotificationService.Application.Notifications.Queries.GetAllNotifications;
using NotificationService.Application.Notifications.Queries.GetNotificationById;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Presentation.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNotif([FromQuery] string?  type, [FromQuery] NotificationSeverity? severity,[FromQuery] string? status, CancellationToken cancellationToken)
    {
        var notif = await _mediator.Send(new GetAllNotificationsQuery(type,severity,status,cancellationToken), cancellationToken);
        return Ok(notif);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetNotificationById(string id, CancellationToken cancellationToken)
    {

        if (!Guid.TryParse(id, out var guid))
            return BadRequest("Invalid notification id");

        var notif = await _mediator.Send(new GetNotificationByIdQuery(guid), cancellationToken);
        
        if (notif is null)
        {
            return NotFound("The notification with the specified Id was not found");
        }

        return Ok(notif);
    }
    
    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(string id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out var guid))
            return BadRequest("Invalid notification id");
        
        var notification = await _mediator.Send(new MarkNotifAsReadCommand(guid), cancellationToken);

        if (notification is null)
        {
            return NotFound("The notification with the specified Id was not found.");
        }

        return Ok(notification);
    }
}
