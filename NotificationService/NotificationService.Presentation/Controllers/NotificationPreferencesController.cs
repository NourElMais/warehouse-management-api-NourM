using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.NotificationPreference.Commands.UpdateNotificationPreferenceCommand;
using NotificationService.Application.NotificationPreference.GetAllNotificationPreferences;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationPreferencesController : ControllerBase
{
    private readonly IMediator _mediator;

    public NotificationPreferencesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var preferences = await _mediator.Send(new GetAllNotificationsPreferencesQuery(), cancellationToken);

        return Ok(preferences);
    }

    [HttpPut("{notificationType}")]
    public async Task<IActionResult> Update(string notificationType, UpdateNotificationPreferenceRequest request, CancellationToken cancellationToken)
    {
        var upd = new UpdateNotificationPreferenceCommand(notificationType, request.Severity);

        var updatedPreference = await _mediator.Send(upd, cancellationToken);

        return Ok(updatedPreference);
    }
}