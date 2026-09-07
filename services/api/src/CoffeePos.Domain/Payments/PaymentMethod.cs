namespace CoffeePos.Domain.Payments;

public enum PaymentMethod
{
    Cash = 1,
    DuitNowQR = 2,
    CreditCard = 3,
    DebitCard = 4,
    Custom = 5
}

public enum PaymentStatus
{
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Refunded = 4
}
