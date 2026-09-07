namespace CoffeePos.Domain.Inventory;

public enum UnitOfMeasure
{
    Grams = 1,
    Milliliters = 2,
    Pieces = 3,
    Kilograms = 4,
    Liters = 5
}

public enum StockMovementType
{
    SaleDepletion = 1,
    ReceiveStock = 2,
    ManualAdjustment = 3,
    WasteWritedown = 4,
    Transfer = 5
}
