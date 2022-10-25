namespace Producer.Models;

public class ReturnOrder : Order
{
    public int CookingTime { get; set; }
    protected ReturnOrder(Order thisOrder, int cookingTime) : base(thisOrder)
    {
        CookingTime = cookingTime;
    }

    public ReturnOrder() : base()
    {

    }
}