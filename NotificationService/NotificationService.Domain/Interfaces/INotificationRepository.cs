using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetAllAsync(CancellationToken cancellationToken);

    Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddAsync(Notification notification, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<bool> ExistsByEventIdAsync(Guid eventId, CancellationToken cancellationToken);
}