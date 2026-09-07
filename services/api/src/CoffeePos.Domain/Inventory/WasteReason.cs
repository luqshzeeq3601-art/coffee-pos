namespace CoffeePos.Domain.Inventory;

public enum WasteReason
{
    Spillage = 1,
    Expired = 2,
    CalibrationDialIn = 3,
    QualityDefect = 4,
    StaffTraining = 5
}

public enum PurchaseOrderStatus
{
    Draft = 1,
    Ordered = 2,
    Received = 3,
    Cancelled = 4
}
