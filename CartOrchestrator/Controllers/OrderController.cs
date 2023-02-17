using CartOrchestrator.Clients.DairyClient;
using CartOrchestrator.Clients.ProduceClient;
using CartOrchestrator.Models;
using CartOrchestrator.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace SagaDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IDairyClient _dairyClient;
    private readonly IProduceClient _produceClient;

    public OrderController(IDairyClient dairyClient, IProduceClient produceClient)
    {
        _dairyClient = dairyClient;
        _produceClient = produceClient;
    }

    [HttpPost]
    public async Task Post(List<CartItem> items)
    {
        var dairyItems = items.Where(x => x.Type == ItemType.DAIRY.ToString()).Select(x => new DairyItem()
        {
            Name = x.Name,
            Count = x.Count
        });
        await _dairyClient.SaveOrder(dairyItems);

        var produceItems = items.Where(x => x.Type == ItemType.PRODUCE.ToString()).Select(x => new ProduceItem()
        {
            Name = x.Name,
            Count = x.Count
        });
        await _produceClient.SaveOrder(produceItems);
    }
}
