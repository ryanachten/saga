
namespace CartService.Clients.NotificationClient;

public interface INotificationClient
{
    Task PushOrderNotification(Guid id);
}
