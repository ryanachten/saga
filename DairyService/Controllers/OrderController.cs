using DairyService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DairyService.Controllers;

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
    public void Post(List<DairyItem> items)
    {
        foreach (DairyItem item in items)
        {
            _logger.LogInformation("Adding dairy item: {item.Name} count:{item.Count}", item.Name, item.Count);
        }
    }

    [HttpDelete]
    public void Delete(List<DairyItem> items)
    {
        foreach (DairyItem item in items)
        {
            _logger.LogWarning("Deleting dairy item: {item.Name} count:{item.Count}", item.Name, item.Count);
        }
    }
}
