using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Infrastructure.Data;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Infrastructure.Repositories;

public class NotificationPreferenceRepository : INotificationPreferenceRepository
    {
        private readonly NotificationDbContext _dbContext;

        public NotificationPreferenceRepository(NotificationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NotificationPreference?> GetByTypeAsync(string notificationType, CancellationToken cancellationToken)
        {
            return await _dbContext.NotificationPreferences.FirstOrDefaultAsync(preference => preference.NotificationType.ToLower() == notificationType.ToLower(), cancellationToken);
        }

        public async Task<List<NotificationPreference>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.NotificationPreferences.ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(NotificationPreference preference, CancellationToken cancellationToken)
        {
            _dbContext.NotificationPreferences.Update(preference);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
