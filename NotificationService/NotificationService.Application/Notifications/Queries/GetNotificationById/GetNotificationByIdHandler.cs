using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NotificationService.Application.Interfaces;
using Warehouse.Notifications.Domain.Entities;

namespace NotificationService.Application.Notifications.Queries.GetNotificationById;

public class GetNotificationByIdHandler : IRequestHandler<GetNotificationByIdQuery, Notification>
{
    private readonly INotificationRepository _notificationRepository;
    public GetNotificationByIdHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Notification> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.GetByIdAsync(request.Id, cancellationToken);
    }
    
}