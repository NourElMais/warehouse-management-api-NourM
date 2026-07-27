using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Data;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _dbContext;

    public NotificationRepository(NotificationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Notification>> GetAllAsync(string? type, NotificationSeverity? severity, string? status, CancellationToken cancellationToken)
    {
        IQueryable<Notification> query = _dbContext.Notifications;

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(n => n.Type.ToLower() == type.ToLower());
        }

        if (severity.HasValue)
        {
            query = query.Where(n => n.Severity == severity.Value);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(n => n.Status.ToLower() == status.ToLower());
        }

        return await query.ToListAsync(cancellationToken);
    }
    

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Notifications.FirstOrDefaultAsync(notification => notification.Id == id, cancellationToken);
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        await _dbContext.Notifications.AddAsync(notification, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
       await _dbContext.SaveChangesAsync(cancellationToken); 
    }

    public async Task<bool> ExistsByEventIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await _dbContext.Notifications.AnyAsync(notification => notification.EventId == eventId, cancellationToken);
    }
}