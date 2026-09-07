namespace CoffeePos.Application.DTOs;

public record TenantSubscriptionDto(
    Guid Id,
    Guid TenantId,
    string Tier,
    string Status,
    decimal MonthlyPriceMyr,
    int MaxOutlets,
    int MaxRegisters,
    IReadOnlyList<string> FeaturesEnabled,
    DateTime? TrialEndUtc,
    DateTime CurrentPeriodEndUtc);

public record MerchantOnboardRequest(
    string BusinessName,
    string OutletName,
    string OwnerName,
    string OwnerEmail,
    string Tier,
    string Currency,
    string? TaxRegistrationNumber);

public record EntitlementsDto(
    bool CanUseMyInvois,
    bool CanUseMultiOutlet,
    bool CanUseTimecards,
    bool CanUseAdvancedInventory,
    bool CanUseCustomBranding,
    int MaxOutlets,
    int MaxRegisters);

public record UpgradeSubscriptionRequest(
    string Tier);
