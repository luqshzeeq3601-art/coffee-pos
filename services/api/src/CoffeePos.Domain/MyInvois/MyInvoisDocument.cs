namespace CoffeePos.Domain.MyInvois;

public sealed class BuyerDetails
{
    public string Tin { get; private set; }
    public BuyerIdType IdType { get; private set; }
    public string IdValue { get; private set; }
    public string Name { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public string? Address { get; private set; }

    public BuyerDetails(
        string tin,
        BuyerIdType idType,
        string idValue,
        string name,
        string? phoneNumber = null,
        string? email = null,
        string? address = null)
    {
        if (string.IsNullOrWhiteSpace(tin)) throw new ArgumentException("TIN is required.", nameof(tin));
        if (string.IsNullOrWhiteSpace(idValue)) throw new ArgumentException("ID value is required.", nameof(idValue));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Buyer name is required.", nameof(name));

        Tin = tin.Trim().ToUpperInvariant();
        IdType = idType;
        IdValue = idValue.Trim();
        Name = name.Trim();
        PhoneNumber = phoneNumber?.Trim();
        Email = email?.Trim().ToLowerInvariant();
        Address = address?.Trim();
    }
}

public sealed class MyInvoisDocument
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid SalesTransactionId { get; private set; }
    public string InvoiceNumber { get; private set; }
    public string? Uuid { get; private set; }
    public string? LongId { get; private set; }
    public MyInvoisStatus Status { get; private set; }
    public BuyerDetails Buyer { get; private set; }
    public decimal TotalExcludingTax { get; private set; }
    public decimal TotalTaxAmount { get; private set; }
    public decimal TotalPayable { get; private set; }
    public string? QrCodeUrl { get; private set; }
    public string? ValidationErrorsJson { get; private set; }
    public DateTime? SubmittedAtUtc { get; private set; }
    public DateTime? ValidatedAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public MyInvoisDocument(
        Guid id,
        Guid tenantId,
        Guid salesTransactionId,
        string invoiceNumber,
        BuyerDetails buyer,
        decimal totalExcludingTax,
        decimal totalTaxAmount,
        decimal totalPayable)
    {
        if (id == Guid.Empty) throw new ArgumentException("ID cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (salesTransactionId == Guid.Empty) throw new ArgumentException("Sales Transaction ID cannot be empty.", nameof(salesTransactionId));
        if (string.IsNullOrWhiteSpace(invoiceNumber)) throw new ArgumentException("Invoice number is required.", nameof(invoiceNumber));

        Id = id;
        TenantId = tenantId;
        SalesTransactionId = salesTransactionId;
        InvoiceNumber = invoiceNumber.Trim();
        Buyer = buyer ?? throw new ArgumentNullException(nameof(buyer));
        TotalExcludingTax = totalExcludingTax;
        TotalTaxAmount = totalTaxAmount;
        TotalPayable = totalPayable;
        Status = MyInvoisStatus.Draft;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void MarkSubmitted(string uuid, string longId, string qrCodeUrl)
    {
        Uuid = uuid;
        LongId = longId;
        QrCodeUrl = qrCodeUrl;
        Status = MyInvoisStatus.Submitted;
        SubmittedAtUtc = DateTime.UtcNow;
    }

    public void MarkValidated()
    {
        Status = MyInvoisStatus.Valid;
        ValidatedAtUtc = DateTime.UtcNow;
    }

    public void MarkInvalid(string errorsJson)
    {
        Status = MyInvoisStatus.Invalid;
        ValidationErrorsJson = errorsJson;
    }

    public void Cancel(string reason)
    {
        if (Status == MyInvoisStatus.Cancelled)
            throw new InvalidOperationException("Invoice is already cancelled.");

        Status = MyInvoisStatus.Cancelled;
    }
}
