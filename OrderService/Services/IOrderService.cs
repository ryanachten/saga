using OrderService.Models;
using OrderService.Models.Enums;

namespace OrderService.Services;

public interface IOrderService
{
    Task SubmitOrder(Order order, OrderStrategy? strategy = OrderStrategy.SIMPLE);
}