namespace CoffeePos.Domain.Tenancy;

public sealed class Tenant
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string CurrencyCode { get; private set; }
    public string TimeZone { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public Tenant(Guid id, string name, string currencyCode = "MYR", string timeZone = "Asia/Kuala_Lumpur")
    {
        if (id == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Tenant name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(currencyCode)) throw new ArgumentException("Currency code cannot be empty.", nameof(currencyCode));

        Id = id;
        Name = name.Trim();
        CurrencyCode = currencyCode.Trim().ToUpperInvariant();
        TimeZone = timeZone.Trim();
        IsActive = true;
        CreatedAtUtc = DateTime.UtcNow;
    }
}
