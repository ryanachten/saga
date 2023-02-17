
namespace CartOrchestrator.Clients.DairyClient
{
    public interface IDairyClient
    {
        Task SaveOrder(IEnumerable<DairyItem> items);
    }
}