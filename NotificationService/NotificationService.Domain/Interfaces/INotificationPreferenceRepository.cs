using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Interfaces;

public interface INotificationPreferenceRepository
{
    Task<NotificationPreference?> GetByTypeAsync(string notificationType, CancellationToken cancellationToken);

    Task<List<NotificationPreference>> GetAllAsync(CancellationToken cancellationToken);

    Task UpdateAsync(NotificationPreference preference, CancellationToken cancellationToken);
}