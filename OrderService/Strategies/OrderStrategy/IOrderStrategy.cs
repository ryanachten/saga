using Common.Contracts;

namespace OrderService.Strategies.OrderStrategy;

public interface IOrderStrategy
{
    Task SubmitOrder(Order order);
}
