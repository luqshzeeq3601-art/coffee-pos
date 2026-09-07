namespace CoffeePos.Application.DTOs;

public record TenantBrandingDto(
    Guid TenantId,
    string BrandName,
    string? LogoUrl,
    string PrimaryColorHex,
    string? HeaderText,
    string? FooterText,
    string? TaxRegistrationNumber,
    bool ShowWifiInfo,
    string? WifiSsid,
    string? WifiPassword);

public record UpdateBrandingRequest(
    string BrandName,
    string? LogoUrl,
    string PrimaryColorHex,
    string? HeaderText,
    string? FooterText,
    string? TaxRegistrationNumber,
    bool ShowWifiInfo,
    string? WifiSsid,
    string? WifiPassword);
