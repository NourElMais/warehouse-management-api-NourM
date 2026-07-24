using Microsoft.EntityFrameworkCore;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Infrastructure.Data;

public class NotificationDbContext:DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
}