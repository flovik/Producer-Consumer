namespace Consumer.Models;

public class ReturnOrder : Order
{
    public int CookingTime { get; set; }
    public ReturnOrder(Order thisOrder, int cookingTime) : base(thisOrder)
    {
        CookingTime = cookingTime;
    }

    public ReturnOrder() : base()
    {

    }
}