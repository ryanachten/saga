
namespace CartService.Clients.DeliveryClient;

public interface IDeliveryClient
{
    Task DeleteOrder(Guid id);
    Task<Guid> SaveOrder(DeliveryOrder order, Func<Task>? fallback = null);
}
