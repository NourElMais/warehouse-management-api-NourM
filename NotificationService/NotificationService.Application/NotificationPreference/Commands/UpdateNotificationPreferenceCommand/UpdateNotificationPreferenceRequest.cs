using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.NotificationPreference.Commands.UpdateNotificationPreferenceCommand;

    public class UpdateNotificationPreferenceRequest
    {
        public NotificationSeverity Severity { get; set; }
    }
