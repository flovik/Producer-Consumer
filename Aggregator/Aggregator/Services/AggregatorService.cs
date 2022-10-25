using Aggregator.Interfaces;
using Aggregator.Models;

namespace Aggregator.Services;

public class AggregatorService : IAggregatorService
{
    private readonly Queue<Order> _producerOrders = new(10);
    private readonly Queue<ReturnOrder> _consumerOrders = new(10);
    private readonly SemaphoreSlim _producerSemaphore = new(10, 10);
    private readonly SemaphoreSlim _consumerSemaphore = new(10, 10);
    private readonly ILogger<AggregatorService> _logger;
    private static readonly HttpClient HttpClient = new();
    private readonly Mutex _producerMutex = new();
    private readonly Mutex _consumerMutex = new();

    public AggregatorService(ILogger<AggregatorService> logger)
    {
        _logger = logger;
        for (int i = 0; i < 6; i++)
        {
            Task.Run(ProcessProducerOrders);
            Thread.Sleep(1000);
        }

        for (int i = 0; i < 6; i++)
        {
            Task.Run(ProcessConsumerOrders);
            Thread.Sleep(1000);
        }
    }

    public async Task ReceiveProducerOrder(Order order)
    {
        //waits for one empty slot, decrements the semaphore to take a slot
        //producer inserts data in one of those slots
        await _producerSemaphore.WaitAsync();
        //acquire lock on buffer so consumer cannot access until producer completes its operation
        _producerMutex.WaitOne();
        _producerOrders.Enqueue(order);
        //after insertion, lock is released and value of full is incremented because producer has just filled a slot
        _producerMutex.ReleaseMutex();
        _producerSemaphore.Release();
    }

    public async Task ReceiveConsumerOrder(ReturnOrder order)
    {
        await _consumerSemaphore.WaitAsync();
        _consumerMutex.WaitOne();
        _consumerOrders.Enqueue(order);
        _consumerMutex.ReleaseMutex();
        _consumerSemaphore.Release();
    }

    public async Task ProcessProducerOrders()
    {
        while (true)
        {
            await _producerSemaphore.WaitAsync();
            _producerMutex.WaitOne();
            if (_producerOrders.Count != 0)
            {
                //wait 5 secs for dequeuing
                Thread.Sleep(5000);
                var order = _producerOrders.Dequeue();
                _logger.LogInformation($"Sending producer order with Id: {order.OrderId}");
                HttpClient.PostAsJsonAsync("http://localhost:5044/api/consumer", order);
            }
            _producerMutex.ReleaseMutex();
            _producerSemaphore.Release();
        }
    }

    public async Task ProcessConsumerOrders()
    {
        while (true)
        {
            await _consumerSemaphore.WaitAsync();
            _consumerMutex.WaitOne();
            if (_consumerOrders.Count != 0)
            {
                //wait 5 secs for dequeuing
                Thread.Sleep(5000);
                var order = _consumerOrders.Dequeue();
                _logger.LogInformation($"Sending consumer order with Id: {order.OrderId}");
                //no await, thread is updated and release mutex causes error
                HttpClient.PostAsJsonAsync("http://localhost:5042/api/producer", order);
            }
            _consumerMutex.ReleaseMutex();
            _consumerSemaphore.Release();
        }
    }
}