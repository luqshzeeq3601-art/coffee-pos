namespace CoffeePos.Domain.MyInvois;

public enum MyInvoisStatus
{
    Draft = 1,
    Submitted = 2,
    Valid = 3,
    Invalid = 4,
    Cancelled = 5
}

public enum BuyerIdType
{
    NRIC = 1,
    BRN = 2,
    PASSPORT = 3,
    ARMY = 4
}
