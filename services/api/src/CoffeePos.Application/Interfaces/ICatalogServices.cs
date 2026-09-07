using CoffeePos.Application.DTOs;
using CoffeePos.Domain.Catalog;

namespace CoffeePos.Application.Interfaces;

public interface ICatalogStore
{
    Task<IReadOnlyList<Category>> GetCategoriesAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetProductsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<Product?> GetProductByIdAsync(Guid tenantId, Guid productId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaxRate>> GetTaxRatesAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Discount>> GetDiscountsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task SaveProductAsync(Product product, CancellationToken cancellationToken = default);
}

public interface ICatalogService
{
    Task<CatalogResponseDto> GetFullCatalogAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductDto>> GetProductsAsync(Guid? categoryId = null, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TaxRateDto>> GetTaxRatesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DiscountDto>> GetDiscountsAsync(CancellationToken cancellationToken = default);
}
