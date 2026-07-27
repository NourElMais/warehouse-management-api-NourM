using MediatR;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.NotificationPreference.Commands.UpdateNotificationPreferenceCommand;

    public class UpdateNotificationPreferenceCommand : IRequest<Warehouse.Notifications.Domain.Entities.NotificationPreference>
    {
        public string NotificationType { get; }
        public NotificationSeverity Severity { get; }

        public UpdateNotificationPreferenceCommand(string notificationType, NotificationSeverity severity)
        {
            NotificationType = notificationType;
            Severity = severity;
        }
    }
