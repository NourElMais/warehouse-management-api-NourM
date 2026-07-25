using Microsoft.EntityFrameworkCore;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Infrastructure.Data;

public class NotificationDbContext:DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationPreference> NotificationPreferences { get; set; }
    
    //Rule to say that there should be one preference per type.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationPreference>().HasIndex(preference => preference.NotificationType).IsUnique();
        modelBuilder.Entity<Notification>().Property(n => n.Severity).HasConversion<string>();
    }
}