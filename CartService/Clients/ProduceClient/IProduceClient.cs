
namespace CartService.Clients.ProduceClient;

public interface IProduceClient
{
    Task SaveOrder(IEnumerable<ProduceItem> items, Func<Task>? fallback = null);

    Task DeleteOrder(IEnumerable<ProduceItem> items);

}
