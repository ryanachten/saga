using Microsoft.AspNetCore.Mvc;
using ProduceService.Models;

namespace ProduceService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public void Post(List<ProduceItem> items)
    {
        foreach (ProduceItem item in items)
        {
            _logger.LogInformation("Adding produce item: {item.Name} count:{item.Count}", item.Name, item.Count);
        }
    }

    [HttpDelete]
    public void Delete(List<ProduceItem> items)
    {
        foreach (ProduceItem item in items)
        {
            _logger.LogWarning("Deleting produce item: {item.Name} count:{item.Count}", item.Name, item.Count);
        }
    }
}
