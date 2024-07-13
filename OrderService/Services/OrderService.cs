using Common.Contracts;
using OrderService.Models.Enums;
using OrderService.Strategies.OrderStrategy;

namespace OrderService.Services;

public class OrderService(
    ISimpleOrderStrategy simpleOrderStrategy,
    IOrchestratedOrderStrategy orchestratedOrderStrategy,
    IEventOrderStrategy eventOrderStrategy
) : IOrderService
{
    public Task SubmitOrder(Order order, OrderStrategy? strategy = OrderStrategy.SIMPLE)
    {
        return strategy switch
        {
            OrderStrategy.ORCHESTRATED => orchestratedOrderStrategy.SubmitOrder(order),
            OrderStrategy.EVENT => eventOrderStrategy.SubmitOrder(order),
            _ => simpleOrderStrategy.SubmitOrder(order),
        };
    }
}
