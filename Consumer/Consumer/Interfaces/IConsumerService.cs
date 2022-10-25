using Consumer.Models;

namespace Consumer.Interfaces;

public interface IConsumerService
{
    Task ReceiveAggregatorOrder(Order order);
}