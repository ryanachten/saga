using DairyService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DairyService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController(ILogger<OrderController> logger) : ControllerBase
{
    [HttpGet("{name}")]
    public ActionResult Get(string name)
    {
        if (Stock.Items.TryGetValue(name, out int value))
        {
            return Ok(new { Item = name, Stock = value });
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
            logger.LogInformation("Dairy order fulfilled. Stock now: {Name} count: {Stock}", item.Name, Stock.Items[item.Name]);
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
            logger.LogInformation("Dairy order removed. Stock now: {Name} count: {Stock}", item.Name, Stock.Items[item.Name]);
        }
    }
}
