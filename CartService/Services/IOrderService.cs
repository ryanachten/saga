using CartService.Models;

namespace CartService.Services;

public interface IOrderService
{
    Task SubmitOrchestratedOrder(IEnumerable<CartItem> items);
    Task SubmitOrder(IEnumerable<CartItem> items);
}
