using CoffeePos.Application.Common;
using CoffeePos.Domain.Audit;
using CoffeePos.Domain.Catalog;
using CoffeePos.Domain.Common;
using CoffeePos.Domain.Devices;
using CoffeePos.Domain.Events;
using CoffeePos.Domain.Identity;
using CoffeePos.Domain.Inventory;
using CoffeePos.Domain.Kitchen;
using CoffeePos.Domain.Loyalty;
using CoffeePos.Domain.MyInvois;
using CoffeePos.Domain.Orders;
using CoffeePos.Domain.Payments;
using CoffeePos.Domain.Saas;
using CoffeePos.Domain.Shifts;
using CoffeePos.Domain.Staff;
using CoffeePos.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace CoffeePos.Infrastructure.Persistence;

public class CoffeePosDbContext : DbContext
{
    private readonly ITenantContext? _tenantContext;

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Outlet> Outlets => Set<Outlet>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    // Catalog DbSets
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Variant> Variants => Set<Variant>();
    public DbSet<ModifierGroup> ModifierGroups => Set<ModifierGroup>();
    public DbSet<Modifier> Modifiers => Set<Modifier>();
    public DbSet<TaxRate> TaxRates => Set<TaxRate>();
    public DbSet<Discount> Discounts => Set<Discount>();

    // Order DbSets
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLineItem> OrderItems => Set<OrderLineItem>();
    public DbSet<OrderLineModifier> OrderItemModifiers => Set<OrderLineModifier>();

    // Payment & Sales DbSets
    public DbSet<SalesTransaction> SalesTransactions => Set<SalesTransaction>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Refund> Refunds => Set<Refund>();

    // Shift DbSets
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<CashMovement> CashMovements => Set<CashMovement>();

    // Inventory DbSets
    public DbSet<StockItem> StockItems => Set<StockItem>();
    public DbSet<StockLedgerEntry> StockLedgerEntries => Set<StockLedgerEntry>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeItem> RecipeItems => Set<RecipeItem>();
    public DbSet<StockWastageEntry> StockWastageEntries => Set<StockWastageEntry>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<StockTransfer> StockTransfers => Set<StockTransfer>();
    public DbSet<StockTransferItem> StockTransferItems => Set<StockTransferItem>();

    // Kitchen DbSets
    public DbSet<KitchenChit> KitchenChits => Set<KitchenChit>();
    public DbSet<KitchenChitItem> KitchenChitItems => Set<KitchenChitItem>();

    // Loyalty DbSets
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<LoyaltyLedgerEntry> LoyaltyLedgerEntries => Set<LoyaltyLedgerEntry>();

    // MyInvois DbSets
    public DbSet<MyInvoisDocument> MyInvoisDocuments => Set<MyInvoisDocument>();

    // Staff Timecard DbSets
    public DbSet<StaffTimecard> StaffTimecards => Set<StaffTimecard>();

    // SaaS & Branding DbSets
    public DbSet<TenantSubscription> TenantSubscriptions => Set<TenantSubscription>();
    public DbSet<TenantBranding> TenantBrandings => Set<TenantBranding>();

    public CoffeePosDbContext(DbContextOptions<CoffeePosDbContext> options, ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoffeePosDbContext).Assembly);

        // Global Query Filter for Multi-Tenancy
        if (_tenantContext != null)
        {
            modelBuilder.Entity<Outlet>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Device>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<User>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Employee>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<AuditEvent>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<OutboxMessage>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<IdempotencyRecord>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);

            modelBuilder.Entity<Category>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Product>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Variant>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<ModifierGroup>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Modifier>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<TaxRate>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Discount>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);

            modelBuilder.Entity<Order>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<OrderLineItem>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<OrderLineModifier>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);

            modelBuilder.Entity<SalesTransaction>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Payment>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Refund>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);

            modelBuilder.Entity<Shift>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<CashMovement>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);

            modelBuilder.Entity<StockItem>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<StockLedgerEntry>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<Recipe>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<StockWastageEntry>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<PurchaseOrder>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<StockTransfer>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);

            modelBuilder.Entity<KitchenChit>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);

            modelBuilder.Entity<Customer>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<LoyaltyLedgerEntry>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);

            modelBuilder.Entity<MyInvoisDocument>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<StaffTimecard>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<TenantSubscription>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
            modelBuilder.Entity<TenantBranding>().HasQueryFilter(e => !_tenantContext.TenantId.HasValue || e.TenantId == _tenantContext.TenantId.Value);
        }
    }

    public async Task SetSessionTenantContextAsync(CancellationToken cancellationToken = default)
    {
        if (_tenantContext?.TenantId != null)
        {
            var outletParam = _tenantContext.OutletId.HasValue ? $"'{_tenantContext.OutletId.Value}'" : "NULL";
            var sql = $"SELECT set_tenant_context('{_tenantContext.TenantId.Value}', {outletParam});";
            await Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }
    }
}
