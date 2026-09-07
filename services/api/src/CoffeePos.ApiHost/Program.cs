using CoffeePos.ApiHost.Endpoints;
using CoffeePos.ApiHost.Health;
using CoffeePos.ApiHost.Middleware;
using CoffeePos.Application.Common;
using CoffeePos.Application.Interfaces;
using CoffeePos.Infrastructure.Audit;
using CoffeePos.Infrastructure.Printing;
using CoffeePos.Infrastructure.Security;
using CoffeePos.Infrastructure.Services;
using CoffeePos.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Security & Cryptography
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<ITokenService>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var secret = config["Jwt:Secret"] ?? "CoffeePos_Development_Super_Secret_Key_At_Least_32_Chars!";
    return new JwtTokenService(secret);
});

// Printing & Receipt Formatter
builder.Services.AddSingleton<IReceiptFormatter, EscPosReceiptFormatter>();

// Tenancy & Current User (Scoped per HTTP request)
builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<ITenantContext>(sp => sp.GetRequiredService<TenantContext>());
builder.Services.AddScoped<ICurrentUser>(sp => sp.GetRequiredService<TenantContext>());

// In-Memory Data Store & Audit Sink
builder.Services.AddSingleton<IIdentityStore, InMemoryIdentityStore>();
builder.Services.AddSingleton<ICatalogStore, InMemoryCatalogStore>();
builder.Services.AddSingleton<IOrderStore, InMemoryOrderStore>();
builder.Services.AddSingleton<IPaymentStore, InMemoryPaymentStore>();
builder.Services.AddSingleton<IShiftStore, InMemoryShiftStore>();
builder.Services.AddSingleton<IInventoryStore, InMemoryInventoryStore>();
builder.Services.AddSingleton<IKitchenStore, InMemoryKitchenStore>();
builder.Services.AddSingleton<ILoyaltyStore, InMemoryLoyaltyStore>();
builder.Services.AddSingleton<IMyInvoisStore, InMemoryMyInvoisStore>();
builder.Services.AddSingleton<ITransferStore, InMemoryTransferStore>();
builder.Services.AddSingleton<ITimecardStore, InMemoryTimecardStore>();
builder.Services.AddSingleton<ISubscriptionStore, InMemorySubscriptionStore>();
builder.Services.AddSingleton<IAuditService, InMemoryAuditService>();

// Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPinAuthService, PinAuthService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IShiftService, ShiftService>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<ISyncService, SyncService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IKitchenService, KitchenService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ILoyaltyService, LoyaltyService>();
builder.Services.AddScoped<IMyInvoisService, MyInvoisService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<ITimecardService, TimecardService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();

var app = builder.Build();

// Middleware Pipeline
app.UseMiddleware<ProblemDetailsExceptionMiddleware>();
app.UseMiddleware<TenantResolutionMiddleware>();

// Route Endpoints
app.MapHealthEndpoints();
app.MapAuthEndpoints();
app.MapDeviceEndpoints();
app.MapCatalogEndpoints();
app.MapOrderEndpoints();
app.MapPaymentEndpoints();
app.MapShiftEndpoints();
app.MapReceiptEndpoints();
app.MapSyncEndpoints();
app.MapInventoryEndpoints();
app.MapKitchenEndpoints();
app.MapReportEndpoints();
app.MapLoyaltyEndpoints();
app.MapMyInvoisEndpoints();
app.MapTransferEndpoints();
app.MapTimecardEndpoints();
app.MapSaasEndpoints();

app.Run();
