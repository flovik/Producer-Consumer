using Consumer.Interfaces;
using Consumer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Consumer.Controllers;

[ApiController]
[Route("api")]
public class ConsumerController : ControllerBase
{
    private readonly ILogger<ConsumerController> _logger;
    private readonly IConsumerService _consumerService;

    public ConsumerController(ILogger<ConsumerController> logger, IConsumerService consumerService)
    {
        _logger = logger;
        _consumerService = consumerService;
    }

    [HttpPost("consumer")]
    public async Task Order([FromBody] Order order)
    {
        _logger.LogInformation($"Received from aggregator order: {order.OrderId}");
        await _consumerService.ReceiveAggregatorOrder(order);
    }
}