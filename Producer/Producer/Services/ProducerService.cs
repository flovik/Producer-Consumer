using Producer.Interfaces;
using Producer.Models;

namespace Producer.Services;

public class ProducerService : IProducerService
{
    private static readonly HttpClient HttpClient = new();
    private readonly ILogger<ProducerService> _logger;
    private readonly OrderCreator _creator = new();

    public ProducerService(ILogger<ProducerService> logger)
    {
        _logger = logger;
        HttpClient.BaseAddress = new Uri("http://localhost:5043/");

        for (int i = 0; i < 15; i++)
        {
            Task.Run(ProduceData);
            Thread.Sleep(1000);
        }
    }

    public async Task ProduceData()
    {
        while (true)
        {
            var order = _creator.CreateOrder();
            _logger.LogInformation($"Created order with Id: {order.OrderId}");
            //if semaphore doesn't let to continue, stop producing until a new place is given
            await HttpClient.PostAsJsonAsync("api/aggregator/order", order);
        }
    }
}