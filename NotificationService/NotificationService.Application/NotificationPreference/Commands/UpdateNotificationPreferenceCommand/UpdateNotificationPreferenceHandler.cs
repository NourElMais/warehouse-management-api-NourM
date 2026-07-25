using MediatR;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.NotificationPreference.Commands.UpdateNotificationPreferenceCommand;

public class UpdateNotificationPreferenceHandler : IRequestHandler<UpdateNotificationPreferenceCommand, Warehouse.Notifications.Domain.Entities.NotificationPreference>
{
    private readonly INotificationPreferenceRepository _repository;

    public UpdateNotificationPreferenceHandler(INotificationPreferenceRepository repository)
    {
        _repository = repository;
    }

    public async Task<Warehouse.Notifications.Domain.Entities.NotificationPreference> Handle(UpdateNotificationPreferenceCommand request, CancellationToken cancellationToken)
    {
        var preference = await _repository.GetByTypeAsync(request.NotificationType, cancellationToken);

        if (preference is null)
        {
            throw new Exception($"Notification preference '{request.NotificationType}' was not found.");
        }

        preference.Severity = request.Severity;

        await _repository.UpdateAsync(preference, cancellationToken);

        return preference;
    }
}