using Common.Contracts;
using Common.Contracts.Enums;
using OrderService.Clients.DairyClient;
using OrderService.Clients.DeliveryClient;
using OrderService.Clients.ProduceClient;

namespace OrderService.Strategies.OrderStrategy;

public abstract class BaseOrderStrategy
{
    protected static IEnumerable<DairyItem> GetDairyItems(IEnumerable<OrderItem> items)
    => items.Where(x => x.Type == ItemType.DAIRY).Select(x => new DairyItem()
    {
        Name = x.Name,
        Count = x.Count
    });

    protected static IEnumerable<ProduceItem> GetProduceItems(IEnumerable<OrderItem> items)
        => items.Where(x => x.Type == ItemType.PRODUCE).Select(x => new ProduceItem()
        {
            Name = x.Name,
            Count = x.Count
        });

    protected static DeliveryOrder GetDeliveryOrder(IEnumerable<OrderItem> items)
        => new()
        {
            Items = items.Select(x => new DeliveryItem() { Name = x.Name, Count = x.Count })
        };
}
