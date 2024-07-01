using CartService.Models;
using CartService.Strategies;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckoutController(
    ISimpleOrderStrategy simpleOrderStrategy, 
    IOrchestratedOrderStrategy orchestratedOrderStrategy
) : ControllerBase
{

    [HttpPost]
    public async Task CheckoutSimple(List<CartItem> items)
    {
        await simpleOrderStrategy.SubmitOrder(items);
    }

    [HttpPost("orchestrated")]
    public async Task CheckoutOrchestrated(List<CartItem> items)
    {
        await orchestratedOrderStrategy.SubmitOrder(items);
    }
}
