using CartService.Models;

namespace CartService.Strategies;

public interface IOrderStrategy
{
    Task SubmitOrder(IEnumerable<CartItem> items);
}
