using OrderService.Models;
using OrderService.Models.Enums;
using OrderService.Strategies.OrderStrategy;

namespace OrderService.Services;

public class OrderService(
    ISimpleOrderStrategy simpleOrderStrategy,
    IOrchestratedOrderStrategy orchestratedOrderStrategy,
    IEventOrderStrategy eventOrderStrategy
) : IOrderService
{
    public Task SubmitOrder(IEnumerable<OrderItem> items, OrderStrategy? strategy = OrderStrategy.SIMPLE)
    {
        return strategy switch
        {
            OrderStrategy.ORCHESTRATED => orchestratedOrderStrategy.SubmitOrder(items),
            OrderStrategy.EVENT => eventOrderStrategy.SubmitOrder(items),
            _ => simpleOrderStrategy.SubmitOrder(items),
        };
    }
}
