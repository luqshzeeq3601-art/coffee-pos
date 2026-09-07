namespace CoffeePos.Domain.Inventory;

public enum TransferStatus
{
    Draft = 1,
    Dispatched = 2,
    Received = 3,
    Cancelled = 4
}

public sealed class StockTransferItem
{
    public Guid Id { get; private set; }
    public Guid TransferId { get; private set; }
    public Guid StockItemId { get; private set; }
    public decimal Quantity { get; private set; }

    public StockTransferItem(Guid id, Guid transferId, Guid stockItemId, decimal quantity)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (transferId == Guid.Empty) throw new ArgumentException("TransferId cannot be empty.", nameof(transferId));
        if (stockItemId == Guid.Empty) throw new ArgumentException("StockItemId cannot be empty.", nameof(stockItemId));
        if (quantity <= 0) throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        Id = id;
        TransferId = transferId;
        StockItemId = stockItemId;
        Quantity = quantity;
    }
}

public sealed class StockTransfer
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string TransferNumber { get; private set; }
    public Guid SourceOutletId { get; private set; }
    public Guid DestinationOutletId { get; private set; }
    public TransferStatus Status { get; private set; }
    public List<StockTransferItem> Items { get; private set; } = new();
    public Guid? DispatchedById { get; private set; }
    public string? DispatchedByName { get; private set; }
    public DateTime? DispatchedAtUtc { get; private set; }
    public Guid? ReceivedById { get; private set; }
    public string? ReceivedByName { get; private set; }
    public DateTime? ReceivedAtUtc { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public StockTransfer(
        Guid id,
        Guid tenantId,
        string transferNumber,
        Guid sourceOutletId,
        Guid destinationOutletId,
        string? notes = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("ID cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(transferNumber)) throw new ArgumentException("Transfer number is required.", nameof(transferNumber));
        if (sourceOutletId == destinationOutletId) throw new InvalidOperationException("Source and destination outlets cannot be identical.");

        Id = id;
        TenantId = tenantId;
        TransferNumber = transferNumber.Trim();
        SourceOutletId = sourceOutletId;
        DestinationOutletId = destinationOutletId;
        Notes = notes?.Trim();
        Status = TransferStatus.Draft;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void AddItem(Guid stockItemId, decimal quantity)
    {
        if (Status != TransferStatus.Draft)
            throw new InvalidOperationException("Cannot add items to a non-draft transfer.");

        Items.Add(new StockTransferItem(Guid.NewGuid(), Id, stockItemId, quantity));
    }

    public void Dispatch(Guid dispatchedById, string dispatchedByName, string? notes = null)
    {
        if (Status != TransferStatus.Draft)
            throw new InvalidOperationException("Only draft transfers can be dispatched.");
        if (Items.Count == 0)
            throw new InvalidOperationException("Cannot dispatch a transfer with 0 items.");

        Status = TransferStatus.Dispatched;
        DispatchedById = dispatchedById;
        DispatchedByName = dispatchedByName;
        DispatchedAtUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(notes)) Notes = notes.Trim();
    }

    public void Receive(Guid receivedById, string receivedByName, string? notes = null)
    {
        if (Status != TransferStatus.Dispatched)
            throw new InvalidOperationException("Only dispatched transfers can be received.");

        Status = TransferStatus.Received;
        ReceivedById = receivedById;
        ReceivedByName = receivedByName;
        ReceivedAtUtc = DateTime.UtcNow;
        if (!string.IsNullOrWhiteSpace(notes)) Notes = notes.Trim();
    }
}
