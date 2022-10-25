using Aggregator.Interfaces;
using Aggregator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aggregator.Controllers;

[ApiController]
[Route("api")]
public class AggregatorController : ControllerBase
{
    private readonly ILogger<AggregatorController> _logger;
    private readonly IAggregatorService _aggregatorService;

    public AggregatorController(ILogger<AggregatorController> logger, IAggregatorService aggregatorService)
    {
        _logger = logger;
        _aggregatorService = aggregatorService;
    }

    [HttpPost("aggregator/order")]
    public async Task Order([FromBody] Order order)
    {
        _logger.LogInformation($"Received from producer order: {order.OrderId}");
        await _aggregatorService.ReceiveProducerOrder(order);
    }

    [HttpPost("aggregator/returnOrder")]
    public async Task Order([FromBody] ReturnOrder order)
    {
        _logger.LogInformation($"Received from consumer order: {order.OrderId}");
        await _aggregatorService.ReceiveConsumerOrder(order);
    }
}