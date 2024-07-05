using CartService.Models;
using CartService.Models.Enums;

namespace CartService.Services;

public interface IOrderService
{
    Task SubmitOrder(IEnumerable<CartItem> items, OrderStrategy? strategy = OrderStrategy.SIMPLE);
}