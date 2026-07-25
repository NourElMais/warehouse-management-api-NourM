using MediatR;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.NotificationPreference.GetAllNotificationPreferences;

public class GetAllNotificationsPreferencesHandler : IRequestHandler<GetAllNotificationsPreferencesQuery,
    List<Warehouse.Notifications.Domain.Entities.NotificationPreference>>
{
    private readonly INotificationPreferenceRepository _repository;

    public GetAllNotificationsPreferencesHandler(INotificationPreferenceRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Warehouse.Notifications.Domain.Entities.NotificationPreference>> Handle(GetAllNotificationsPreferencesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }  
}

