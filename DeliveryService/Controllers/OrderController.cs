using DeliveryService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok(Orders.Items);
    }

    [HttpPost]
    public ActionResult Post(Order order)
    {
        Orders.Items.Add(order);

        _logger.LogInformation("Dispatching order: {order.Id}", order.Id);

        return Ok(order.Id);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        _logger.LogWarning("Deleting order: {id}", id);

        Orders.Items.RemoveAll(x => x.Id == id);
    }
}
