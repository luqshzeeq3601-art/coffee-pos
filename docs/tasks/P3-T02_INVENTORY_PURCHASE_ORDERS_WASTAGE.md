# Task Capsule: P3-T02 — Inventory Stock Receiving, Supplier POs & Wastage Logs

## 1. Objective & Observable Outcomes
- **Objective:** Implement supplier purchase order receiving, wastage write-down logging with reason codes (spillages, dial-in grinder calibration, expired items), cost impact tracking, and the interactive Admin Inventory Portal screen.
- **Outcome:** Complete stock replenishment and loss-tracking workflow across backend APIs (`CoffeePos.ApiHost`), EF Core configurations & in-memory stores (`CoffeePos.Infrastructure`), contracts (`@coffee-pos/contracts`), and Admin Portal (`apps/admin`).

## 2. Files & Ownership
- `packages/contracts/src/inventory.ts`: Exported `StockWastageDto`, `RecordWastageRequest`, `PurchaseOrderDto`, `PurchaseOrderItemDto`, `CreatePurchaseOrderRequest`, `WasteReason`, `PurchaseOrderStatus`.
- `services/api/src/CoffeePos.Domain/Inventory/`: `WasteReason.cs`, `StockWastageEntry.cs`, `PurchaseOrder.cs`.
- `services/api/src/CoffeePos.Application/DTOs/InventoryDtos.cs`: Application DTO records.
- `services/api/src/CoffeePos.Application/Interfaces/IInventoryServices.cs`: `IInventoryStore`, `IInventoryService` interfaces.
- `services/api/src/CoffeePos.Infrastructure/Persistence/Configurations/WastageAndPoConfigurations.cs`: EF Core mappings for `stock_wastage_entries`, `purchase_orders`, `purchase_order_items`.
- `services/api/src/CoffeePos.Infrastructure/Storage/InMemoryInventoryStore.cs`: Seeded wastage entries and purchase orders.
- `services/api/src/CoffeePos.Infrastructure/Services/InventoryService.cs`: Orchestrates waste write-downs, PO creation, stock receipt, and audit events.
- `services/api/migrations/009_purchase_orders_and_wastage.sql`: DDL with PostgreSQL RLS policies.
- `services/api/src/CoffeePos.ApiHost/Endpoints/InventoryEndpoints.cs`: Minimal APIs mapped at `/api/v1/inventory/wastage` and `/api/v1/inventory/purchase-orders`.
- `apps/admin/src/components/AdminShell.tsx` & `AdminShell.css`: Interactive Inventory workspace under `📦 Inventory & Stock` tab with Receive Stock modal, Log Wastage modal, and Live Stock table.

## 3. Interfaces & Invariants
- **Cost Impact Tracking:** Automated calculation of `QuantityWasted * CostPerUnit`.
- **Append-Only Movement Integration:** Every wastage write-down or PO receipt automatically appends to the stock movement ledger.
- **Role Permissions:** Wastage logging and receiving restricted to Manager/Owner.

## 4. Verification Evidence
- TypeScript compiler across 5 packages: `bun x tsc` returned code 0 (0 errors).
- Architectural boundaries check: `npm run check:boundaries` passed.
- .NET project references check: `pwsh scripts/check-dotnet-references.ps1` passed.
