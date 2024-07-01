using CartService.Clients.DairyClient;
using CartService.Clients.DeliveryClient;
using CartService.Clients.ProduceClient;
using CartService.Models;
using CartService.Models.Enums;

namespace CartService.Strategies;

public abstract class BaseOrderStrategy
{
    protected static IEnumerable<DairyItem> GetDairyItems(IEnumerable<CartItem> items)
    => items.Where(x => x.Type == ItemType.DAIRY).Select(x => new DairyItem()
    {
        Name = x.Name,
        Count = x.Count
    });

    protected static IEnumerable<ProduceItem> GetProduceItems(IEnumerable<CartItem> items)
        => items.Where(x => x.Type == ItemType.PRODUCE).Select(x => new ProduceItem()
        {
            Name = x.Name,
            Count = x.Count
        });

    protected static DeliveryOrder GetDeliveryOrder(IEnumerable<CartItem> items)
        => new()
        {
            Items = items.Select(x => new DeliveryItem() { Name = x.Name, Count = x.Count })
        };
}
