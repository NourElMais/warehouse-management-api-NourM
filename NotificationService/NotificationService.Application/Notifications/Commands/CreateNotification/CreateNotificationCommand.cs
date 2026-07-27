using MediatR;
using NotificationService.Presentation;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Commands.CreateNotification;

public class CreateNotificationCommand:IRequest<Notification>
{
   public CreateNotificationRequest Request { get; }
   public CreateNotificationCommand(CreateNotificationRequest request)
   {
      Request = request;
   }

}