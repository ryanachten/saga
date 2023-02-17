using DairyService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DairyService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    [HttpPost]
    public void Post(List<DairyItem> items)
    {
        foreach (DairyItem item in items)
        {
            Console.WriteLine($"Dairy item: {item.Name} count:{item.Count}");
        }
    }
}
