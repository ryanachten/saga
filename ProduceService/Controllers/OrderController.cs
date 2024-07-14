using Microsoft.AspNetCore.Mvc;
using ProduceService.Models;

namespace ProduceService.Controllers;

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
    public void Post(List<ProduceItem> items)
    {
        foreach (ProduceItem item in items)
        {
            if (Stock.Items.ContainsKey(item.Name))
            {
                Stock.Items[item.Name] -= item.Count;
            }
            logger.LogInformation("Produce order fulfilled. Stock now: {Name} count: {Stock}", item.Name, Stock.Items[item.Name]);
        }
    }

    [HttpDelete]
    public void Delete(List<ProduceItem> items)
    {
        foreach (ProduceItem item in items)
        {
            if (Stock.Items.ContainsKey(item.Name))
            {
                Stock.Items[item.Name] += item.Count;
            }
            logger.LogInformation("Produce order removed. Stock now: {Name} count: {Stock}", item.Name, Stock.Items[item.Name]);
        }
    }
}
