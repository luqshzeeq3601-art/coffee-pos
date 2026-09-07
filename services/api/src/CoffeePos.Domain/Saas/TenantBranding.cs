namespace CoffeePos.Domain.Saas;

public sealed class TenantBranding
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string BrandName { get; private set; }
    public string? LogoUrl { get; private set; }
    public string PrimaryColorHex { get; private set; }
    public string? HeaderText { get; private set; }
    public string? FooterText { get; private set; }
    public string? TaxRegistrationNumber { get; private set; }
    public bool ShowWifiInfo { get; private set; }
    public string? WifiSsid { get; private set; }
    public string? WifiPassword { get; private set; }

    public TenantBranding(
        Guid id,
        Guid tenantId,
        string brandName,
        string primaryColorHex = "#005D52")
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
        if (tenantId == Guid.Empty) throw new ArgumentException("Tenant ID cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(brandName)) throw new ArgumentException("Brand name is required.", nameof(brandName));

        Id = id;
        TenantId = tenantId;
        BrandName = brandName.Trim();
        PrimaryColorHex = primaryColorHex.Trim();
    }

    public void Update(
        string brandName,
        string? logoUrl,
        string primaryColorHex,
        string? headerText,
        string? footerText,
        string? taxRegNumber,
        bool showWifi,
        string? wifiSsid,
        string? wifiPass)
    {
        if (string.IsNullOrWhiteSpace(brandName)) throw new ArgumentException("Brand name is required.", nameof(brandName));

        BrandName = brandName.Trim();
        LogoUrl = logoUrl?.Trim();
        PrimaryColorHex = primaryColorHex.Trim();
        HeaderText = headerText?.Trim();
        FooterText = footerText?.Trim();
        TaxRegistrationNumber = taxRegNumber?.Trim();
        ShowWifiInfo = showWifi;
        WifiSsid = wifiSsid?.Trim();
        WifiPassword = wifiPass?.Trim();
    }
}
