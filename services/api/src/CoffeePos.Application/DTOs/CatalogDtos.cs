namespace CoffeePos.Application.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Code,
    int SortOrder,
    bool IsActive);

public record ModifierDto(
    Guid Id,
    string Name,
    decimal PriceDelta,
    bool IsDefault,
    int SortOrder);

public record ModifierGroupDto(
    Guid Id,
    string Name,
    int MinSelections,
    int MaxSelections,
    bool IsRequired,
    IReadOnlyList<ModifierDto> Modifiers);

public record VariantDto(
    Guid Id,
    string Name,
    string? Sku,
    string? Barcode,
    decimal Price,
    decimal? CostPrice,
    int SortOrder);

public record ProductDto(
    Guid Id,
    Guid CategoryId,
    string? CategoryName,
    string Name,
    string Description,
    decimal BasePrice,
    bool IsTaxInclusive,
    Guid? TaxRateId,
    decimal? TaxRatePercent,
    IReadOnlyList<VariantDto> Variants,
    IReadOnlyList<ModifierGroupDto> ModifierGroups,
    bool IsActive);

public record TaxRateDto(
    Guid Id,
    string Name,
    decimal RatePercent,
    string Code,
    bool IsDefault,
    bool IsActive);

public record DiscountDto(
    Guid Id,
    string Name,
    string DiscountType,
    decimal Value,
    bool RequiresManagerApproval,
    bool IsActive);

public record CatalogResponseDto(
    IReadOnlyList<CategoryDto> Categories,
    IReadOnlyList<ProductDto> Products,
    IReadOnlyList<TaxRateDto> TaxRates,
    IReadOnlyList<DiscountDto> Discounts);
