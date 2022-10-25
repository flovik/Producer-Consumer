using Producer.Models;

namespace Producer.Services;

public class OrderCreator
{
    private static int Id = 0;
    private Random random = new();

    public Order CreateOrder()
    {
        var order = new Order
        {
            OrderId = Id++,
            WaiterId = random.Next(0, 10),
            MaxWait = random.Next(1, 51),
            Priority = random.Next(1, 6),
            TableId = random.Next(1, 11)
        };

        return order;
    }
}