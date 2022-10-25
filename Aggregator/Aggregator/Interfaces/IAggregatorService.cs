using Aggregator.Models;

namespace Aggregator.Interfaces;

public interface IAggregatorService
{
    Task ReceiveProducerOrder(Order order);
    Task ReceiveConsumerOrder(ReturnOrder order);
}