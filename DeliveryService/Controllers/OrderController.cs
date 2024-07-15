using DeliveryService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(ILogger<OrderController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Ok(Orders.Items);
    }

    [HttpPost]
    public ActionResult Post(Order order)
    {
        Orders.Items.Add(order);

        logger.LogInformation("Dispatching order delivery: {Id}", order.Id);

        return Ok(order.Id);
    }

    [HttpDelete("{id}")]
    public void Delete(Guid id)
    {
        logger.LogWarning("Deleting order delivery: {Id}", id);

        Orders.Items.RemoveAll(x => x.Id == id);
    }
}
