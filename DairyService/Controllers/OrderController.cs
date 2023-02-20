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


    [HttpGet("{name}")]
    public ActionResult Get(string name)
    {
        if (Stock.Items.ContainsKey(name))
        {
            return Ok(new { Item = name, Stock = Stock.Items[name] });
        }

        return NotFound();
    }

    [HttpPost]
    public void Post(List<DairyItem> items)
    {
        foreach (DairyItem item in items)
        {
            if (Stock.Items.ContainsKey(item.Name))
            {
                Stock.Items[item.Name] -= item.Count;
            }
            _logger.LogInformation("Added dairy order. Stock now: {item.Name} count: {Stock.Items[item.Name]}", item.Name, Stock.Items[item.Name]);
        }
    }

    [HttpDelete]
    public void Delete(List<DairyItem> items)
    {
        foreach (DairyItem item in items)
        {
            if (Stock.Items.ContainsKey(item.Name))
            {
                Stock.Items[item.Name] += item.Count;
            }
            _logger.LogInformation("Deleted dairy order. Stock now: {item.Name} count: {Stock.Items[item.Name]}", item.Name, Stock.Items[item.Name]);
        }
    }
}
