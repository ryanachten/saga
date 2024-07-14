using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Models.Enums;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{

    [HttpPost]
    public async Task Create(Order order, [FromQuery] OrderStrategy? strategy)
    {
        await orderService.SubmitOrder(order, strategy);
    }
}
