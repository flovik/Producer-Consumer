using Microsoft.AspNetCore.Mvc;
using Producer.Interfaces;
using Producer.Models;

namespace Producer.Controllers;

[ApiController]
[Route("api")]
public class ProducerController : ControllerBase
{
    private readonly ILogger<ProducerController> _logger;

    public ProducerController(ILogger<ProducerController> logger)
    {
        _logger = logger;
    }

    [HttpPost("producer")]
    public void ReturnOrder([FromBody] ReturnOrder returnOrder)
    {
        _logger.LogInformation($"Received order {returnOrder.OrderId} from aggregator");
    }
}