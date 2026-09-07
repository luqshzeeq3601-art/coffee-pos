namespace CoffeePos.Domain.Identity;

public enum Role
{
    Owner = 1,
    Manager = 2,
    Cashier = 3,
    Barista = 4
}

public static class Permissions
{
    public const string PosCheckout = "pos:checkout";
    public const string PosRefund = "pos:refund";
    public const string PosVoid = "pos:void";
    public const string PosDiscount = "pos:discount";
    public const string PosCashDrawer = "pos:cash_drawer";
    public const string PosReprintReceipt = "pos:reprint_receipt";
    
    public const string CatalogRead = "catalog:read";
    public const string CatalogWrite = "catalog:write";
    
    public const string InventoryView = "inventory:view";
    public const string InventoryAdjust = "inventory:adjust";
    
    public const string ReportsView = "reports:view";
    public const string ReportsExport = "reports:export";
    
    public const string ShiftsOpen = "shifts:open";
    public const string ShiftsClose = "shifts:close";
    public const string ShiftsManage = "shifts:manage";
    
    public const string AdminUsers = "admin:users";
    public const string AdminDevices = "admin:devices";
    public const string AdminSettings = "admin:settings";
    
    public const string OverrideAuthorize = "override:authorize";

    public static IReadOnlySet<string> GetPermissionsForRole(Role role) => role switch
    {
        Role.Owner => new HashSet<string>
        {
            PosCheckout, PosRefund, PosVoid, PosDiscount, PosCashDrawer, PosReprintReceipt,
            CatalogRead, CatalogWrite,
            InventoryView, InventoryAdjust,
            ReportsView, ReportsExport,
            ShiftsOpen, ShiftsClose, ShiftsManage,
            AdminUsers, AdminDevices, AdminSettings,
            OverrideAuthorize
        },
        Role.Manager => new HashSet<string>
        {
            PosCheckout, PosRefund, PosVoid, PosDiscount, PosCashDrawer, PosReprintReceipt,
            CatalogRead, CatalogWrite,
            InventoryView, InventoryAdjust,
            ReportsView, ReportsExport,
            ShiftsOpen, ShiftsClose, ShiftsManage,
            AdminDevices,
            OverrideAuthorize
        },
        Role.Cashier => new HashSet<string>
        {
            PosCheckout, PosReprintReceipt,
            CatalogRead,
            InventoryView,
            ShiftsOpen, ShiftsClose
        },
        Role.Barista => new HashSet<string>
        {
            CatalogRead,
            InventoryView
        },
        _ => new HashSet<string>()
    };
}
