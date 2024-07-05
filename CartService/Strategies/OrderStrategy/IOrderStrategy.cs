using CartService.Models;
using CartService.Models.Enums;

namespace CartService.Strategies.OrderStrategy;

public interface IOrderStrategy
{
    Task SubmitOrder(IEnumerable<CartItem> items);
}
