using CoffeePos.Application.Common;
using CoffeePos.Application.DTOs;
using CoffeePos.Application.Interfaces;

namespace CoffeePos.Infrastructure.Services;

public sealed class CatalogService : ICatalogService
{
    private readonly ICatalogStore _catalogStore;
    private readonly ITenantContext _tenantContext;

    public CatalogService(ICatalogStore catalogStore, ITenantContext tenantContext)
    {
        _catalogStore = catalogStore;
        _tenantContext = tenantContext;
    }

    private Guid RequireTenantId()
    {
        return _tenantContext.TenantId ?? throw new UnauthorizedAccessException("Tenant context is required for catalog access.");
    }

    public async Task<CatalogResponseDto> GetFullCatalogAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var categories = await GetCategoriesAsync(cancellationToken);
        var products = await GetProductsAsync(null, cancellationToken);
        var taxRates = await GetTaxRatesAsync(cancellationToken);
        var discounts = await GetDiscountsAsync(cancellationToken);

        return new CatalogResponseDto(categories, products, taxRates, discounts);
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var domainCategories = await _catalogStore.GetCategoriesAsync(tenantId, cancellationToken);
        return domainCategories.Select(c => new CategoryDto(c.Id, c.Name, c.Code, c.SortOrder, c.IsActive)).ToList();
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(Guid? categoryId = null, CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var domainProducts = await _catalogStore.GetProductsAsync(tenantId, cancellationToken);
        var categories = await _catalogStore.GetCategoriesAsync(tenantId, cancellationToken);
        var taxes = await _catalogStore.GetTaxRatesAsync(tenantId, cancellationToken);

        var filtered = categoryId.HasValue
            ? domainProducts.Where(p => p.CategoryId == categoryId.Value)
            : domainProducts;

        return filtered.Select(p =>
        {
            var cat = categories.FirstOrDefault(c => c.Id == p.CategoryId);
            var tax = taxes.FirstOrDefault(t => t.Id == p.TaxRateId);

            var variants = p.Variants.Select(v => new VariantDto(
                v.Id,
                v.Name,
                v.Sku,
                v.Barcode,
                v.Price,
                v.CostPrice,
                v.SortOrder)).ToList();

            var modGroups = p.ModifierGroups.Select(mg => new ModifierGroupDto(
                mg.Id,
                mg.Name,
                mg.MinSelections,
                mg.MaxSelections,
                mg.IsRequired,
                mg.Modifiers.Select(m => new ModifierDto(
                    m.Id,
                    m.Name,
                    m.PriceDelta,
                    m.IsDefault,
                    m.SortOrder)).ToList())).ToList();

            return new ProductDto(
                p.Id,
                p.CategoryId,
                cat?.Name,
                p.Name,
                p.Description,
                p.BasePrice,
                p.IsTaxInclusive,
                p.TaxRateId,
                tax?.RatePercent,
                variants,
                modGroups,
                p.IsActive);
        }).ToList();
    }

    public async Task<IReadOnlyList<TaxRateDto>> GetTaxRatesAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var domainTaxes = await _catalogStore.GetTaxRatesAsync(tenantId, cancellationToken);
        return domainTaxes.Select(t => new TaxRateDto(t.Id, t.Name, t.RatePercent, t.Code, t.IsDefault, t.IsActive)).ToList();
    }

    public async Task<IReadOnlyList<DiscountDto>> GetDiscountsAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = RequireTenantId();
        var domainDiscounts = await _catalogStore.GetDiscountsAsync(tenantId, cancellationToken);
        return domainDiscounts.Select(d => new DiscountDto(
            d.Id,
            d.Name,
            d.DiscountType.ToString(),
            d.Value,
            d.RequiresManagerApproval,
            d.IsActive)).ToList();
    }
}
