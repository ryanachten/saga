using OrderService.Models;
using OrderService.Models.Enums;

namespace OrderService.Strategies.OrderStrategy;

public interface IOrderStrategy
{
    Task SubmitOrder(IEnumerable<OrderItem> items);
}
