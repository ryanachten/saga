using CartService.Models;
using CartService.Models.Enums;
using CartService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckoutController(IOrderService orderService) : ControllerBase
{

    [HttpPost]
    public async Task CheckoutSimple(List<CartItem> items, [FromQuery] OrderStrategy? strategy)
    {
        await orderService.SubmitOrder(items, strategy);
    }
}
