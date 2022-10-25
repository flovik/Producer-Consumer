namespace Consumer.Models;

public class Order
{
    public int OrderId { get; set; }
    public int TableId { get; set; }
    public int WaiterId { get; set; }
    public int Priority { get; set; }
    public int MaxWait { get; set; }
    public long PickUpTime { get; set; } = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

    protected Order(Order thisOrder) : this()
    {
        OrderId = thisOrder.OrderId;
        TableId = thisOrder.TableId;
        WaiterId = thisOrder.WaiterId;
        Priority = thisOrder.Priority;
        MaxWait = thisOrder.MaxWait;
        PickUpTime = thisOrder.PickUpTime;
    }

    public Order()
    {
    }
}