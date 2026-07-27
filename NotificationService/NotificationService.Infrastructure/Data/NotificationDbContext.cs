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
        modelBuilder.Entity<NotificationPreference>().Property(p => p.Severity).HasConversion<string>();
        modelBuilder.Entity<Notification>().Property(n => n.Severity).HasConversion<string>();
        modelBuilder.Entity<NotificationPreference>().HasData(
            new NotificationPreference
            {
                Id = Guid.Parse("8bc1eebf-f71d-46ec-b5b5-fd86553d6efe"),
                NotificationType = "LowStock",
                Severity = NotificationSeverity.Warning
            },
            new NotificationPreference
            {
                Id = Guid.Parse("6f71ffff-9acc-4c7b-8553-5e82664864a0"),
                NotificationType = "FileUploaded",
                Severity = NotificationSeverity.Information
            },
            new NotificationPreference
            {
                Id = Guid.Parse("10203a03-79bb-4d68-925d-ce6114ea0fad"),
                NotificationType = "ProductCreated",
                Severity = NotificationSeverity.Information
            }
        );
    }
}