using CartOrchestrator.Clients.DairyClient;
using CartOrchestrator.Clients.ProduceClient;
using CartOrchestrator.Models;
using CartOrchestrator.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CartOrchestrator.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IDairyClient _dairyClient;
    private readonly IProduceClient _produceClient;

    public OrderController(
        ILogger<OrderController> logger,
        IDairyClient dairyClient,
        IProduceClient produceClient
    )
    {
        _logger = logger;
        _dairyClient = dairyClient;
        _produceClient = produceClient;

    }

    [HttpPost]
    public async Task OrderSimple(List<CartItem> items)
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

    [HttpPost("orchestrated")]
    public async Task OrderOrchestrated(List<CartItem> items)
    {
        var dairyItems = items.Where(x => x.Type == ItemType.DAIRY.ToString()).Select(x => new DairyItem()
        {
            Name = x.Name,
            Count = x.Count
        });

        // We don't need to perform any compensating transactions in this failure case
        // given there are no prior transactions to rollback
        await _dairyClient.SaveOrder(dairyItems);

        try
        {
            var produceItems = items.Where(x => x.Type == ItemType.PRODUCE.ToString()).Select(x => new ProduceItem()
            {
                Name = x.Name,
                Count = x.Count
            });
            await _produceClient.SaveOrder(produceItems);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error submitting produce: {ex.Message}", ex.Message);

            // In the case of an error scenario
            // we need to use compensating transactions to rollback prior transactions
            _logger.LogInformation("Rolling back dairy orders");
            await _dairyClient.DeleteOrder(dairyItems);

            // Once finished rolling back, we throw the exeception
            // to ensure the error isn't swallowed
            throw;
        }
    }
}
