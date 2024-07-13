using Common.Contracts;
using OrderService.Models.Enums;

namespace OrderService.Services;

public interface IOrderService
{
    Task SubmitOrder(Order order, OrderStrategy? strategy = OrderStrategy.SIMPLE);
}