
namespace CartService.Clients.DairyClient;

public interface IDairyClient
{
    Task SaveOrder(IEnumerable<DairyItem> items);

    Task DeleteOrder(IEnumerable<DairyItem> items);
}
