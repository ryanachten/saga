using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    [HttpPost("{orderId}")]
    public void Post(Guid orderId)
    {
        _logger.LogInformation("Order placed, view it at http://delivery/{orderId}", orderId);
    }
}
