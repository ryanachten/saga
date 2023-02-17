using Microsoft.AspNetCore.Mvc;
using ProduceService.Models;

namespace ProduceService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    [HttpPost]
    public void Post(List<ProduceItem> items)
    {
        foreach (ProduceItem item in items)
        {
            Console.WriteLine($"Produce item: {item.Name} count:{item.Count}");
        }
    }
}
