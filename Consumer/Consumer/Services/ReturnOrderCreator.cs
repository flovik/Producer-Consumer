using Consumer.Models;

namespace Consumer.Services;

public class ReturnOrderCreator
{
    private static readonly Random Random = new();

    public ReturnOrder CreateReturnOrder(Order order)
    {
        var returnOrder = new ReturnOrder(order, Random.Next(10, 51));
        return returnOrder;
    }
}