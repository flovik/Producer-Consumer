using Consumer.Interfaces;
using Consumer.Models;

namespace Consumer.Services;

public class ConsumerService : IConsumerService
{
    private readonly Queue<Order> _aggregatorOrders = new(10);
    private readonly ReturnOrderCreator _returnOrderCreator = new();
    private readonly ILogger<ConsumerService> _logger;
    private static readonly HttpClient HttpClient = new();
    private readonly Mutex _aggregatorMutex = new();
    private readonly SemaphoreSlim _aggregatorSemaphore = new(10, 10);

    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
        HttpClient.BaseAddress = new Uri("http://localhost:5043/");
        for (int i = 0; i < 6; i++)
        {
            Task.Run(ConsumeData);
            Thread.Sleep(1000);
        }
    }

    public async Task ReceiveAggregatorOrder(Order order)
    {
        await _aggregatorSemaphore.WaitAsync();
        _aggregatorMutex.WaitOne();
        _aggregatorOrders.Enqueue(order);
        _aggregatorMutex.ReleaseMutex();
        _aggregatorSemaphore.Release();
    }

    public async Task ConsumeData()
    {
        while (true)
        {
            await _aggregatorSemaphore.WaitAsync();
            _aggregatorMutex.WaitOne();
            if (_aggregatorOrders.Count != 0)
            {
                Thread.Sleep(5000);
                var order = _aggregatorOrders.Dequeue();
                var returnOrder = _returnOrderCreator.CreateReturnOrder(order);
                _logger.LogInformation($"Sending aggregator order with Id: {returnOrder.OrderId}");
                HttpClient.PostAsJsonAsync("api/aggregator/returnOrder", returnOrder);
            }
            _aggregatorMutex.ReleaseMutex();
            _aggregatorSemaphore.Release();
        }
    }
}