using CartService.Models;
using CartService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckoutController : ControllerBase
{
    private readonly IOrderService _orderService;

    public CheckoutController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task CheckoutSimple(List<CartItem> items)
    {
        await _orderService.SubmitOrder(items);
    }

    [HttpPost("orchestrated")]
    public async Task CheckoutOrchestrated(List<CartItem> items)
    {
        await _orderService.SubmitOrchestratedOrder(items);
    }
}
