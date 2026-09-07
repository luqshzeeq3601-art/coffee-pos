namespace CoffeePos.Domain.Orders;

public enum OrderStatus
{
    Draft = 1,
    Open = 2,
    Paid = 3,
    Cancelled = 4
}

public enum DiningOption
{
    DineIn = 1,
    Takeaway = 2,
    Delivery = 3
}
