
namespace CartOrchestrator.Clients.ProduceClient;

public interface IProduceClient
{
    Task SaveOrder(IEnumerable<ProduceItem> items);

    Task DeleteOrder(IEnumerable<ProduceItem> items);

}
