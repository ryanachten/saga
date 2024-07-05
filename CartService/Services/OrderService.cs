using CartService.Models;
using CartService.Models.Enums;
using CartService.Strategies.OrderStrategy;

namespace CartService.Services;

public class OrderService(
    ISimpleOrderStrategy simpleOrderStrategy,
    IOrchestratedOrderStrategy orchestratedOrderStrategy,
    IEventOrderStrategy eventOrderStrategy
) : IOrderService
{
    public Task SubmitOrder(IEnumerable<CartItem> items, OrderStrategy? strategy = OrderStrategy.SIMPLE)
    {
        return strategy switch
        {
            OrderStrategy.ORCHESTRATED => orchestratedOrderStrategy.SubmitOrder(items),
            OrderStrategy.EVENT => eventOrderStrategy.SubmitOrder(items),
            _ => simpleOrderStrategy.SubmitOrder(items),
        };
    }
}
