namespace CoffeePos.Domain.Kitchen;

public enum PrepStation
{
    All = 0,
    EspressoBar = 1,
    FilterBar = 2,
    PastryKitchen = 3
}

public enum ChitStatus
{
    Queued = 1,
    Preparing = 2,
    Ready = 3,
    Completed = 4,
    Recalled = 5
}
