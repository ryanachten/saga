using OrderService.Models;
using OrderService.Models.Enums;

namespace OrderService.Services;

public interface IOrderService
{
    Task SubmitOrder(IEnumerable<OrderItem> items, OrderStrategy? strategy = OrderStrategy.SIMPLE);
}